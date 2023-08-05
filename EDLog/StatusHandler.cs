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
            status = JsonConvert.DeserializeObject<StatusModel>(Path.Combine(Config.Instance.PathJournal, "status.json"));
        }
        catch
        {
            EDDN.StatusBodyName = string.Empty;
        }
        if (status != null) EDDN.StatusBodyName = status.BodyName;
    }
}