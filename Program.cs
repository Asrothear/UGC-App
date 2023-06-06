using System.Diagnostics;
using System.Globalization;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using Squirrel;
using UGC_App.ErrorReporter;

namespace UGC_App;

internal static class Program
{
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
#pragma warning disable SYSLIB1054
    private static extern bool SetForegroundWindow(IntPtr hWnd);
#pragma warning restore SYSLIB1054
    private static Mutex? _mutex;
    private const string MutexName = "UGC App";
        
    [STAThread]
    private static void Main(string[] args)
    {
        CheckSingleInstance();
        SquirrelAwareApp.HandleEvents(
            onInitialInstall: OnAppInstall,
            onAppUninstall: OnAppUninstall,
            onEveryRun: OnAppRun);
        UpdateMyApp(args);
        Application.ThreadException += Application_ThreadException;
        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        ApplicationConfiguration.Initialize();
        var mainForm = new Mainframe();
        if (args.Contains("--autostart"))
        {
            mainForm.WindowState = FormWindowState.Minimized;
            mainForm.ShowInTaskbar = false;
        }
        Application.Run(mainForm);
    }

    private static void CheckSingleInstance()
    {
        _mutex = new Mutex(true, MutexName, out var createdNew);
        _ = _mutex.GetType();
        if (createdNew) return;
        var hWnd = IntPtr.Zero;
        var currentProcess = Process.GetCurrentProcess();
        foreach (var process in Process.GetProcessesByName(currentProcess.ProcessName))
        {
            if (process.Id == currentProcess.Id) continue;
            hWnd = process.MainWindowHandle;
            break;
        }

        if (hWnd != IntPtr.Zero)
        {
            SetForegroundWindow(hWnd);
        }
        var pipeClient = new NamedPipeClientStream(".", "UGC App", PipeDirection.Out);
        Task.Run(pipeClient.Connect);
        Application.Exit();
        Application.ExitThread();
        Environment.Exit(0);
    }

    private static void OnAppInstall(SemanticVersion version, IAppTools tools)
    {
        tools.CreateShortcutForThisExe();
    }

    private static void OnAppUninstall(SemanticVersion version, IAppTools tools)
    {
        tools.RemoveShortcutForThisExe();
        SetStartup(false);
    }

    private static void OnAppRun(SemanticVersion version, IAppTools tools, bool firstRun)
    {
        tools.SetProcessAppUserModelId();
        // show a welcome message when the app is first installed
        if (firstRun) MessageBox.Show("UGC APP erfolgreich Installiert");
    }
    private static void UpdateMyApp(string[] args)
    {
        if(!Config.Instance.AutoUpdate) return;
        using var mgr = new UpdateManager(Config.Instance.UpdateUrl,"UGC-App");
        var newVersion = mgr.UpdateApp().Result;
        if (newVersion == null) return;
        //MessageBox.Show($"Die neue Version {newVersion.Version} wurde installiert!");
        var arg = string.Join(",",args);
        UpdateManager.RestartApp(null, arg);
    }

    internal static void SetStartup(bool enable)
    {
        var appPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "UGC-App", "UGC App.exe");
        var appName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
        var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion", true);
        var runKey = key?.OpenSubKey("Run", true) ?? key?.CreateSubKey("Run");
        if (enable)
        {
            try
            {
                runKey?.SetValue(appName, $"\"{appPath}\" --autostart");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
        else
        {
            try
            {
                if (appName != null) runKey?.DeleteValue(appName, false);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        runKey?.Close();
        key?.Close();
    }

    private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
    {
        LogException(e.Exception);
    }
    private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        LogException((Exception)e.ExceptionObject);
    }
    private static void LogException(Exception exception)
    {
        if(MailClient.IsDelError)return;
        const string logFileName = "error_log.json";
    
        var logDirectory = Config.Instance.PathLogs;
        Directory.CreateDirectory(logDirectory);
        var logFilePath = Path.Combine(logDirectory, logFileName);

        var timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffffffK", CultureInfo.InvariantCulture);

        var errorLog = new
        {
            Timestamp = timestamp,
            exception.Message,
            exception.StackTrace,
            exception.Source,
            InnerException = exception.InnerException?.ToString()
        };

        string jsonContent;
        if (!File.Exists(logFilePath))
        {
            var file = File.Create(logFilePath);
            file.Close();
        }
        using (var reader = new StreamReader(logFilePath))
        {
            jsonContent = reader.ReadToEnd();
            reader.Close();
            reader.Dispose();
        }
        if(string.IsNullOrWhiteSpace(jsonContent))jsonContent="[]";
        var errorLogs = JsonConvert.DeserializeObject<List<dynamic>>(jsonContent);
        errorLogs?.Add(errorLog);
        jsonContent = JsonConvert.SerializeObject(errorLogs, Formatting.Indented);
        using (var writer = new StreamWriter(logFilePath))
        {
            writer.Write(jsonContent);
            writer.Close();
            writer.Dispose();
        }
    }
    internal static void Log(string msg)
    {
        if (MailClient.IsDelLog)
            return;

        const string logFileName = "log.json";
        var logDirectory = Config.Instance.PathLogs;
        Directory.CreateDirectory(logDirectory);
        var logFilePath = Path.Combine(logDirectory, logFileName);

        if (!Config.Instance.Debug)
            return;

        var timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffffffK", CultureInfo.InvariantCulture);

        var errorLog = new
        {
            Timestamp = timestamp,
            Message = msg
        };

        if (!Config.Instance.Debug)
            return;
        string jsonContent;
        if (!File.Exists(logFilePath))
        {
            var file = File.Create(logFilePath);
            file.Close();
        }
        using (var reader = new StreamReader(logFilePath))
        {
            jsonContent = reader.ReadToEnd();
            reader.Close();
            reader.Dispose();
        }
        if(string.IsNullOrWhiteSpace(jsonContent))jsonContent="[]";
        var errorLogs = JsonConvert.DeserializeObject<List<dynamic>>(jsonContent);
        errorLogs?.Add(errorLog);
        jsonContent = JsonConvert.SerializeObject(errorLogs, Formatting.Indented);
        using (var writer = new StreamWriter(logFilePath))
        {
            writer.Write(jsonContent);
            writer.Close();
            writer.Dispose();
        }
    }
}