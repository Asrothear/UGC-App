using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UGC_App.WebClient.Schema;

public class SchemaFilter
{
    protected JObject Data { get; init; }

    protected SchemaFilter()
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

    internal void GetCoords(JObject inp)
    {
        var datas = StateReceiver.GetSystemData(Convert.ToUInt64(inp["SystemAddress"]));
        Data["message"]!["StarPos"] = JToken.FromObject(JsonConvert.DeserializeObject<double[]>(datas?[1]!) ?? Array.Empty<double>());
    }
    internal void GetStarSystem(JObject inp)
    {
        var datas = StateReceiver.GetSystemData(Convert.ToUInt64(inp["SystemAddress"]));
        Data["message"]!["StarSystem"] = datas?[0];
    }
    internal void GetSystemMeta(JObject inp)
    {
        var datas = StateReceiver.GetSystemData(Convert.ToUInt64(inp["SystemAddress"]));
        Data["message"]!["StarSystem"] = datas?[0];
        Data["message"]!["StarPos"] = JToken.FromObject(JsonConvert.DeserializeObject<double[]>(datas?[1]!) ?? Array.Empty<double>());
    }
    
    internal void Merge(JObject inp)
    {
        if (Data["message"] is JObject message)
        {
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
}