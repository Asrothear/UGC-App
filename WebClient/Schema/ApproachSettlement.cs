using Newtonsoft.Json.Linq;

namespace UGC_App.WebClient.Schema;

public class ApproachSettlement : SchemaFilter
{
    public ApproachSettlement(JObject inp)
    {
        Data["$schemaRef"] = "https://eddn.edcd.io/schemas/approachsettlement/1";
        Data["message"] = inp;
        Data["message"]["StarPos"] = StateReceiver.GetSystemData(Convert.ToUInt64(inp["SystemAddress"]))[1];
        Data["message"]["StarSystem"] = StateReceiver.GetSystemData(Convert.ToUInt64(inp["SystemAddress"]))[0];
    }
}