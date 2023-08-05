namespace UGC_App.EDLog.Model;

public class StatusModel
{
    public int[] Pips { get; set; } = { 4, 4, 4 };
    public int FireGroup {get;set;}
    public int GuiFocus {get;set;}
    //public Fuel:{ FuelMain:30.664074, FuelReservoir:0.472058 } {get;set;}
    public double Cargo {get;set;}
    public string LegalState { get; set; } = string.Empty;
    public double Latitude {get;set;}
    public double Longitude {get;set;}
    public double Heading {get;set;}
    public double Altitude {get;set;}
    public string BodyName {get;set;} = string.Empty;
    public double PlanetRadius {get;set;}
    public ulong Balance {get;set;}
    //public Destination:{ System:3309029001579, Body:1, Name:Wapiya 1 } {get;set;}
}