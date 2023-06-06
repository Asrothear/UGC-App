namespace UGC_App.Order.Model;

public class OrderList
{
    public ulong? SystemAddress { get; set; }
    public string Faction { get; set; }
    public string Type { get; set; }
    public int Priority { get; set; }
    public string Order { get; set; }
}