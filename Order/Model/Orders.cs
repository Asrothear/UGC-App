namespace UGC_App.Order.Model;

public class Orders
{
    public ulong SystemAddress { get; set; }
    public string StarSystem { get; set; }
    public string Faction { get; set; }
    public string Type { get; set; }
    public int Priority { get; set; }
    public string Order { get; set; }
    public DateTime TimeStamp { get; set; } = DateTime.MinValue;
}