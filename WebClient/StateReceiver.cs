using System.Net.Http.Json;

namespace UGC_App.WebClient;

public class StateReceiver
{
    internal static string[] SystemList = new string[0];
    internal static string[] Tick = new string[0];
    internal static string[] GetState()
    {
        var Client = new HttpClient();
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Config.Instance.State_Url);
        
        request.Headers.Add("version", Config.Instance.Version);
        request.Headers.Add("br", Config.Instance.Version_Meta);
        request.Headers.Add("branch", $"standalone");
        request.Headers.Add("cmdr", $"{Config.Instance.Send_Name}");
        request.Headers.Add("token", $"{Config.Instance.Token}");
        request.Headers.Add("onlyBGS", $"{Config.Instance.BGS_Only}");
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
            var rets = new String[1];
            rets[0] = response.Content.ToString();
            return rets;
        }
        SystemList = content;
        return Config.Instance.Show_All ? SystemList : SystemList.Take(Convert.ToInt32(Config.Instance.ListCount)).ToArray();
    }
    
    internal static string[] GetTick()
    {
        var Client = new HttpClient();
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Config.Instance.Tick_Url);
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

        Tick = content;
        return Tick;
    }

    internal static string[] GetSystemData(ulong Adress)
    {
        var Client = new HttpClient();
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"https://api.ugc-tools.de/api/v1/GetSystemData?SystemAdress={Adress}");
        HttpResponseMessage response = new();
        response = Client.Send(request);
        return response.Content.ReadFromJsonAsync<string[]>().Result;
    }
}