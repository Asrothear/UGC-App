using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UGC_App.WebClient.Schema;

public class FSSDiscoveryScan : SchemaFilter
{
    public FSSDiscoveryScan(JObject inp)
    {
        Data["$schemaRef"] = "https://eddn.edcd.io/schemas/fssdiscoveryscan/1";
        Merge(inp);
        GetCoords(inp);
    }
}