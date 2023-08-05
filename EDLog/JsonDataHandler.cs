using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UGC_App.EDLog;

public class JsonDataHandler
{
    internal static JObject? GetData(string file = "")
    {
        if (string.IsNullOrWhiteSpace(file)) return null;
        JObject? mats = null;
        try
        {
            var jsonContent = File.ReadAllText(Path.Combine(Config.Instance.PathJournal, file));
            mats = JsonConvert.DeserializeObject<JObject>(jsonContent);
        }catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
        return mats;
    }
}