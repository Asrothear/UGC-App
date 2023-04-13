using System.Diagnostics;
using System.IO.Pipes;
using System.Net.Mime;
using System.Runtime.InteropServices;
using Squirrel;

namespace UGC_App
{
    internal static class Program
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        private static Mutex _mutex = null;
        private const string MutexName = "UGC App";
        
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static async Task Main(string[] args)
        {
            CheckSingleInstance();
            SquirrelAwareApp.HandleEvents(
            onInitialInstall: OnAppInstall,
            onAppUninstall: OnAppUninstall,
            onEveryRun: OnAppRun);
            await UpdateMyApp(args);
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
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
                // Wenn die Mutex bereits vorhanden ist, bringen Sie die laufende Anwendung in den Vordergrund
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
        private static async Task UpdateMyApp(string[] args)
        {
            if(!Properties.Settings.Default.Auto_Update) return;
            using var mgr = new UpdateManager(Properties.Settings.Default.Update_Url,"UGC-App");
            var newVersion = await mgr.UpdateApp();

            // optionally restart the app automatically, or ask the user if/when they want to restart
            if (newVersion != null)
            {
                var arg = string.Join(",",args);
                UpdateManager.RestartApp(null, arg);
            }
        }

        internal static void SetStartup(bool enable)
        {
            string appName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            string appPath = Application.ExecutablePath + " --autostart";
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
                    runKey.SetValue(appName, appPath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
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
                    Console.WriteLine(ex);
                }
            }
            runKey.Close();
            key.Close();
        }
    }
}