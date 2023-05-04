using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UGC_App.WebClient.Schema;

public class ApproachSettlement : SchemaFilter
{
    public ApproachSettlement(JObject inp)
    {
        Data["$schemaRef"] = "https://eddn.edcd.io/schemas/approachsettlement/1";
        Merge(inp);
        var Datas = StateReceiver.GetSystemData(Convert.ToUInt64(inp["SystemAddress"]));
        Data["message"]["StarSystem"] = Datas[0];
        Data["message"]["StarPos"] = JToken.FromObject(JsonConvert.DeserializeObject<double[]>(Datas[1]));
    }
}