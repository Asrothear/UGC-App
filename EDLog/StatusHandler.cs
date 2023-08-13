using Newtonsoft.Json;
using UGC_App.EDLog.Model;
using UGC_App.WebClient;

namespace UGC_App.EDLog;

public class StatusHandler
{
    internal static void GetBodyStatus()
    {
        StatusModel? status = null;
        try
        {
            var str = JsonDataHandler.GetData("Status.json");
            status = JsonConvert.DeserializeObject<StatusModel>(str.ToString());
        }
        catch(Exception ex)
        {
            EDDN.StatusBodyName = string.Empty;
        }
        if (status != null) EDDN.StatusBodyName = status.BodyName;
    }
}