﻿using Newtonsoft.Json.Linq;

namespace UGC_App.WebClient.Schema
{
    public class Journal
    {
        public JObject Data => data;
        private JObject data = new JObject();

        public Journal(JObject inp)
        {
            data["$schemaRef"] = "https://eddn.edcd.io/schemas/journal/1";
            if (data["header"] == null)
            {
                data["header"] = new JObject();
            }

            data["header"]["uploaderID"] = Config.Instance.CMDR;
            data["header"]["softwareName"] = "UGC-APP";
            data["header"]["softwareVersion"] = $"{Config.Instance.Version}{Config.Instance.Version_Meta}";
            data["header"]["gameversion"] = Config.Instance.GameVersion;
            data["header"]["gamebuild"] = Config.Instance.GameBuild;
            if (data["message"] == null)
            {
                data["message"] = new JObject();
            }

            data["message"]["horizons"] = Config.Instance.Horizons;
            data["message"]["odyssey"] = Config.Instance.Odyssey;

            data["message"] = inp;
            var item = data["message"]["Factions"];
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

            var items = data["message"] as JObject;
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

            RemoveKeysWithSubstring(data, "_Localised");

        }

        public static void RemoveKeysWithSubstring(JObject jsonObject, string substring)
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
}