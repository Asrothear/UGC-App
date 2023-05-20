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
            request.Headers.Add("br", Config.Instance.VersionMeta);
            request.Headers.Add("branch", $"standalone");
            request.Headers.Add("cmdr", $"{Config.Instance.SendName}");
            request.Headers.Add("token", $"{Config.Instance.Token}");
            request.Content = new StringContent(payload, Encoding.UTF8, "application/json");
            HttpResponseMessage response = new();
            try
            {
                response = client.Send(request);
                parent.AddSucess();
            }catch{}
        });
    }
}