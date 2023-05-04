using Newtonsoft.Json.Linq;

namespace UGC_App.WebClient.Schema;

public class Market : SchemaFilter
{
    public Market(JObject inp)
    {
        Data["$schemaRef"] = "https://eddn.edcd.io/schemas/commodity/3";
        Data["message"] = inp;
    }
}