using Newtonsoft.Json.Linq;

namespace UGC_App.WebClient.Schema;

public class FssAllBodiesFound : SchemaFilter
{
    public FssAllBodiesFound(JObject inp)
    {
        Data["$schemaRef"] = "https://eddn.edcd.io/schemas/fssallbodiesfound/1";
        Merge(inp);
        GetCoords(inp);
    }
}