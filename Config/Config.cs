using System.Reflection;
using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace UGC_App;


public class Config
{
    [AttributeUsage(AttributeTargets.Property)]
    private class DisallowDeviationAttribute : Attribute {}
    [DisallowDeviation] public string Version { get; private set; } = "0.5.5";
    [DisallowDeviation] public string VersionMeta { get; private set; } = "-alpha";
    public static Config Instance { get; }
    public Point MainLocation { get; set; } = new (50, 50);
    public Point OverlayLocation { get; set; } = new (50, 50);
    public string Cmdr { get; set; } = "";    
    public string Suit { get; set; } = "";
    public string Token { get; set; } = "";
    public string SendUrl { get; set; } = "https://api.ugc-tools.de/api/v1/QLS";
    public string StateUrl { get; set; } = "https://api.ugc-tools.de/api/v1/State";
    // ReSharper disable once UnusedMember.Global
    public string CmdUrl { get; set; } = "https://api.ugc-tools.de/api/v1/PluginControll";
    public string SystemDataUrl { get; set; } = "https://api.ugc-tools.de/api/v1/SystemHistory";
    public string SystemOrdersUrl { get; set; } = "https://api.ugc-tools.de/api/v1/OrdersList";
    public string TickUrl { get; private set; } = "https://api.ugc-tools.de/api/v1/Tick";
    public string UpdateUrl { get; private set; } = "https://update.ugc-tools.de";
    public bool BgsOnly { get; set; }
    public bool ShowAll { get; set; } = true;
    public bool SendName { get; set; } = true;
    public bool Debug { get; set; }
    public bool AutoUpdate { get; set; } = true;
    public bool SlowState { get; set; }
    public bool AutoStart { get; set; }
    public decimal ListCount { get; set; } = 1;
    public bool CloseMini { get; set; } = true;
    public int DesignSel { get; set; }
    public Color ColorMainBackground { get; set; } = Color.FromName("Control");
    public Color ColorMainInfo { get; set; } = Color.Black;
    public Color ColorMainTick { get; set; } = Color.Black;
    public Color ColorMainSysteme { get; set; } = Color.Black;
    public Color ColorOverlayBackground { get; set; } = Color.DarkGoldenrod;
    public Color ColorOverlayTickLight { get; set; } = Color.Black;
    public Color ColorOverlaySystemeLight { get; set; } = Color.Black;
    public Color ColorOverlayTickDark { get; set; } = Color.White;
    public Color ColorOverlaySystemeDark { get; set; } = Color.White;
    internal Color ColorDefaultChroma { get; } = Color.DarkGoldenrod;
    internal Color ColorDefaultLabelDark { get; } = Color.White;
    internal Color ColorDefaultLabelLight { get; } = Color.Black;
    internal Color ColorDefaultBackgroundLight { get; } = Color.FromName("Control");
    internal Color ColorDefaultBackgroundDark { get; } = Color.FromArgb(60, 60, 60);
    internal decimal CheckBackgroundIntervall { get; set; } = 500;
    public string? LastSystem { get; set; } = "";
    public string? LastDocked { get; set; } = "";
    public bool ColorOverlayOverride { get; set; }
    public bool AlwaysOnTop { get; set; }
    public string? GameVersion { get; set; } = "";
    public string? GameBuild { get; set; } = "";
    public bool Odyssey { get; set; }
    public bool Horizons { get; set; }
    public bool Eddn { get; set; }
    public string PathLogs { get; set; } =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "UGC-App", "logs");

    public string PathConfig { get; set; } =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "UGC-App", "config");
    public string PathJournal { get; set; } =
        Environment.ExpandEnvironmentVariables(@"%USERPROFILE%\Saved Games\Frontier Developments\Elite Dangerous\");

    private static readonly string ConfigFilePath;

    static Config()
    {
        var configFolder = 
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "UGC-App", "config");
        if (!Directory.Exists(configFolder))
        {
            Directory.CreateDirectory(configFolder);
        }

        ConfigFilePath = Path.Combine(configFolder, "config.json");
        if (!File.Exists(ConfigFilePath))
        {
            Instance = new Config();
            Save();
        }
        else
        {
            var json = File.ReadAllText(ConfigFilePath);
            Instance = JsonConvert.DeserializeObject<Config>(json) ?? new Config();
            ResetDisallowedDeviations();
        }
    }

    private static void ResetDisallowedDeviations()
    {
        var defaultInstance = new Config();

        var properties = typeof(Config).GetProperties();
        var hasDeviations = false;

        foreach (var property in properties)
        {
            var attribute = property.GetCustomAttribute<DisallowDeviationAttribute>();
            if (attribute == null) continue;
            var currentValue = property.GetValue(Instance);
            var defaultValue = property.GetValue(defaultInstance);
            if (Equals(currentValue, defaultValue)) continue;
            property.SetValue(Instance, defaultValue);
            hasDeviations = true;
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