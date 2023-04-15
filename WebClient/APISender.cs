using System.Text;

namespace UGC_App.WebClient;

public class APISender
{
    internal static void SendAPI(string payload, Mainframe parent)
    {
        Task.Run(() =>
        {
            var Client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, Config.Instance.Send_Url);
            request.Headers.Add("version", $"1,0");
            request.Headers.Add("br", $"1");
            request.Headers.Add("branch", $"standalone");
            request.Headers.Add("source", $"standalone");
            request.Headers.Add("token", $"{Config.Instance.Token}");
            request.Content = new StringContent(payload, Encoding.UTF8, "application/json");
            HttpResponseMessage response = new();
            try
            {
                response = Client.Send(request);
                parent.AddSucess();
            }catch{}
        });
    }
}