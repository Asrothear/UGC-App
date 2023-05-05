using System;
using System.Diagnostics;
using System.Globalization;
using System.IO.Pipes;
using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Squirrel;

namespace UGC_App
{
    internal static class Program
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        internal static Mutex _mutex = null;
        private const string MutexName = "UGC App";
        
        [STAThread]
        static void Main(string[] args)
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
            bool createdNew;
            _mutex = new Mutex(true, MutexName, out createdNew);

            if (!createdNew)
            {
                IntPtr hWnd = IntPtr.Zero;
                Process currentProcess = Process.GetCurrentProcess();
                foreach (Process process in Process.GetProcessesByName(currentProcess.ProcessName))
                {
                    if (process.Id != currentProcess.Id)
                    {
                        hWnd = process.MainWindowHandle;
                        break;
                    }
                }

                if (hWnd != IntPtr.Zero)
                {
                    SetForegroundWindow(hWnd);
                }
                NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", "UGC App", PipeDirection.Out);
                Task.Run(pipeClient.Connect);
                Application.Exit();
                Application.ExitThread();
                Environment.Exit(0);
            }
        }

        private static void OnAppInstall(SemanticVersion version, IAppTools tools)
        {
            tools.CreateShortcutForThisExe(ShortcutLocation.StartMenu | ShortcutLocation.Desktop);
        }

        private static void OnAppUninstall(SemanticVersion version, IAppTools tools)
        {
            tools.RemoveShortcutForThisExe(ShortcutLocation.StartMenu | ShortcutLocation.Desktop);
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
            if(!Config.Instance.Auto_Update) return;
            using var mgr = new UpdateManager(Config.Instance.Update_Url,"UGC-App");
            var newVersion = mgr.UpdateApp().Result;
            if (newVersion != null)
            {
                MessageBox.Show($"Die neue Version {newVersion.Version} wurde installiert!");
                var arg = string.Join(",",args);
                UpdateManager.RestartApp(null, arg);
            }
        }

        internal static void SetStartup(bool enable)
        {
            var appPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "UGC-App", "UGC App.exe");
            string appName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion", true);
            Microsoft.Win32.RegistryKey runKey = key.OpenSubKey("Run", true);
            if (runKey == null)
            {
                runKey = key.CreateSubKey("Run");
            }
            if (enable)
            {
                try
                {
                    runKey.SetValue(appName, $"\"{appPath}\" --autostart");
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
                    runKey.DeleteValue(appName, false);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
            runKey.Close();
            key.Close();
        }
        internal static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            LogException(e.Exception);
        }
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            LogException((Exception)e.ExceptionObject);
        }
        private static void LogException(Exception exception)
        {
            string logFileName = "error_log.json";
    
            var logDirectory = Config.Instance.PathLogs;
            Directory.CreateDirectory(logDirectory);
            string logFilePath = Path.Combine(logDirectory, logFileName);

            string timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffffffK", CultureInfo.InvariantCulture);

            var errorLog = new
            {
                Timestamp = timestamp,
                Message = exception.Message,
                StackTrace = exception.StackTrace,
                Source = exception.Source,
                InnerException = exception.InnerException?.ToString()
            };

            string jsonContent = File.Exists(logFilePath) ? File.ReadAllText(logFilePath) : "[]";
            var errorLogs = JsonConvert.DeserializeObject<List<dynamic>>(jsonContent);

            errorLogs.Add(errorLog);
            jsonContent = JsonConvert.SerializeObject(errorLogs, Formatting.Indented);
            File.WriteAllText(logFilePath, jsonContent);
        }
        internal static void Log(string msg)
        {
            string logFileName = "log.json";

            var logDirectory = Config.Instance.PathLogs;
            Directory.CreateDirectory(logDirectory);
            string logFilePath = Path.Combine(logDirectory, logFileName);
            if(!Config.Instance.Debug) return;
            string timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffffffK", CultureInfo.InvariantCulture);

            var errorLog = new
            {
                Timestamp = timestamp,
                Message = msg
            };
            if(!Config.Instance.Debug) return;
            string jsonContent = File.Exists(logFilePath) ? File.ReadAllText(logFilePath) : "[]";
            var errorLogs = JsonConvert.DeserializeObject<List<dynamic>>(jsonContent);

            errorLogs.Add(errorLog);
            jsonContent = JsonConvert.SerializeObject(errorLogs, Formatting.Indented);
            File.WriteAllText(logFilePath, jsonContent);
        }
    }
}