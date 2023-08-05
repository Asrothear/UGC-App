using Newtonsoft.Json.Linq;

namespace UGC_App.WebClient.Schema;

public class FssSignalDiscovered : SchemaFilter
{
    public FssSignalDiscovered(JObject inp)
    {
        Data["$schemaRef"] = "https://eddn.edcd.io/schemas/fsssignaldiscovered/1/test";
        Data["message"] = inp;
    }
}