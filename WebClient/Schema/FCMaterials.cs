using Newtonsoft.Json.Linq;

namespace UGC_App.WebClient.Schema;

public class FCMaterials : SchemaFilter
{
    public FCMaterials(JObject inp)
    {
        Data["$schemaRef"] = "https://eddn.edcd.io/schemas/fcmaterials_journal/1";
        Data["message"] = inp;
    }
}