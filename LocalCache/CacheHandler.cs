using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;
using UGC_App.WebClient;
using UGC_App.Forms.Order.Model;

namespace UGC_App.LocalCache;

public static class CacheHandler
{
    private static string cahcefolder =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "UGC-App", "cache");
    static CacheHandler()
    {
        Directory.CreateDirectory(cahcefolder);
    }
    private static string SystemListCacheDataPath { get; set; } = Path.Combine(cahcefolder, "System.ugc");
    private static string OrderCacheDataPath { get; set; } = Path.Combine(cahcefolder, "Order.ugc");
    private static string UserCacheDataPath { get; set; } = Path.Combine(cahcefolder, "User.ugc");
    private static string HistoryCacheDataPath { get; set; } = Path.Combine(cahcefolder, "History.ugc");
    private static bool Block;
    internal static void InitAll(bool force = false)
    {
        if(Block)return;
        File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "UGC-App", "cache.ugc"));
        if(Config.Instance.Debug)Program.Log($"Update Cache, Forced = {force}");
        CacheOrder(force);
        CacheHistory(force);
        CacheSystemList(force);
        CacheUser(force);
    }

    internal static void DeleteAll()
    {
        Block = true;
        File.Delete(SystemListCacheDataPath);
        File.Delete(OrderCacheDataPath);
        File.Delete(UserCacheDataPath);
        File.Delete(HistoryCacheDataPath);
        Block = false;
    }
    internal static HashSet<Orders> GetOrderFromCache()
    {
        if(Block)return new HashSet<Orders>();
        var cache = GetCache<OrdersCacheModel>(OrderCacheDataPath);
        return cache.Order;
    }
    internal static HashSet<SystemHistoryData> GetHistoryFromCache()
    {
        if(Block)return new HashSet<SystemHistoryData>();
        var cache = GetCache<HistoryChacheModel>(HistoryCacheDataPath);
        return cache.SystemHistoryData;
    }
    internal static HashSet<SystemListing> GetSystemListFromCache()
    {
        if(Block)return new HashSet<SystemListing>();
        var cache = GetCache<SystemListDataCacheModel>(SystemListCacheDataPath);
        return cache.SystemList;
    }
    internal static void CacheOrder(bool force = false)
    {
        if(Block)return;
        if(Config.Instance.Debug)Program.Log($"Update Order-Cache, Forced = {force}");
        var cache = GetCache<OrdersCacheModel>(OrderCacheDataPath);
        if (DateTime.UtcNow - cache.LastUpdate <= new TimeSpan(0, 30, 0) && !force) return;
        SaveOrderCache<OrdersCacheModel>(new OrdersCacheModel(), OrderAPI.GetAllOrders());
    }
    internal static void CacheHistory(bool force = false)
    {
        if(Block)return;
        if(Config.Instance.Debug)Program.Log($"Update History-Cache, Forced = {force}");
        var cache = GetCache<HistoryChacheModel>(HistoryCacheDataPath);
        if (DateTime.UtcNow - cache.LastUpdate <= new TimeSpan(1, 0, 0) && !force) return;
        cache = new HistoryChacheModel();
        var syslist = OrderAPI.GetSystemList();
        foreach (var syshist in syslist.Select(system => OrderAPI.GetSystemHistory(system.SystemAddress))) 
            SaveHistoryCache<HistoryChacheModel>(cache, syshist);
    }
    internal static void CacheUser(bool force = false)
    {
        if(Block)return;
        if(Config.Instance.Debug)Program.Log($"Update User-Cache, Forced = {force}");
        var cache = GetCache<UserDataCacheModel>(UserCacheDataPath);
        if (DateTime.UtcNow - cache.LastUpdate <= new TimeSpan(1, 0, 0) && !force) return;
        cache = new UserDataCacheModel();
            SaveUserCache<UserDataCacheModel>(cache);
    }
    internal static void CacheSystemList(bool force = false)
    {
        if(Block)return;
        if(Config.Instance.Debug)Program.Log($"Update SystemList-Cache, Forced = {force}");
        var cache = GetCache<SystemListDataCacheModel>(SystemListCacheDataPath);
        if (DateTime.UtcNow - cache.LastUpdate <= new TimeSpan(1, 0, 0) && !force) return;
        cache = new SystemListDataCacheModel();
        SaveSystemListCache<SystemListDataCacheModel>(cache, OrderAPI.GetSystemList());
    }

    private static bool CheckDataExist(string path)
    {
        return File.Exists(path);
    }

    private static void CreateDataFile<T>(string path) where T : class, new()
    {
        if(Block)return;
        if (CheckDataExist(path)) return;
        var file = File.Create(path);
        file.Dispose();
        Save(new T(), path);
    }

    private static void Save(dynamic data, string path)
    {
        if(Block)return;
        var encryptedBytes = Encrypt(JsonConvert.SerializeObject(data), Config.Instance.Token);
        File.WriteAllBytes(path, encryptedBytes);
        //File.WriteAllText(path, JsonConvert.SerializeObject(data));
    }

    private static void SaveOrderCache<T>(OrdersCacheModel cache, HashSet<Orders> ordersSet) where T : class, new()
    {
        if(Block)return;
        CreateDataFile<T>(OrderCacheDataPath);
        foreach (var order in ordersSet) cache.Order.Add(order);
        cache.LastUpdate = DateTime.UtcNow;
        Save(cache, OrderCacheDataPath);
    }

    private static void SaveHistoryCache<T>(HistoryChacheModel cache, SystemHistoryData historyData) where T : class, new()
    {
        if(Block)return;
        CreateDataFile<T>(HistoryCacheDataPath);
        var needle = cache.SystemHistoryData.FirstOrDefault(x => x.systemAddress == historyData.systemAddress);
        if (needle != null) cache.SystemHistoryData.Remove(needle);
        cache.SystemHistoryData.Add(historyData);
        cache.LastUpdate = DateTime.UtcNow;
        Save(cache,HistoryCacheDataPath);
    }

    private static void SaveUserCache<T>(UserDataCacheModel ordersSet) where T : class, new()
    {
        if(Block)return;
        CreateDataFile<T>(UserCacheDataPath);
    }

    private static void SaveSystemListCache<T>(SystemListDataCacheModel cache, HashSet<SystemListing> systemListings) where T : class, new()
    {
        if(Block)return;
        CreateDataFile<T>(SystemListCacheDataPath);
        foreach (var system in systemListings) cache.SystemList.Add(system);
        cache.LastUpdate = DateTime.UtcNow;
        Save(cache,SystemListCacheDataPath);
    }

    private static dynamic GetCache <T>(string path) where T : class, new()
    {
        CreateDataFile<T>(path);
        var data = Decrypt(File.ReadAllBytes(path), Config.Instance.Token);
        return JsonConvert.DeserializeObject<T>(data) ?? new T();
        //return JsonConvert.DeserializeObject<T>(File.ReadAllText(path)) ?? new T();
    }
    private static byte[] Encrypt(string plainText, string encryptionKey)
    {
        using var aesAlg = Aes.Create();
        aesAlg.KeySize = 256;
        aesAlg.BlockSize = 128;
        aesAlg.GenerateIV();
        var keyBytes = Encoding.UTF8.GetBytes(encryptionKey);
        Array.Resize(ref keyBytes, 32);
        aesAlg.Key = keyBytes;
        var iv = aesAlg.IV;
        using var encryptor = aesAlg.CreateEncryptor();
        using var msEncrypt = new MemoryStream();
        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
        {
            using var swEncrypt = new StreamWriter(csEncrypt);
            swEncrypt.Write(plainText);
        }
        var cipherText = msEncrypt.ToArray();
        var encryptedBytes = new byte[iv.Length + cipherText.Length];
        Buffer.BlockCopy(iv, 0, encryptedBytes, 0, iv.Length);
        Buffer.BlockCopy(cipherText, 0, encryptedBytes, iv.Length, cipherText.Length);
        return encryptedBytes;
    }
    private static string Decrypt(byte[] cipherText, string encryptionKey)
    {
        using var aesAlg = Aes.Create();
        aesAlg.KeySize = 256;
        aesAlg.BlockSize = 128;
        var keyBytes = Encoding.UTF8.GetBytes(encryptionKey);
        Array.Resize(ref keyBytes, 32);
        aesAlg.Key = keyBytes;
        var iv = new byte[16];
        Buffer.BlockCopy(cipherText, 0, iv, 0, iv.Length);
        var encryptedData = new byte[cipherText.Length - iv.Length];
        Buffer.BlockCopy(cipherText, iv.Length, encryptedData, 0, encryptedData.Length);
        aesAlg.IV = iv;
        using var decryptor = aesAlg.CreateDecryptor();
        using var msDecrypt = new MemoryStream(encryptedData);
        using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
        using var srDecrypt = new StreamReader(csDecrypt);
        var plainText = srDecrypt.ReadToEnd();
        return plainText;
    }
}