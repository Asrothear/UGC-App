using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

                // JSON-Inhalt verarbeiten
                ProcessJsonContent(jsonContent);

                _lastCheckedTime = lastWriteTime;
            }
        }

        private static void ProcessJsonContent(string jsonContent)
        {
            var jsonObjects = ParseJsonObjects(jsonContent);

            //Todo: Send Json to API
            foreach (var jsonObject in jsonObjects)
            {
                Debug.WriteLine(jsonObject.GetValue("event"));
                if (jsonObject.ContainsKey("event"))
                {
                    switch (jsonObject["event"].ToString())
                    {
                        case "LoadGame":
                            if (Config.Instance.CMDR == ToString(jsonObject["Commander"])) break;
                            Config.Instance.CMDR = ToString(jsonObject["Commander"]);
                            parent.label_CMDrText = Config.Instance.CMDR;
                            Config.Save();
                            break;
                    }
                }
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