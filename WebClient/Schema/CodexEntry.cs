using Newtonsoft.Json.Linq;

namespace UGC_App.WebClient.Schema;

public class CodexEntry : SchemaFilter
{
    public CodexEntry(JObject inp)
    {
        Data["$schemaRef"] = "https://eddn.edcd.io/schemas/codexentry/1";
        Data["message"] = inp;
    }
}