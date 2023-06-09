using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UGC_App.WebClient;
using UGC_App.WebClient.Schema;

namespace UGC_App.EDLog
{
    internal static class JournalHandler
    {
        private static Mainframe? _parent;
        private static string? _currentFile;
        private static long _previousPosition;
        private static DateTime _lastCheckedTime;
        internal static bool Running;

        internal static void Start(Mainframe? parrent)
        {
            if (Running) return;
            Running = true;
            _parent = parrent;
            var prefix = "Journal";
            var pollingInterval = TimeSpan.FromSeconds(5);
            SwitchToLatestFile(Config.Instance.PathJournal, prefix);
            Program.Log($"Journal Watchdog starting... {_currentFile}");
            while (Running)
            {
                
                _parent?.SetLightActive(_parent.yellowLight, true);
                _parent?.SetLightActive(_parent.greenLight, false);
                
                CheckForLatestFile(Config.Instance.PathJournal, prefix);
                CheckForFileChanges();
                
                _parent?.SetLightActive(_parent.yellowLight, false);
                _parent?.SetLightActive(_parent.greenLight, true);
                Thread.Sleep(pollingInterval);
            }
        }

        internal static void SwitchToLatestFile(string directoryPath, string prefix)
        {
            var latestFile = GetLatestFile(directoryPath, prefix);

            if (latestFile != null)
            {
                _currentFile = latestFile;
                _previousPosition = new FileInfo(_currentFile).Length;
                _lastCheckedTime = File.GetLastWriteTimeUtc(_currentFile);
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
                Program.Log($"Wechsel zur neuesten Datei: {latestFile}");
                CheckForFileChanges(true);
                _currentFile = latestFile;
                _previousPosition = 0;
                _lastCheckedTime = File.GetLastWriteTimeUtc(_currentFile);
            }
        }

        private static void CheckForFileChanges(bool swit = false)
        {
            if(swit)Program.Log($"Final Check before switching");
            if (_currentFile == null) 
            {
                if(swit)Program.Log($"nor current, now switching");
                return;
            };;
            var lastWriteTime = File.GetLastWriteTimeUtc(_currentFile);

            if (lastWriteTime <= _lastCheckedTime)
            {
                if(swit)Program.Log($"_lastCheckedTime not enough, now switching");
                return;
            };
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
            if(swit)Program.Log($"Final end, now switching");
        }

        private static void ProcessJsonContent(string jsonContent)
        {
            var jsonObjects = ParseJsonObjects(jsonContent);
            foreach (var jsonObject in jsonObjects)
            {
                if (jsonObject != null && jsonObject.TryGetValue("event", out var value))
                {
                    switch (value.ToString())
                    {
                        case "Fileheader":
                            Config.Instance.GameVersion = ToString(jsonObject["gameversion"]);
                            Config.Instance.GameBuild = ToString(jsonObject["build"]);
                            break;
                        case "LoadGame":
                            Config.Instance.Cmdr = ToString(jsonObject["Commander"]);
                            Config.Instance.GameVersion = ToString(jsonObject["gameversion"]);
                            Config.Instance.GameBuild = ToString(jsonObject["build"]);
                            Config.Instance.Horizons = Convert.ToBoolean(jsonObject["Horizons"]);
                            Config.Instance.Odyssey = Convert.ToBoolean(jsonObject["Odyssey"]);
                            _parent?.SetCmDrText(Config.Instance.Cmdr);
                            break;
                        case "Location":
                            Config.Instance.LastSystem = ToString(jsonObject["StarSystem"]);
                            _parent?.SetSystemText(Config.Instance.LastSystem);
                            Config.Instance.LastDocked = Convert.ToBoolean(jsonObject["Docked"]) ? ToString(jsonObject["StationName"]) : "-";
                            _parent?.SetDockedText(Config.Instance.LastDocked);
                            Eddn.Send(new Journal(jsonObject), _parent);
                            _parent?.GetSystemOrder(Convert.ToUInt64(jsonObject["SystemAddress"]));
                            break;
                        case "Docked":
                            Config.Instance.LastDocked = ToString(jsonObject["StationName"]);
                            _parent?.SetDockedText(Config.Instance.LastDocked);
                            Eddn.Send(new Journal(jsonObject), _parent);
                            break;
                        case "Undocked":
                            Config.Instance.LastDocked = "-";
                            _parent?.SetDockedText(Config.Instance.LastDocked);
                            break;
                        case "CarrierJump":
                        case "FSDJump":
                            Config.Instance.LastSystem = ToString(jsonObject["StarSystem"]);
                            Config.Instance.LastDocked = "-";
                            Eddn.Send(new Journal(jsonObject), _parent);
                            _parent?.GetSystemOrder(Convert.ToUInt64(jsonObject["SystemAddress"]));
                            break;
                        case "ApproachSettlement":
                            Eddn.Send(new ApproachSettlement(jsonObject), _parent);
                            break;
                        case "ApproachBody":
                            //ToDo: Collect safe Location Data for EDDN Tranmission
                            break;
                        case "CodexEntry":
                            //EDDN.Send(new CodexEntry(jsonObject), parent);
                            break;
                        case "Market":
                            //EDDN.Send(new Market(jsonObject), parent);
                            break;
                        case "FCMaterials":
                            //EDDN.Send(new FCMaterials(jsonObject), parent);
                            break;
                        case "FSSAllBodiesFound":
                            Eddn.Send(new FssAllBodiesFound(jsonObject), _parent);
                            break;
                        case "FSSBodySignals":
                            Eddn.Send(new FssBodySignals(jsonObject), _parent);
                            break;
                        case "FSSDiscoveryScan":
                            Eddn.Send(new FssDiscoveryScan(jsonObject), _parent);
                            break;
                        case "FSSSignalDiscovered":
                            //EDDN.Send(new FSSSignalDiscovered(jsonObject), parent);
                            break;
                        case "NavBeaconScan":
                            Eddn.Send(new NavBeaconScan(jsonObject), _parent);
                            break;
                        case "NavRoute":
                            //EDDN.Send(new NavRoute(jsonObject), parent);
                            break;
                        case "Outfitting":
                            //EDDN.Send(new Outfitting(jsonObject), parent);
                            break;
                        case "ScanBaryCentre":
                            Eddn.Send(new ScanBaryCentre(jsonObject), _parent);
                            break;
                        case "Shipyard":
                            //EDDN.Send(new Shipyard(jsonObject), parent);
                            break;
                    }

                    Config.Save();
                    _parent?.SetSystemText(Config.Instance.LastSystem);
                    _parent?.SetDockedText(Config.Instance.LastDocked);
                }

                if (jsonObject == null) continue;
                jsonObject["user"] = Config.Instance.Cmdr;
                ApiSender.SendApi(jsonObject.ToString(), _parent);
            }
        }

        private static string? ToString(JToken? inp)
        {
            return inp?.ToString();
        }
        private static List<JObject?> ParseJsonObjects(string input)
        {
            var lines = input.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            return lines.Select(JsonConvert.DeserializeObject<JObject>).Where(jsonObject => jsonObject != null).ToList();
        }
    }
}