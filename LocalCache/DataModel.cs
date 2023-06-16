using UGC_App.Order.Model;

namespace UGC_App.LocalCache;

public class DataModel
{
    public OrdersCacheModel OrdersCahce { get; set; } = new();
    public HistoryChacheModel HistoryChache { get; set; } = new();
    public UserDataCacheModel UserDataCache { get; set; } = new();
    public SystemListDataCacheModel SystemListCache{ get; set; } = new();
}
public class OrdersCacheModel
{
    public DateTime LastUpdate { get; set; } = DateTime.MinValue;
    public HashSet<Orders> Order { get; set; } = new();
}
public class HistoryChacheModel
{
    public DateTime LastUpdate { get; set; } = DateTime.MinValue;
    public HashSet<SystemHistoryData> SystemHistoryData { get; set; } = new();
}
public class UserDataCacheModel
{
    public DateTime LastUpdate { get; set; } = DateTime.MinValue;
    public string Token { get; set; } = "";
}
public class SystemListDataCacheModel
{
    public DateTime LastUpdate { get; set; } = DateTime.MinValue;
    public HashSet<SystemListing> SystemList { get; set; } = new();
}