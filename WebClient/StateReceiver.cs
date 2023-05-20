using System.Diagnostics;
using System.Net.Http.Json;

namespace UGC_App.WebClient;

public class StateReceiver
{
    internal static string?[] SystemList = new string?[0];
    internal static string[]? Tick = new string[0];
    internal static string?[] GetState()
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Get, Config.Instance.StateUrl);
        
        request.Headers.Add("version", Config.Instance.Version);
        request.Headers.Add("br", Config.Instance.VersionMeta);
        request.Headers.Add("branch", $"standalone");
        request.Headers.Add("cmdr", $"{Config.Instance.SendName}");
        request.Headers.Add("token", $"{Config.Instance.Token}");
        request.Headers.Add("onlyBGS", $"{Config.Instance.BgsOnly}");
        HttpResponseMessage response = new();
        try
        {
            response = client.Send(request);
        }
        catch
        {
            //ToDo: Add Error Handler for any Connection failures
            string?[] er = new string[1];
            er[0] = $"Error: {response.Content}";
            return er;
        }
        var content = new string[0];
        if (response.IsSuccessStatusCode)
        {
            content = response.Content.ReadFromJsonAsync<string[]>().Result;
            Debug.WriteLine(content);
        }
        else
        {
            Debug.WriteLine($"Error: {response.StatusCode}");
            var rets = new string?[1];
            rets[0] = response.Content.ToString();
            return rets;
        }

        if (content != null) SystemList = content;
        return Config.Instance.ShowAll ? SystemList : SystemList.Take(Convert.ToInt32(Config.Instance.ListCount)).ToArray();
    }
    
    internal static string[]? GetTick()
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Get, Config.Instance.TickUrl);
        HttpResponseMessage response = new();
        try
        {
            response = client.Send(request);
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
            Debug.WriteLine(content);
        }
        else
        {
            Debug.WriteLine($"Error: {response.StatusCode}");
        }

        Tick = content;
        return Tick;
    }

    internal static string?[]? GetSystemData(ulong adress)
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.ugc-tools.de/api/v1/GetSystemData?SystemAdress={adress}");
        var response = client.Send(request);
        return response.Content.ReadFromJsonAsync<string[]>().Result;
    }
}