using System.Net.Http.Json;

namespace UGC_App.WecClient;

public class StateReceiver
{
    internal static string[] GetState()
    {
        var Client = new HttpClient();
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Properties.Settings.Default.State_Url);
        
        request.Headers.Add("version", $"1.0");
        request.Headers.Add("br", $"1");
        request.Headers.Add("branch", $"beta");
        request.Headers.Add("cmdr", $"{Properties.Settings.Default.Send_Name}");
        request.Headers.Add("token", $"{Properties.Settings.Default.Token}");
        request.Headers.Add("onlyBGS", $"{Properties.Settings.Default.BGS_Only}");
        HttpResponseMessage response = new();
        try
        {
            response = Client.Send(request);
        }
        catch
        {
            //ToDo: Add Error Handler for any Connection failures
            var er = new string[1];
            er[0] = $"Error: {response.Content}";
            return er;
        }
        var content = new string[0];
        if (response.IsSuccessStatusCode)
        {
            content = response.Content.ReadFromJsonAsync<string[]>().Result;
            Console.WriteLine(content);
        }
        else
        {
            Console.WriteLine($"Error: {response.StatusCode}");
        }
        return content;
    }
}