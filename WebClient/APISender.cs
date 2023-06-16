using System.Text;

namespace UGC_App.WebClient;

public class ApiSender
{
    internal static void SendApi(string payload, Mainframe? parent)
    {
        Task.Run(() =>
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, Config.Instance.SendUrl);
            request.Headers.Add("version", Config.Instance.Version);
            request.Headers.Add("branch", Config.Instance.VersionMeta);
            request.Headers.Add("cmdr", $"{Config.Instance.SendName}");
            request.Headers.Add("token", $"{Config.Instance.Token}");
            request.Content = new StringContent(payload, Encoding.UTF8, "application/json");
            try
            {
                client.Send(request);
                parent?.AddSucess();
            }
            catch
            {
                // ignored
            }
        });
    }
}