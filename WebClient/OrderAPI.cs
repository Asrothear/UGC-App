﻿using System.Diagnostics;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using UGC_App.Order.DashViews;
using UGC_App.Order.Model;

namespace UGC_App.WebClient;

public class OrderAPI
{
    internal static dynamic GetSystemList()
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Get, Config.Instance.SystemDataUrl);
        var response = client.Send(request);
        
        dynamic? content = null;
        
        if (response.IsSuccessStatusCode)
        {
            content = response.Content.ReadFromJsonAsync<HashSet<dynamic>>().Result;
        }
        return  content;
    }
    internal static SystemHistoryData GetSystemHistory(string address)
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Get, $"{Config.Instance.SystemDataUrl}/{address}");
        var response = client.Send(request);
        
        dynamic? content = null;
        
        if (response.IsSuccessStatusCode)
        {
                content = response.Content.ReadFromJsonAsync<SystemHistoryData>().Result;
        }
        return  content;
    }
}