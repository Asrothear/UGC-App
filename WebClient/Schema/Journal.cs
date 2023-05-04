using Newtonsoft.Json.Linq;

namespace UGC_App.WebClient.Schema
{
    public class Journal : SchemaFilter
    {
        public Journal(JObject inp)
        {
            Data["$schemaRef"] = "https://eddn.edcd.io/schemas/journal/1";
            Merge(inp);
            FilterEntry(Data);
        }
    }
}