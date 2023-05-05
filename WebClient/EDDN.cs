using System.Diagnostics;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UGC_App.WebClient.Schema;

namespace UGC_App.WebClient;

public class EDDN
{
    internal static void Send(dynamic payload, Mainframe parrent)
    {
        //if(!Config.Instance.EDDN)return;
        Task.Run(() =>
        {
            var shef = payload.Data["$schemaRef"].ToString();
            payload.Data["$schemaRef"] = $"{shef}/test";
            var inp = JsonConvert.SerializeObject(payload.Data, Formatting.Indented);
            var Client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://eddn.edcd.io:4430/upload/");
            request.Content = new StringContent(inp, Encoding.UTF8, "application/json");
            var response = Client.Send(request);
            Program.Log(response.ToString());
            switch (response.StatusCode)
            {
                case HttpStatusCode.RequestTimeout:
                    parrent.SetStatus(response);
                    break;
                case HttpStatusCode.ServiceUnavailable:
                    parrent.SetStatus(response);
                    break;
                case HttpStatusCode.BadRequest:
                    parrent.SetStatus(response);
                    break;
                case HttpStatusCode.RequestEntityTooLarge:
                    parrent.SetStatus(response);
                    break;
                case HttpStatusCode.OK:
                    parrent.SetStatus(response);
                    break;
            }
        });
    }
}