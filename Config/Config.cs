using System.Reflection;
using Newtonsoft.Json;

namespace UGC_App;


public class Config
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DisallowDeviationAttribute : Attribute
    {
    }
    public static Config Instance { get; private set; }

    
    public Size FormSize { get; set; } = new Size(50,50);
    public Point FormLocation { get; set; } = new Point(50,50);
    public string CMDR { get; set; } = "";
    public string Token { get; set; } = "";
    public string Send_Url { get; set; } = "https://api.ugc-tools.de/api/v1/QLS";
    public string State_Url { get; set; } = "https://api.ugc-tools.de/api/v1/State";
    public string CMD_Urls { get; set; } = "https://api.ugc-tools.de/api/v1/PluginControll";
    public string Tick_Url { get; set; } = "https://api.ugc-tools.de/api/v1/Tick";
    public string Update_Url { get; set; } = "https://update.ugc-tools.de";
    public bool BGS_Only { get; set; } = false;
    public bool Show_All { get; set; } = true;
    public bool Send_Name { get; set; } = true;
    public bool Debug { get; set; } = false;
    public bool Auto_Update { get; set; } = true;
    public bool SlowState { get; set; } = false;
    public bool AutoStart { get; set; } = false;
    public decimal ListCount { get; set; } = 1;
    public bool CloseMini { get; set; } = true;
    [DisallowDeviation]
    public string Version { get; set; } = "0.2.2";
    [DisallowDeviation]
    public string Version_Meta { get; set; } = "-alpha-030523";
    public int Design_Sel { get; set; } = 0;
    public Color Color_Main_Background { get; set; } = Color.FromName("Control");
    public Color Color_Main_Info { get; set; } = Color.Black;
    public Color Color_Main_Tick { get; set; } = Color.Black;
    public Color Color_Main_Systeme { get; set; } = Color.Black;
    public Color Color_Overlay_Background { get; set; } = Color.DarkGoldenrod;
    public Color Color_Overlay_Tick_Light { get; set; } = Color.Black;
    public Color Color_Overlay_Systeme_Light { get; set; } = Color.Black;
    public Color Color_Overlay_Tick_Dark { get; set; } = Color.White;
    public Color Color_Overlay_Systeme_Dark { get; set; } = Color.White;
    internal Color Color_Default_Chroma { get;} = Color.DarkGoldenrod;
    internal Color Color_Default_Label_Dark { get;} = Color.White;
    internal Color Color_Default_Label_Light { get;} = Color.Black;
    internal Color Color_Default_Background_Light { get;} = Color.FromName("Control");
    internal Color Color_Default_Background_Dark { get;} = Color.FromArgb(60,60,60);
    public string LastSystem { get; set; }
    public string LastDocked { get; set; }
    public bool Color_Overlay_Override { get; set; } = false;
    public bool AlwaysOnTop { get; set; } = false;
    public string GameVersion { get; set; }
    public string GameBuild { get; set; }
    public bool Odyssey { get; set; }
    public bool Horizons { get; set; }
    public bool EDDN { get; set; }

    private static string ConfigFilePath;
    static Config()
    {
        var configFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "UGC-App", "config");
        if (!Directory.Exists(configFolder))
        {
            Directory.CreateDirectory(configFolder);
        }
        ConfigFilePath = Path.Combine(configFolder, "config.json");
        if (!File.Exists(ConfigFilePath))
        {
            Instance = new ();
            Save();
        }
        else
        {
            var json = File.ReadAllText(ConfigFilePath);
            Instance = JsonConvert.DeserializeObject<Config>(json);
            ResetDisallowedDeviations();
        }
    }

    private static void ResetDisallowedDeviations()
    {
        var defaultInstance = new Config();

        var properties = typeof(Config).GetProperties();
        bool hasDeviations = false;

        foreach (var property in properties)
        {
            var attribute = property.GetCustomAttribute<DisallowDeviationAttribute>();
            if (attribute != null)
            {
                var currentValue = property.GetValue(Instance);
                var defaultValue = property.GetValue(defaultInstance);

                if (!Equals(currentValue, defaultValue))
                {
                    property.SetValue(Instance, defaultValue);
                    hasDeviations = true;
                }
            }
        }

        if (hasDeviations)
        {
            Save();
        }
    }

    public static void Save()
    {
        var json = JsonConvert.SerializeObject(Instance, Formatting.Indented);
        File.WriteAllText(ConfigFilePath, json);
    }
}