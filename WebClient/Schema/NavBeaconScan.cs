using Newtonsoft.Json.Linq;

namespace UGC_App.WebClient.Schema;

public class NavBeaconScan : SchemaFilter
{
    public NavBeaconScan(JObject inp)
    {
        Data["$schemaRef"] = "https://eddn.edcd.io/schemas/navbeaconscan/1";
        Merge(inp);
        GetSystemMeta(inp);
    }
}