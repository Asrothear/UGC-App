using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UGC_App.WebClient.Schema;

public class ApproachSettlement : SchemaFilter
{
    public ApproachSettlement(JObject inp)
    {
        Data["$schemaRef"] = "https://eddn.edcd.io/schemas/approachsettlement/1";
        Merge(inp);
        GetSystemMeta(inp);
    }
}