using Newtonsoft.Json;
using UGC_App.Order.Model;
using System.Security.Cryptography;
using System.Text;
using UGC_App.WebClient;

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
    internal static void InitAll(bool force = false)
    {
        File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "UGC-App", "cache.ugc"));
        Program.Log($"Update Cache, Forced = {force}");
        CacheOrder(force);
        CacheHistory(force);
        CacheSystemList(force);
        CacheUser(force);
    } 
    internal static HashSet<Orders> GetOrderFromCache()
    {
        var cache = GetCache(OrderCacheDataPath);
        return cache.OrdersCahce.Order;
    }
    internal static HashSet<SystemHistoryData> GetHistoryFromCache()
    {
        var cache = GetCache(HistoryCacheDataPath);
        return cache.HistoryChache.SystemHistoryData;
    }
    internal static HashSet<SystemListing> GetSystemListFromCache()
    {
        var cache = GetCache(SystemListCacheDataPath);
        return cache.SystemListCache.SystemList;
    }
    internal static void CacheOrder(bool force = false)
    {
        Program.Log($"Update Order-Cache, Forced = {force}");
        var cache = GetCache(OrderCacheDataPath);
        if (DateTime.UtcNow - cache.OrdersCahce.LastUpdate <= new TimeSpan(1, 0, 0) && !force) return;
        cache.OrdersCahce = new OrdersCacheModel();
            SaveOrderCache(cache, OrderAPI.GetAllOrders());
    }
    internal static void CacheHistory(bool force = false)
    {
        Program.Log($"Update History-Cache, Forced = {force}");
        var cache = GetCache(HistoryCacheDataPath);
        if (DateTime.UtcNow - cache.HistoryChache.LastUpdate <= new TimeSpan(1, 0, 0) && !force) return;
        cache.HistoryChache = new HistoryChacheModel();
        var syslist = OrderAPI.GetSystemList();
        foreach (var syshist in syslist.Select(system => OrderAPI.GetSystemHistory(system.SystemAddress)))
            SaveHistoryCache(cache, syshist);
    }
    internal static void CacheUser(bool force = false)
    {
        Program.Log($"Update User-Cache, Forced = {force}");
        var cache = GetCache(UserCacheDataPath);
        if (DateTime.UtcNow - cache.OrdersCahce.LastUpdate <= new TimeSpan(1, 0, 0) && !force) return;
        cache.OrdersCahce = new OrdersCacheModel();
        var syslist = OrderAPI.GetSystemList();
        foreach (var sysorder in syslist.Select(system => OrderAPI.GetSystemOrders(system.SystemAddress)))
            SaveOrderCache(cache, sysorder);
    }
    internal static void CacheSystemList(bool force = false)
    {
        Program.Log($"Update SystemList-Cache, Forced = {force}");
        var cache = GetCache(SystemListCacheDataPath);
        if (DateTime.UtcNow - cache.SystemListCache.LastUpdate <= new TimeSpan(1, 0, 0) && !force) return;
        cache.SystemListCache = new SystemListDataCacheModel();
        SaveSystemListCache(cache, OrderAPI.GetSystemList());
    }

    private static bool CheckDataExist(string path)
    {
        return File.Exists(path);
    }

    private static void CreateDataFile(string path)
    {
        if (CheckDataExist(path)) return;
        var file = File.Create(path);
        file.Dispose();
        Save(new DataModel(), path);
    }

    private static void Save(DataModel data, string path)
    {
        var encryptedBytes = Encrypt(JsonConvert.SerializeObject(data), Config.Instance.Token);
        File.WriteAllBytes(path, encryptedBytes);
    }

    private static void SaveOrderCache(DataModel cache, HashSet<Orders> ordersSet)
    {
        CreateDataFile(OrderCacheDataPath);
        foreach (var order in ordersSet) cache.OrdersCahce.Order.Add(order);
        cache.OrdersCahce.LastUpdate = DateTime.UtcNow;
        Save(cache, OrderCacheDataPath);
    }

    private static void SaveHistoryCache(DataModel cache, SystemHistoryData historyData)
    {
        CreateDataFile(HistoryCacheDataPath);
        var needle = cache.HistoryChache.SystemHistoryData.FirstOrDefault(x => x.systemAddress == historyData.systemAddress);
        if (needle != null) cache.HistoryChache.SystemHistoryData.Remove(needle);
        cache.HistoryChache.SystemHistoryData.Add(historyData);
        cache.HistoryChache.LastUpdate = DateTime.UtcNow;
        Save(cache,HistoryCacheDataPath);
    }

    private static void SaveUserCache(HashSet<Orders> ordersSet)
    {
        CreateDataFile(UserCacheDataPath);
    }

    private static void SaveSystemListCache(DataModel cache, HashSet<SystemListing> systemListings)
    {
        CreateDataFile(SystemListCacheDataPath);
        foreach (var system in systemListings) cache.SystemListCache.SystemList.Add(system);
        cache.SystemListCache.LastUpdate = DateTime.UtcNow;
        Save(cache,SystemListCacheDataPath);
    }

    private static DataModel GetCache(string path)
    {
        CreateDataFile(path);
        var data = Decrypt(File.ReadAllBytes(path), Config.Instance.Token);
        return JsonConvert.DeserializeObject<DataModel>(data) ?? new DataModel();
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