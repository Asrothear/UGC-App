using Newtonsoft.Json.Linq;

namespace UGC_App.WebClient.Schema;

public class ScanBaryCentre : SchemaFilter
{
    public ScanBaryCentre(JObject inp)
    {
        Data["$schemaRef"] = "https://eddn.edcd.io/schemas/scanbarycentre/1";
        Merge(inp);
        GetCoords(inp);
    }
}