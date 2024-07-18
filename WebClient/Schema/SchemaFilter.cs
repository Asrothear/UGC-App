using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UGC_App.WebClient.Schema;

public class SchemaFilter
{
    internal bool DontSend { get; private set; }
    internal JObject Data { get; private set; }

    internal SchemaFilter()
    {
        Data ??= new JObject();
        Data["header"] ??= new JObject();

        Data["header"]!["uploaderID"] = Config.Instance.Cmdr;
        Data["header"]!["softwareName"] = "UGC App";
        Data["header"]!["softwareVersion"] = $"{Config.Instance.Version}{Config.Instance.VersionMeta}";
        Data["header"]!["gameversion"] = Config.Instance.GameVersion;
        Data["header"]!["gamebuild"] = Config.Instance.GameBuild;
        Data["message"] ??= new JObject();
        Data["message"]!["horizons"] = Config.Instance.Horizons;
        Data["message"]!["odyssey"] = Config.Instance.Odyssey;
        
        FilterEntry(Data);
        RemoveKeysWithSubstring(Data, "_Localised");
    }
    internal static void FilterEntry(JObject jsonObject)
    {
        if (jsonObject["message"] is JObject items)
        {
            if (string.IsNullOrWhiteSpace(Config.Instance.GameVersion) &&
                string.IsNullOrWhiteSpace(Config.Instance.GameBuild))
            {
                items.Remove("horizons");
                items.Remove("odyssey");
            }
            items.Remove("ActiveFine");
            items.Remove("BoostUsed");
            items.Remove("FuelLevel");
            items.Remove("FuelUsed");
            items.Remove("JumpDist");
            items.Remove("Latitude");
            items.Remove("Longitude");
            items.Remove("Wanted");
            items.Remove("IsNewEntry");
            items.Remove("NewTraitsDiscovered");
            items.Remove("Traits");
            items.Remove("VoucherAmount");
        }
        var item = jsonObject["message"]?["Factions"];
        if (item == null) return;
        foreach (var ite in item)
        {
            var factionObject = ite.Value<JObject>();
            if (factionObject == null) continue;
            factionObject.Remove("HappiestSystem");
            factionObject.Remove("HomeSystem");
            factionObject.Remove("MyReputation");
            factionObject.Remove("SquadronFaction");
        }
    }
    
    internal static void FilterMarket(JObject jsonObject)
    {
        if (jsonObject["message"] is JObject items)
        {
            items.Remove("StationType");
            items.Remove("CarrierDockingAccess");
            RenameKeys(items,"StationName","stationName");
            RenameKeys(items,"StarSystem","systemName");
            RenameKeys(items,"MarketID","marketId");
        }
        var item = jsonObject["message"]?["commodities"];
        if (item == null) return;
        foreach (var ite in item)
        {
            var itemObject = ite.Value<JObject>();
            if (itemObject == null) continue;
            if (itemObject["categoryname"]?.ToString() == "NonMarketable")
            {
                ite.Remove();
                continue;
            }
            itemObject.Remove("Producer");
            itemObject.Remove("Rare");
            itemObject.Remove("id");
            itemObject.Remove("Category_Localised");
            itemObject.Remove("Category");
            itemObject.Remove("Consumer");
            itemObject["Name"] = itemObject["Name"]?.ToString().Replace("$", "").Replace("_name;", "");
            RenameKeys(itemObject,"Name","name");
            RenameKeys(itemObject,"MeanPrice","meanPrice");
            RenameKeys(itemObject,"BuyPrice","buyPrice");
            RenameKeys(itemObject,"Stock","stock");
            RenameKeys(itemObject,"StockBracket","stockBracket");
            RenameKeys(itemObject,"SellPrice","sellPrice");
            RenameKeys(itemObject,"Demand","demand");
            RenameKeys(itemObject,"DemandBracket","demandBracket");
        }
    }
    
    private static void RemoveKeysWithSubstring(JObject jsonObject, string substring)
    {
        var properties = jsonObject.Properties().ToList();

        foreach (var property in properties)
        {
            if (property.Name.Contains(substring))
            {
                jsonObject.Remove(property.Name);
            }
            else if (property.Value.Type == JTokenType.Object)
            {
                RemoveKeysWithSubstring((JObject)property.Value, substring);
            }
            else if (property.Value.Type == JTokenType.Array)
            {
                // ReSharper disable once HeapView.BoxingAllocation
                foreach (var item in property.Value.Children().Where(c => c.Type == JTokenType.Object))
                {
                    RemoveKeysWithSubstring((JObject)item, substring);
                }
            }
        }
    }
    internal static void RenameKeys(JObject jsonObject, string oldKeyName, string newKeyName)
    {
        var oldValue = jsonObject[oldKeyName];
        if (oldValue == null) return;
        jsonObject.Remove(oldKeyName);
        jsonObject.Add(newKeyName, oldValue);
    }
    internal void GetCoords(JObject inp)
    {
        var post = Data["message"]!["StarPos"]?.ToString().Split(",").ToList();
        if (post != null)
        {
            foreach (var cord in post.Where(string.IsNullOrWhiteSpace))
            {
                DontSend = true;
            }
            return;
        }
        var datas = StateReceiver.GetSystemData(Convert.ToUInt64(inp["SystemAddress"]));
        if(string.IsNullOrWhiteSpace(datas[0]))DontSend = true;
        var pos = JsonConvert.DeserializeObject<double[]>(datas[1]);
        if (pos == null) DontSend = true;
        Data["message"]!["StarPos"] = JToken.FromObject(pos);
    }
    internal void GetStarSystem(JObject inp)
    {
        var syst = Data["message"]!["StarSystem"]?.ToString();
        if(!string.IsNullOrWhiteSpace(syst))return;
        var datas = StateReceiver.GetSystemData(Convert.ToUInt64(inp["SystemAddress"]));
        if (string.IsNullOrWhiteSpace(datas[0])) DontSend = true;
        Data["message"]!["StarSystem"] = datas?[0];
    }
    internal void GetSystemMeta(JObject inp)
    {
        GetStarSystem(inp);
        GetCoords(inp);
    }
    internal void CheckBodyMeta(JObject inp)
    {
        var temp = Data["message"]!["BodyName"];
        if (Data["message"] is JObject message)
        {
            message.Remove("BodyName");
            message.Remove("BodyID");
            message.Remove("IsNewEntry");
            message.Remove("NewTraitsDiscovered");
        }
        if (string.IsNullOrWhiteSpace(EDDN.StatusBodyName))return;
        Data["message"]!["BodyName"] = EDDN.StatusBodyName;
        if (EDDN.StatusBodyName == EDDN.JournalBodyName) Data["message"]!["BodyID "] = EDDN.JournalBodyId;
    }
    
    internal void Merge(JObject inp)
    {
        if (Data["message"] is JObject message)
        {
            message.Remove("CarrierDockingAccess");
            message.Merge(inp, new JsonMergeSettings
            {
                MergeArrayHandling = MergeArrayHandling.Merge
            });
            Data["message"] = message;
        }
        else
        {
            Data["message"] = inp;
        }
        RemoveKeysWithSubstring(Data, "_Localised");
    }
    internal void MergeMarket(JObject inp)
    {
        var items = inp["Items"];
        inp.Remove("Items");
        Merge(inp);
        if (Data["message"] is JObject message)
        {
            message.Remove("event");
            message["commodities"] ??= new JObject();
        }
        Data["message"]!["commodities"] = items;
        
        if (items == null || !items.Any()) DontSend = true;
        RemoveKeysWithSubstring(Data, "_Localised");
    }
    internal void ParseModuleArray(JObject inp)
    {
        var items = inp["Items"];
        inp.Remove("Items");
        Merge(inp);
        if (Data["message"] is JObject message)
        {
            RenameKeys(message,"StarSystem","systemName");
            RenameKeys(message,"StationName","stationName");
            RenameKeys(message,"MarketID","marketId");
            message.Remove("CarrierDockingAccess");
            message.Remove("event");
            message.Remove("Horizons");
            message["modules"] ??= new JObject();
        }
        HashSet<JToken> list = new();
        foreach (var item in items)
        {
            var name = item["Name"]?.ToString();
            if(string.IsNullOrWhiteSpace(name))continue;
            var sitem = item.ToString();
            if (JObject.Parse(item.ToString()).TryGetValue("sku", out var starSystem))
            {
                if(item["sku"]==null && name.ToLower()!= "ELITE_HORIZONS_V_PLANETARY_LANDINGS".ToLower())continue;
            }
            
            if (name.ToLower() == "ELITE_HORIZONS_V_PLANETARY_LANDINGS".ToLower())
            {
                list.Add(name);
                continue;
            }
            if(name.ToLower().Contains("hpt_") || name.Contains("int_") || name.Contains("_armour_") && !string.Equals(name.ToLower(), "Int_PlanetApproachSuite".ToLower(), StringComparison.CurrentCultureIgnoreCase))list.Add(name);
        }
        if(Config.Instance.Debug)Program.Log(JsonConvert.SerializeObject(list));
        if(Config.Instance.Debug)Program.Log(list.Any().ToString());
        if (!list.Any()) DontSend = true;
        if(Config.Instance.Debug)Program.Log(DontSend.ToString());
        Data["message"]!["modules"] = new JArray(list);
        RemoveKeysWithSubstring(Data, "_Localised");
    }
    internal void ParseShipArray(JObject inp)
    {
        var items = inp["PriceList"];
        inp.Remove("PriceList");
        Merge(inp);
        if (Data["message"] is JObject message)
        {
            RenameKeys(message,"StarSystem","systemName");
            RenameKeys(message,"StationName","stationName");
            RenameKeys(message,"MarketID","marketId");
            RenameKeys(message,"AllowCobraMkIV","allowCobraMkIV");
            message.Remove("CarrierDockingAccess");
            message.Remove("event");
            message.Remove("Horizons");
            message["ships"] ??= new JObject();
        }
        HashSet<JToken> list = new();
        foreach (var item in items)
        {
            var name = item["ShipType"]?.ToString();
            list.Add(name);
        }
        if (!list.Any()) DontSend = true;
        Data["message"]!["ships"] = new JArray(list);
        RemoveKeysWithSubstring(Data, "_Localised");
    }
}