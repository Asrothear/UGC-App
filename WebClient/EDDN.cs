using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace UGC_App.WebClient;

public static class EDDN
{
    internal static string JournalBodyName { get; set; } = string.Empty;
    internal static ulong JournalBodyId { get; set; }
    internal static string StatusBodyName { get; set; } = string.Empty;
    internal static void Send(dynamic payload, Mainframe? parrent)
    {
        //if(!Config.Instance.EDDN)return;
        Task.Run(() =>
        {
            if (Config.Instance.ExternTool)
            {
                parrent?.SetStatus("EDMC");
                //return;
            }
            if(payload.DontSend)return;
            string shef = payload.Data["$schemaRef"].ToString();
            string inp = JsonConvert.SerializeObject(payload.Data, Formatting.Indented);
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://eddn.edcd.io:4430/upload/");
            request.Content = new StringContent(inp, Encoding.UTF8, "application/json");
            var response = client.Send(request);
            switch (response.StatusCode)
            {
                case HttpStatusCode.RequestTimeout:
                    Program.Log(response.ToString());
                    Program.Log(inp);
                    parrent?.SetStatus(response);
                    break;
                case HttpStatusCode.ServiceUnavailable:
                    Program.Log(response.ToString());
                    Program.Log(inp);
                    parrent?.SetStatus(response);
                    break;
                case HttpStatusCode.BadRequest:
                    Program.Log(response.ToString());
                    Program.Log(inp);
                    parrent?.SetStatus(response);
                    break;
                case HttpStatusCode.RequestEntityTooLarge:
                    Program.Log(response.ToString());
                    Program.Log(inp);
                    parrent?.SetStatus(response);
                    break;
                case HttpStatusCode.OK:
                    parrent?.SetStatus(response);
                    break;
            }
        });
    }
}