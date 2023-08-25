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

        internal static void Stop()
        {
            Running = false;
        }

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
                try
                {
                    _parent?.SetLightActive(_parent.yellowLight, true);
                    _parent?.SetLightActive(_parent.greenLight, false);

                    CheckForLatestFile(Config.Instance.PathJournal, prefix);
                    CheckForFileChanges();

                    _parent?.SetLightActive(_parent.yellowLight, false);
                    _parent?.SetLightActive(_parent.greenLight, true);
                    Thread.Sleep(pollingInterval);
                }
                catch
                {
                    Running = false;
                }
            }
        }

        internal static void SwitchToLatestFile(string directoryPath, string prefix)
        {
            var latestFile = GetLatestFile(directoryPath, prefix);

            if (latestFile == null) return;
            _currentFile = latestFile;
            _previousPosition = new FileInfo(_currentFile).Length;
            _lastCheckedTime = File.GetLastWriteTimeUtc(_currentFile);
        }

        private static string? GetLatestFile(string directoryPath, string prefix)
        {
            return Directory.GetFiles(directoryPath, $"{prefix}.*").MaxBy(File.GetCreationTimeUtc);
        }

        private static void CheckForLatestFile(string directoryPath, string prefix)
        {
            var latestFile = GetLatestFile(directoryPath, prefix);

            if (latestFile == null || latestFile == _currentFile) return;
            Program.Log($"Wechsel zur neuesten Datei: {latestFile}");
            CheckForFileChanges(true);
            _currentFile = latestFile;
            _previousPosition = 0;
            _lastCheckedTime = File.GetLastWriteTimeUtc(_currentFile);
        }

        private static void CheckForFileChanges(bool swit = false)
        {
            if(swit)Program.Log($"Final Check before switching");
            if (_currentFile == null) 
            {
                if(swit)Program.Log($"nor current, now switching");
                return;
            }
            var lastWriteTime = File.GetLastWriteTimeUtc(_currentFile);

            if (lastWriteTime <= _lastCheckedTime)
            {
                if(swit)Program.Log($"_lastCheckedTime not enough, now switching");
                return;
            }
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

            Task.Run(() => ProcessJsonContent(jsonContent));

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
                            Config.Instance.Cmdr = ToString(jsonObject["Commander"]) ?? "";
                            Config.Instance.GameVersion = ToString(jsonObject["gameversion"]);
                            Config.Instance.GameBuild = ToString(jsonObject["build"]);
                            Config.Instance.Horizons = Convert.ToBoolean(jsonObject["Horizons"]);
                            Config.Instance.Odyssey = Convert.ToBoolean(jsonObject["Odyssey"]);
                            _parent?.SetCmDrText(Config.Instance.Cmdr);
                            break;
                        case "Location":
                            CheckMeta(jsonObject);
                            if (jsonObject.TryGetValue("Body", out var lbody))EDDN.JournalBodyName = lbody.ToString();
                            if (jsonObject.TryGetValue("BodyID", out var lbodyid)) EDDN.JournalBodyId = Convert.ToUInt64(lbodyid);
                            EDDN.Send(new Journal(jsonObject), _parent);
                            _parent?.GetSystemOrder(Convert.ToUInt64(jsonObject["SystemAddress"]));
                            break;
                        case "Docked":
                            CheckMeta(jsonObject);
                            EDDN.Send(new Journal(jsonObject), _parent);
                            break;
                        case "Undocked":
                            CheckMeta(jsonObject);
                            break;
                        case "CarrierJump":
                        case "FSDJump":
                            CheckMeta(jsonObject);
                            EDDN.JournalBodyName = "";
                            EDDN.JournalBodyId = 0;
                            EDDN.Send(new Journal(jsonObject), _parent);
                            _parent?.GetSystemOrder(Convert.ToUInt64(jsonObject["SystemAddress"]));
                            if(Config.Instance.Debug)Program.Log($"Jump - {Config.Instance.LastSystem}");
                            break;
                        case "ApproachSettlement":
                            CheckMeta(jsonObject);
                            EDDN.Send(new ApproachSettlement(jsonObject), _parent);
                            break;
                        case "ApproachBody":
                            CheckMeta(jsonObject);
                            if (jsonObject.TryGetValue("Body", out var body))EDDN.JournalBodyName = body.ToString();
                            if (jsonObject.TryGetValue("BodyID", out var bodyid)) EDDN.JournalBodyId = Convert.ToUInt64(bodyid);
                            break;
                        case "CodexEntry":
                            StatusHandler.GetBodyStatus();
                            CheckMeta(jsonObject);
                            EDDN.Send(new CodexEntry(jsonObject), _parent);
                            break;
                        case "Market":
                            CheckMeta(jsonObject);
                            var mark = JsonDataHandler.GetData("Market.json");
                            if(mark==null)break;
                            EDDN.Send(new Market(mark), _parent);
                            break;
                        case "FCMaterials":
                            CheckMeta(jsonObject);
                            var mats = JsonDataHandler.GetData("FCMaterials.json");
                            if(mats==null)break;
                            EDDN.Send(new FcMaterials(mats), _parent);
                            break;
                        case "FSSAllBodiesFound":
                            CheckMeta(jsonObject);
                            EDDN.Send(new FssAllBodiesFound(jsonObject), _parent);
                            break;
                        case "FSSBodySignals":
                            CheckMeta(jsonObject);
                            EDDN.Send(new FssBodySignals(jsonObject), _parent);
                            CheckMeta(jsonObject);
                            break;
                        case "FSSDiscoveryScan":
                            CheckMeta(jsonObject);
                            EDDN.Send(new FssDiscoveryScan(jsonObject), _parent);
                            _parent?.GetSystemOrder(Convert.ToUInt64(jsonObject["SystemAddress"]));
                            break;
                        case "FSSSignalDiscovered":
                            //ToDo: https://github.com/EDCD/EDDN/blob/master/schemas/fsssignaldiscovered-README.md
                            CheckMeta(jsonObject);
                            //EDDN.Send(new FSSSignalDiscovered(jsonObject), parent);
                            break;
                        case "LeaveBody":
                            EDDN.JournalBodyName = "";
                            EDDN.JournalBodyId = 0;
                            break;
                        case "NavBeaconScan":
                            CheckMeta(jsonObject);
                            EDDN.Send(new NavBeaconScan(jsonObject), _parent);
                            break;
                        case "NavRoute":
                            CheckMeta(jsonObject);
                            var route = JsonDataHandler.GetData("NavRoute.json");
                            if(route==null)break;
                            EDDN.Send(new NavRoute(route), _parent);
                            break;
                        case "Outfitting":
                            CheckMeta(jsonObject);
                            var outf = JsonDataHandler.GetData("Outfitting.json");
                            if(outf==null)break;
                            EDDN.Send(new Outfitting(outf), _parent);
                            break;
                        case "ScanBaryCentre":
                            CheckMeta(jsonObject);
                            EDDN.Send(new ScanBaryCentre(jsonObject), _parent);
                            break;
                        case "Shipyard":
                            CheckMeta(jsonObject);
                            var ship = JsonDataHandler.GetData("Shipyard.json");
                            if(ship==null)break;
                            EDDN.Send(new Shipyard(ship), _parent);
                            break;
                        case "SuitLoadout":
                            CheckMeta(jsonObject);
                            var suit = CutString(ToString(jsonObject["SuitName"]) ?? "", '_');
                            Config.Instance.Suit = suit;
                            _parent?.SetSuitText(Properties.language.ResourceManager.GetString(Config.Instance.Suit));
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

        private static void CheckMeta(JObject jsonObject)
        {
            if (jsonObject.TryGetValue("StarSystem", out var starSystem))
            {
                Config.Instance.LastSystem = ToString(starSystem);
                _parent?.SetSystemText(Config.Instance.LastSystem);
            }
            if (jsonObject.TryGetValue("Docked", out var docked))
            {
                Config.Instance.LastDocked = Convert.ToBoolean(docked) ? ToString(jsonObject["StationName"]) : "-";
                _parent?.SetDockedText(Config.Instance.LastDocked);
            }
            else
            {
                Config.Instance.LastDocked = "-";
                _parent?.SetDockedText(Config.Instance.LastDocked);
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

        private static string CutString(string inputString, char character)
        {
            if (string.IsNullOrWhiteSpace(inputString)) return "";
            var index = inputString.IndexOf(character);
            var result = index >= 0 ? inputString[..(index + 1)] : inputString;
            return result;
        }
    }
}