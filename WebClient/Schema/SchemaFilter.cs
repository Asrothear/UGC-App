using Newtonsoft.Json.Linq;

namespace UGC_App.WebClient.Schema;

public class SchemaFilter
{
    public JObject Data { get; init; }
    public SchemaFilter()
    {
        if (Data == null) Data = new();
        if (Data["header"] == null)
        {
            Data["header"] = new JObject();
        }

        Data["header"]["uploaderID"] = Config.Instance.CMDR;
        Data["header"]["softwareName"] = "UGC-APP";
        Data["header"]["softwareVersion"] = $"{Config.Instance.Version}{Config.Instance.Version_Meta}";
        Data["header"]["gameversion"] = Config.Instance.GameVersion;
        Data["header"]["gamebuild"] = Config.Instance.GameBuild;
        if (Data["message"] == null)
        {
            Data["message"] = new JObject();
        }

        Data["message"]["horizons"] = Config.Instance.Horizons;
        Data["message"]["odyssey"] = Config.Instance.Odyssey;
        
        FilterEntry(Data);
        RemoveKeysWithSubstring(Data, "_Localised");
    }
    static void FilterEntry(JObject jsonObject)
    {
        var items = jsonObject["message"] as JObject;
        if (items != null)
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
        var item = jsonObject["message"]["Factions"];
        if (item != null)
        {
            foreach (var ite in item)
            {
                var factionObject = ite.Value<JObject>();
                if (factionObject != null)
                {
                    factionObject.Remove("HappiestSystem");
                    factionObject.Remove("HomeSystem");
                    factionObject.Remove("MyReputation");
                    factionObject.Remove("SquadronFaction");
                }
            }
        }
    }
    static void RemoveKeysWithSubstring(JObject jsonObject, string substring)
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
                foreach (var item in property.Value.Children().Where(c => c.Type == JTokenType.Object))
                {
                    RemoveKeysWithSubstring((JObject)item, substring);
                }
            }
        }
    }
}