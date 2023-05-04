using Newtonsoft.Json.Linq;

namespace UGC_App.WebClient.Schema;

public class FSSBodySignals : SchemaFilter
{
    public FSSBodySignals(JObject inp)
    {
        Data["$schemaRef"] = "https://eddn.edcd.io/schemas/approachsettlement/1";
        Data["message"] = inp;
    }
}