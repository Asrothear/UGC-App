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
            };
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
                            Eddn.Send(new Journal(jsonObject), _parent);
                            _parent?.GetSystemOrder(Convert.ToUInt64(jsonObject["SystemAddress"]));
                            break;
                        case "Docked":
                            CheckMeta(jsonObject);
                            Eddn.Send(new Journal(jsonObject), _parent);
                            break;
                        case "Undocked":
                            CheckMeta(jsonObject);
                            break;
                        case "CarrierJump":
                        case "FSDJump":
                            CheckMeta(jsonObject);
                            Eddn.Send(new Journal(jsonObject), _parent);
                            _parent?.GetSystemOrder(Convert.ToUInt64(jsonObject["SystemAddress"]));
                            break;
                        case "ApproachSettlement":
                            CheckMeta(jsonObject);
                            Eddn.Send(new ApproachSettlement(jsonObject), _parent);
                            break;
                        case "ApproachBody":
                            CheckMeta(jsonObject);
                            //ToDo: Collect safe Location Data for EDDN Tranmission
                            break;
                        case "CodexEntry":
                            CheckMeta(jsonObject);
                            //EDDN.Send(new CodexEntry(jsonObject), parent);
                            break;
                        case "Market":
                            CheckMeta(jsonObject);
                            //EDDN.Send(new Market(jsonObject), parent);
                            break;
                        case "FCMaterials":
                            CheckMeta(jsonObject);
                            //EDDN.Send(new FCMaterials(jsonObject), parent);
                            break;
                        case "FSSAllBodiesFound":
                            CheckMeta(jsonObject);
                            Eddn.Send(new FssAllBodiesFound(jsonObject), _parent);
                            break;
                        case "FSSBodySignals":
                            CheckMeta(jsonObject);
                            Eddn.Send(new FssBodySignals(jsonObject), _parent);
                            CheckMeta(jsonObject);
                            break;
                        case "FSSDiscoveryScan":
                            CheckMeta(jsonObject);
                            Eddn.Send(new FssDiscoveryScan(jsonObject), _parent);
                            _parent?.GetSystemOrder(Convert.ToUInt64(jsonObject["SystemAddress"]));
                            break;
                        case "FSSSignalDiscovered":
                            CheckMeta(jsonObject);
                            //EDDN.Send(new FSSSignalDiscovered(jsonObject), parent);
                            break;
                        case "NavBeaconScan":
                            CheckMeta(jsonObject);
                            Eddn.Send(new NavBeaconScan(jsonObject), _parent);
                            break;
                        case "NavRoute":
                            CheckMeta(jsonObject);
                            //EDDN.Send(new NavRoute(jsonObject), parent);
                            break;
                        case "Outfitting":
                            CheckMeta(jsonObject);
                            //EDDN.Send(new Outfitting(jsonObject), parent);
                            break;
                        case "ScanBaryCentre":
                            CheckMeta(jsonObject);
                            Eddn.Send(new ScanBaryCentre(jsonObject), _parent);
                            break;
                        case "Shipyard":
                            CheckMeta(jsonObject);
                            //EDDN.Send(new Shipyard(jsonObject), parent);
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