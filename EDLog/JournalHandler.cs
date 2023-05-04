using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGC_App.WebClient;
using UGC_App.WebClient.Schema;

namespace UGC_App.EDLog
{
    internal class JournalHandler
    {
        private static Mainframe parent;
        private static string _currentFile;
        private static long _previousPosition;
        private static DateTime _lastCheckedTime;
        internal static bool running = false;
        private static string Path = Environment.ExpandEnvironmentVariables(@"%USERPROFILE%\Saved Games\Frontier Developments\Elite Dangerous\");

        internal static void Start(Mainframe parrent)
        {
            if (running) return;
            running = true;
            parent = parrent;
            var prefix = "Journal";
            var pollingInterval = TimeSpan.FromSeconds(5);
            SwitchToLatestFile(Path, prefix);
            Debug.WriteLine("Watchdog running...");
            while (running)
            {
                
                parent.SetLightActive(parent.yellowLight, true);
                parent.SetLightActive(parent.greenLight, false);
                
                CheckForLatestFile(Path, prefix);
                CheckForFileChanges();
                
                parent.SetLightActive(parent.yellowLight, false);
                parent.SetLightActive(parent.greenLight, true);
                Thread.Sleep(pollingInterval);
            }
        }

        private static void SwitchToLatestFile(string directoryPath, string prefix)
        {
            var latestFile = GetLatestFile(directoryPath, prefix);

            if (latestFile != null)
            {
                _currentFile = latestFile;
                _previousPosition = new FileInfo(_currentFile).Length;
                _lastCheckedTime = File.GetLastWriteTimeUtc(_currentFile);
                Debug.WriteLine($"Überwache aktuellste Datei: {_currentFile}");
            }
            else
            {
                Debug.WriteLine("Keine passende Datei gefunden.");
            }
        }

        private static string? GetLatestFile(string directoryPath, string prefix)
        {
            return Directory.GetFiles(directoryPath, $"{prefix}.*").MaxBy(File.GetCreationTimeUtc);
        }

        private static void CheckForLatestFile(string directoryPath, string prefix)
        {
            var latestFile = GetLatestFile(directoryPath, prefix);

            if (latestFile != null && latestFile != _currentFile)
            {
                Debug.WriteLine($"Wechsel zur neuesten Datei: {latestFile}");
                CheckForFileChanges();
                _currentFile = latestFile;
                _previousPosition = 0;
                _lastCheckedTime = File.GetLastWriteTimeUtc(_currentFile);
            }
        }

        private static void CheckForFileChanges()
        {
            if (_currentFile == null) return;

            var lastWriteTime = File.GetLastWriteTimeUtc(_currentFile);

            if (lastWriteTime > _lastCheckedTime)
            {
                Debug.WriteLine($"Datei '{_currentFile}' wurde geändert.");
                string jsonContent;
                using (var stream = new FileStream(_currentFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    stream.Seek(_previousPosition, SeekOrigin.Begin);
                    using (var reader = new StreamReader(stream))
                    {
                        jsonContent = reader.ReadToEnd();
                        _previousPosition = stream.Position;
                    }                    
                }
                ProcessJsonContent(jsonContent);

                _lastCheckedTime = lastWriteTime;
            }
        }

        private static void ProcessJsonContent(string jsonContent)
        {
            var jsonObjects = ParseJsonObjects(jsonContent);
            foreach (var jsonObject in jsonObjects)
            {
                Debug.WriteLine(jsonObject.GetValue("event"));
                if (jsonObject.ContainsKey("event"))
                {
                    switch (jsonObject["event"].ToString())
                    {
                        case "Fileheader":
                            Config.Instance.GameVersion = ToString(jsonObject["gameversion"]);
                            Config.Instance.GameBuild= ToString(jsonObject["build"]);
                            break;
                        case "LoadGame":
                            Config.Instance.CMDR = ToString(jsonObject["Commander"]);
                            Config.Instance.GameVersion = ToString(jsonObject["gameversion"]);
                            Config.Instance.GameBuild = ToString(jsonObject["build"]);
                            Config.Instance.Horizons = Convert.ToBoolean(jsonObject["Horizons"]);
                            Config.Instance.Odyssey = Convert.ToBoolean(jsonObject["Odyssey"]);
                            parent.SetCMDrText(Config.Instance.CMDR);
                            break;
                        case "Location":
                            Config.Instance.LastSystem = ToString(jsonObject["StarSystem"]);
                            parent.SetSystemText(Config.Instance.LastSystem);
                            if (Convert.ToBoolean(jsonObject["Docked"]))
                            {
                                Config.Instance.LastDocked = ToString(jsonObject["StationName"]);
                            }
                            else
                            {
                                Config.Instance.LastDocked = "-";
                            }
                            parent.SetDockedText(Config.Instance.LastDocked);
                            EDDN.Send(new Journal(jsonObject), parent);
                            break;
                        case "Docked":
                            Config.Instance.LastDocked = ToString(jsonObject["StationName"]);
                            parent.SetDockedText(Config.Instance.LastDocked);
                            EDDN.Send(new Journal(jsonObject), parent);
                            break;
                        case "Undocked":
                            Config.Instance.LastDocked = "-";
                            parent.SetDockedText(Config.Instance.LastDocked);
                            break;
                        case "FSDJump":
                            Config.Instance.LastSystem = ToString(jsonObject["StarSystem"]);
                            Config.Instance.LastDocked = "-";
                            EDDN.Send(new Journal(jsonObject), parent);
                            break;
                        case "ApproachSettlement":
                            EDDN.Send(new ApproachSettlement(jsonObject), parent);
                            break;
                        /*
                        case "CodexEntry":
                            EDDN.Send(new CodexEntry(jsonObject), parent);
                            break;
                        case "Market":
                            EDDN.Send(new Market(jsonObject), parent);
                            break;
                        case "FCMaterials":
                            EDDN.Send(new FCMaterials(jsonObject), parent);
                            break;
                        case "FSSAllBodiesFound":
                            EDDN.Send(new FSSAllBodiesFound(jsonObject), parent);
                            break;
                        case "FSSBodySignals":
                            EDDN.Send(new FSSBodySignals(jsonObject), parent);
                            break;
                        case "FSSDiscoveryScan":
                            EDDN.Send(new FSSDiscoveryScan(jsonObject), parent);
                            break;
                        case "FSSSignalDiscovered":
                            EDDN.Send(new FSSSignalDiscovered(jsonObject), parent);
                            break;
                        case "NavBeaconScan":
                            EDDN.Send(new NavBeaconScan(jsonObject), parent);
                            break;
                        case "NavRoute":
                            EDDN.Send(new NavRoute(jsonObject), parent);
                            break;
                        case "Outfitting":
                            EDDN.Send(new Outfitting(jsonObject), parent);
                            break;
                        case "Shipyard":
                            EDDN.Send(new Shipyard(jsonObject), parent);
                            break;*/
                    }
                    Config.Save();
                    parent.SetSystemText(Config.Instance.LastSystem);
                    parent.SetDockedText(Config.Instance.LastDocked);
                }
                jsonObject["user"] = Config.Instance.CMDR;
                APISender.SendAPI(jsonObject.ToString(),parent);
            }
        }

        private static string? ToString(JToken? inp)
        {
            return inp?.ToString();
        }
        private static List<JObject> ParseJsonObjects(string input)
        {
            var jsonObjects = new List<JObject>();
            var lines = input.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                var jsonObject = JsonConvert.DeserializeObject<JObject>(line);
                jsonObjects.Add(jsonObject);
            }
            return jsonObjects;
        }
    }
}