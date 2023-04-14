using System.Net.Http.Json;

namespace UGC_App.WebClient;

public class StateReceiver
{
    internal static string[] SystemList = new string[0];
    internal static string[] Tick = new string[0];
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
            var rets = new String[1];
            rets[0] = response.Content.ToString();
            return rets;
        }
        SystemList = content;
        return Properties.Settings.Default.Show_All ? SystemList : SystemList.Take(Convert.ToInt32(Properties.Settings.Default.ListCount)).ToArray();
    }
    
    internal static string[] GetTick()
    {
        var Client = new HttpClient();
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Properties.Settings.Default.Tick_Url);
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
}