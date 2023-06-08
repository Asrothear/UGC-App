using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using Newtonsoft.Json;
using UGC_App.Order.DashViews;
using UGC_App.Order.Model;

namespace UGC_App.WebClient;

public class OrderAPI
{
    internal static dynamic GetSystemList()
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Get, Config.Instance.SystemDataUrl);
        request.Headers.Add("token", $"{Config.Instance.Token}");
        var response = client.Send(request);

        dynamic? content = null;

        if (response.IsSuccessStatusCode)
        {
            content = response.Content.ReadFromJsonAsync<HashSet<dynamic>>().Result;
        }

        return content;
    }

    internal static SystemHistoryData GetSystemHistory(ulong address)
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Get, $"{Config.Instance.SystemDataUrl}/{address}");
        request.Headers.Add("token", $"{Config.Instance.Token}");
        var response = client.Send(request);

        dynamic? content = null;

        if (response.IsSuccessStatusCode)
        {
            content = response.Content.ReadFromJsonAsync<SystemHistoryData>().Result;
        }

        return content;
    }
    internal static HashSet<Orders> GetSystemOrders(ulong address)
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Get, $"{Config.Instance.SystemOrdersUrl}/{address}");
        request.Headers.Add("token", $"{Config.Instance.Token}");
        var response = client.Send(request);

        HashSet<Orders> content = new();

        if (response.IsSuccessStatusCode)
        {
            content = response.Content.ReadFromJsonAsync<HashSet<Orders>>().Result;
        }
        return content;
    }
    internal static HashSet<Orders> GetAllOrders()
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Get, $"{Config.Instance.SystemOrdersUrl}");
        request.Headers.Add("token", $"{Config.Instance.Token}");
        var response = client.Send(request);

        HashSet<Orders> content = new();

        if (response.IsSuccessStatusCode)
        {
            content = response.Content.ReadFromJsonAsync<HashSet<Orders>>().Result;
        }
        return content;
    }
    internal static bool SaveOrders(Orders order)
    {
        var payload = new HashSet<Orders>();
        payload.Add(order);
        var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Post, $"{Config.Instance.SystemOrdersUrl}");
        var inp = JsonConvert.SerializeObject(payload, Formatting.Indented);
        request.Content = new StringContent(inp, Encoding.UTF8, "application/json");
        request.Headers.Add("token", $"{Config.Instance.Token}");
        var response = client.Send(request);
        return response.StatusCode switch
        {
            HttpStatusCode.OK => true,
            _ => false
        };
    }
}