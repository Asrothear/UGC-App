using Newtonsoft.Json.Linq;

namespace UGC_App.WebClient.Schema;

public class FSSSignalDiscovered : SchemaFilter
{
    public FSSSignalDiscovered(JObject inp)
    {
        Data["$schemaRef"] = "https://eddn.edcd.io/schemas/approachsettlement/1";
        Data["message"] = inp;
    }
}