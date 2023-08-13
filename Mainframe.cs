using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.IO.Pipes;
using System.Media;
using System.Text.RegularExpressions;
using Squirrel;
using UGC_App.EDLog;
using UGC_App.Order;
using UGC_App.Order.DashViews;
using UGC_App.WebClient;
using NonInvasiveKeyboardHookLibrary;
using UGC_App.LocalCache;
using Timer = System.Timers.Timer;

namespace UGC_App;

public partial class Mainframe : Form
{
    private Konfiguration? _conf;
    private About? _about;
    private Dashboard? _dashboard;
    private const int LabelSpacing = 35;
    private bool _closing;
    private bool _ed;
    private int _sends;
    private readonly KeyboardHookManager _keyboardHookManager = new();
    private static readonly Timer CacheTimer = new();

    [GeneratedRegex("(.*?)(?:\\s*:\\s*\\d+(?:\\.\\d+)?\\s*ly)?$")]
    private static partial Regex SystemListRegex();

    public Mainframe()
    {
        InitializeComponent();
        CacheTimer.Interval += 30 * 60 * 1000;
        CacheTimer.Elapsed += (_, _) =>
        {
            Program.Log("CacheTimer");
            CacheHandler.InitAll(true);
        };
        SizeChanged += (_, _) => FixLayout();
        Load += (_, _) =>
        {
            if (WindowState == FormWindowState.Normal)
            {
                Invoke(() => { Location = Config.Instance.MainLocation; });
                FixLayout();
            }

            Task.Run(() =>
            {
                Thread.Sleep(500);
                Program.Log("Caching");
                anweisungenToolStripMenuItem.Text = "warte auf Cache...";
                anweisungenToolStripMenuItem.Enabled = false;
                CacheHandler.InitAll();
                anweisungenToolStripMenuItem.Text = "BGS";
                anweisungenToolStripMenuItem.Enabled = true;
                CacheTimer.Start();
                StartWorker();
                Task.Run(SetUpEdSpy);
            });
        };

        contextMenuStrip.Items.AddRange(new ToolStripItem[]
        {
            CreateToolStripMenuItem("Wiederherstellen", RestoreClick),
            CreateToolStripMenuItem("Toogle Overlay", ToogleOverlayClick),
            CreateToolStripMenuItem("Einstellungen", ShowKonfig),
            new ToolStripSeparator(),
            CreateToolStripMenuItem("About", ShowAbout),
            CreateToolStripMenuItem("auf Updates Prüfen", CheckUpdates),
            CreateToolStripMenuItem("Position Reset (Debug)", ResetPos),
            new ToolStripSeparator(),
            CreateToolStripMenuItem("Beenden", ExitMenuItem_Click)
        });
        notifyIcon.ContextMenuStrip = contextMenuStrip;
        notifyIcon.MouseDoubleClick += NotifyIcon_MouseDoubleClick;
        label_SystemList.SizeChanged += SystemListChanged;
        toolStripMenuItem_Overlay.Click += ToogleOverlayClick;
        toolStripMenuItem_About.Click += ShowAbout;
        toolStripMenuItem_Settings.Click += ShowKonfig;
        toolStripMenuItem_CheckForUpdates.Click += CheckUpdates;
        dashboardOrderToolStripMenuItem.Click += ShowOrderDashboard;
        dashboardSystemsToolStripMenuItem.Click += ShowSystemDashboard;
        SetCircles();
        CenterObjectHorizontally(label_SystemList);
        toolStripStatusLabel_Version.Text = $"{Properties.language.Version} {Config.Instance.Version}";
        Height = label_SystemList.Bottom + LabelSpacing + 46;
        label_CMDr.Text = $"CMDr: {Config.Instance.Cmdr}";
        label_System.Text = $"System: {Config.Instance.LastSystem}";
        label_Docked.Text = $"Angedockt: {Config.Instance.LastDocked}";
        label_Suit.Text = $"Anzug: {Properties.language.ResourceManager.GetString(Config.Instance.Suit)}";
        SetDesign();
        TopMost = Config.Instance.AlwaysOnTop;
        Program.SetStartup(Config.Instance.AutoStart);
        _keyboardHookManager.Start();
        _keyboardHookManager.RegisterHotkey(
            new[]
            {
                NonInvasiveKeyboardHookLibrary.ModifierKeys.Control, NonInvasiveKeyboardHookLibrary.ModifierKeys.Alt
            }, (int)Keys.C, FisrtToClip);
    }

    private void ResetPos(object? sender, EventArgs e)
    {
        var res = MessageBox.Show(
            "Hiermit werden die Positionen des Hauptfensters und des Overlays der UGC-App zurückgesetzt.\nDies ist für den fall das die UGC-App nicht mehr sichtbar ist.\nFortfahren?",
            "Position Reset", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2,
            MessageBoxOptions.ServiceNotification
        );
        if (res != DialogResult.Yes) return;
        Config.Instance.MainLocation = new Point(50, 50);
        Config.Instance.OverlayLocation = new Point(50, 50);
        Config.Save();
        Location = Config.Instance.MainLocation;
        if (overlayForm is { IsDisposed: false }) overlayForm.Location = Config.Instance.OverlayLocation;
        Activate();
    }

    private void FisrtToClip()
    {
        var regex = SystemListRegex();
        var text = label_SystemList.Text.Split(",").First();
        if (string.IsNullOrWhiteSpace(text) || text == "Alles Aktuell!") return;
        SystemSounds.Asterisk.Play();
        var match = regex.Match(text);
        if (!match.Success) return;
        var result = match.Groups[1].Value;
        Invoke(() => Clipboard.SetText(result.Replace("\u00A0", " ")));
    }

    private void ShowOrderDashboard(object? sender, EventArgs e)
    {
        ShowDashboard(1);
    }

    private void ShowSystemDashboard(object? sender, EventArgs e)
    {
        ShowDashboard(2);
    }

    private void ShowDashboard(int type)
    {
        var def = Cursor.Current;
        Cursor.Current = Cursors.WaitCursor;
        if (_dashboard == null || _dashboard.IsDisposed)
        {
            _dashboard = new Dashboard();
            _dashboard.Show(this);
        }

        _dashboard.Activate();
        switch (type)
        {
            case 1:
                _dashboard.SwitchView(new OrderList(_dashboard));
                break;
            case 2:
                _dashboard.SwitchView(new SystemList());
                break;
        }

        Cursor.Current = def;
        //SetDesign();
    }

    private async void CheckUpdates(object? sender, EventArgs e)
    {
        using var mgr = new UpdateManager(Config.Instance.UpdateUrl, "UGC-App");
        var infos = await mgr.CheckForUpdate();
        if (infos.CurrentlyInstalledVersion.Version >= infos.FutureReleaseEntry.Version &&
            infos.CurrentlyInstalledVersion.SHA1 == infos.FutureReleaseEntry.SHA1)
        {
            MessageBox.Show(this, "Die aktuellste Version ist breits Installiert.", "Updater");
            return;
        }

        TopMost = false;
        var dialogResult =
            MessageBox.Show(this,
                $"Eine neue Version ist verfügbar.\n{infos.CurrentlyInstalledVersion.Version}->{infos.FutureReleaseEntry.Version}\nUpdate Installieren?",
                "Updater", MessageBoxButtons.YesNo);
        if (dialogResult != DialogResult.Yes) return;
        JournalHandler.Stop();
        SetStatus("Update App...");
        var newVersion = await mgr.UpdateApp();
        if (newVersion != null)
        {
            MessageBox.Show(this,
                $"Version {newVersion.Version} Installiert,\nVersion nach Neustart der Anwendung verfügbar.",
                "Updater");
        }
    }

    internal void SetCmDrText(string? text)
    {
        Invoke(() => label_CMDr.Text = $"CMDr: {text}");
    }

    internal void SetSystemText(string? text)
    {
        Invoke(() => label_System.Text = $"System: {text}");
    }

    internal void SetSuitText(string? text)
    {
        Invoke(() => label_Suit.Text = $"Anzug: {text}");
    }

    internal void SetDockedText(string? text)
    {
        Invoke(() => label_Docked.Text = $"Angedockt: {text}");
    }

    private void StartWorker()
    {
        Task.Run(() =>
        {
            while (!IsDisposed && !_closing)
            {
                if (!JournalHandler.Running)
                {
                    SetLightActive(redLight, true);
                    SetLightActive(yellowLight, false);
                    SetLightActive(greenLight, false);
                    if (_ed)
                    {
                        SetStatus("Fehler beim JournalHandler");
                        return;
                    }

                    SetStatus("Warte auf Elite");
                    Task.Run(() =>
                    {
                        Thread.Sleep(1100);
                        SetLightActive(redLight, false);
                    });
                    Task.Run(() =>
                    {
                        Thread.Sleep(800);
                        SetLightActive(yellowLight, true);
                    });
                    Task.Run(() =>
                    {
                        Thread.Sleep(400);
                        SetLightActive(greenLight, true);
                    });
                    Thread.Sleep(500);
                }
                else
                {
                    SetLightActive(redLight, false);
                    if (!_ed)
                    {
                        SetLightActive(redLight, true);
                        SetLightActive(yellowLight, false);
                        SetLightActive(greenLight, false);
                        if (_ed)
                        {
                            SetStatus("Fehler beim JournalHandler");
                            return;
                        }

                        SetStatus("Warte auf Elite");
                        Task.Run(() =>
                        {
                            Thread.Sleep(1100);
                            SetLightActive(redLight, false);
                        });
                        Task.Run(() =>
                        {
                            Thread.Sleep(800);
                            SetLightActive(yellowLight, true);
                        });
                        Task.Run(() =>
                        {
                            Thread.Sleep(400);
                            SetLightActive(greenLight, true);
                        });
                        Thread.Sleep(500);
                    }
                }

                Thread.Sleep(1000);
            }
        });
        Task.Run(() =>
        {
            while (!IsDisposed && !_closing)
            {
                var list = string.Join(", ", StateReceiver.GetState());
                var tick = string.Join(", ", StateReceiver.GetTick());
                try
                {
                    Invoke(() =>
                    {
                        if (string.IsNullOrWhiteSpace(Config.Instance.Token)) list = "Kein Token eingetragen";
                        label_SystemList.Text = list;
                        label_Tick.Text = tick;
                        if (overlayForm == null || overlayForm.IsDisposed) return;
                        overlayForm.FillList(list, tick);
                    });
                    FixLayout();
                }
                catch
                {
                    // ignored
                }

                Thread.Sleep(Config.Instance.SlowState
                    ? TimeSpan.FromSeconds(15)
                    : TimeSpan.FromSeconds(2));
                if (IsDisposed || _closing) return;
            }
        });
        Task.Run(() =>
        {
            while (!IsDisposed && !_closing)
            {
                Thread.Sleep(Convert.ToInt32(Config.Instance.CheckBackgroundIntervall));
                if (overlayForm == null || overlayForm.IsDisposed) continue;
                overlayForm.UpdateLabelTextColorBasedOnBackgroundBrightness();
            }
        });
    }

    private void SetUpEdSpy()
    {
        FixLayout();
        var warn = false;
        var p0 = false;
        var wrn = 0;
        while (!IsDisposed && !_closing)
        {
            Thread.Sleep(500);
            var processes = Process.GetProcessesByName("EliteDangerous64");
            if (processes.Length <= 0 && !Config.Instance.RemoteMode)
            {
                _ed = false;
                warn = false;
                JournalHandler.Stop();
                continue;
            }

            if (!_ed) Program.Log(Config.Instance.RemoteMode ? "RemoteMode läuft." : "EliteDangerous läuft.");
            _ed = true;
            Task.Run(() => { JournalHandler.Start(this); });
            Thread.Sleep(5000);
            if (wrn > 0) Thread.Sleep(5000);
            var edmc = Process.GetProcessesByName("EDMarketConnector");
            if (edmc.Length == 0)
            {
                Config.Instance.ExternTool = false;
                if (warn || p0) continue;
                if (wrn >= 2) p0 = true;
                if (Config.Instance.AlertEDMC)
                    Invoke(() => MessageBox.Show(this, "Bitte starte noch EDMC", "Reminder", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning));
                wrn++;
                continue;
            }

            if (warn) continue;
            SetStatus("Start Up");
            Config.Instance.ExternTool = true;
            warn = true;
        }
    }

    private void FixLayout()
    {
        try
        {
            Invoke(() =>
            {
                if (!groupBox_Orders.Visible)
                {
                    label_SystemListLabel.Top = label_Tick.Bottom + 10;
                    label_SystemList.Top = label_SystemListLabel.Bottom + 10;
                }
                else
                {
                    groupBox_Orders.Top = label_Tick.Bottom + 10;
                    label_SystemListLabel.Top = groupBox_Orders.Bottom + 10;
                    label_SystemList.Top = label_SystemListLabel.Bottom + 10;
                }

                Height = label_SystemList.Bottom + LabelSpacing + 46;
                CenterObjectHorizontally(label_SystemList);
            });
        }
        catch
        {
            // ignored
        }
    }

    private void ShowKonfig(object? sender, EventArgs e)
    {
        if (_conf == null || _conf.IsDisposed) _conf = new Konfiguration(this);
        _conf.ShowDialog(this);
    }

    private void ShowAbout(object? sender, EventArgs e)
    {
        if (_about == null || _about.IsDisposed) _about = new About();
        _about.ShowDialog(this);
    }

    private void SystemListChanged(object? sender, EventArgs e)
    {
        if (IsDisposed || _closing) return;
        FixLayout();
    }

    private void CenterObjectHorizontally(dynamic label)
    {
        if (IsDisposed || _closing) return;
        label.Left = (ClientSize.Width - label.Width) / 2;
    }

    internal void RefreshListOnKonfigChange()
    {
        var list = string.Join(", ",
            Config.Instance.ShowAll
                ? StateReceiver.SystemList
                : StateReceiver.SystemList.Take(Convert.ToInt32(Config.Instance.ListCount)).ToArray());
        var tick = string.Join(", ", StateReceiver.Tick);
        label_SystemList.Text = list;
        if (overlayForm == null || overlayForm.IsDisposed) return;
        overlayForm.FillList(list, tick);
    }

    internal void SetLightActive(PictureBox light, bool active)
    {
        if (IsDisposed || _closing) return;
        light.BackColor = active ? (Color)(light.Tag ?? "Color [Magenta]") : Color.Gray;
        if (overlayForm is { IsDisposed: false }) overlayForm.SetLightActive(light.Tag!.ToString(), active);

    }

    private void ToogleOverlayClick(object? sender, EventArgs e)
    {
        if (overlayForm == null || overlayForm.IsDisposed) overlayForm = new Overlay(this);
        overlayForm.Visible = !overlayForm.Visible;
        if (!string.IsNullOrWhiteSpace(groupBox_Orders.Text))
            overlayForm.FillOrderList(label_Orders.Text, groupBox_Orders.Text);
        SetDesign();
    }

    private void SetCircles()
    {
        using var path = new GraphicsPath();
        path.AddEllipse(new Rectangle(Point.Empty, redLight.Size));
        path.AddEllipse(new Rectangle(Point.Empty, yellowLight.Size));
        path.AddEllipse(new Rectangle(Point.Empty, greenLight.Size));
        redLight.Region = new Region(path);
        yellowLight.Region = new Region(path);
        greenLight.Region = new Region(path);
    }

    private static ToolStripMenuItem CreateToolStripMenuItem(string text, EventHandler onClick)
    {
        var menuItem = new ToolStripMenuItem
        {
            Text = text
        };
        menuItem.Click += onClick;
        return menuItem;
    }

    private void NotifyIcon_MouseDoubleClick(object? sender, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Left) return;
        WindowState = FormWindowState.Normal;
        ShowInTaskbar = true;
        Activate();
    }

    private void Form1_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (e.CloseReason == CloseReason.WindowsShutDown)
        {
            StopAll();
        }

        if (!Config.Instance.CloseMini)
        {
            _closing = true;
            JournalHandler.Running = false;
            WindowState = FormWindowState.Normal;
            if (WindowState == FormWindowState.Normal)
            {
                Config.Instance.MainLocation = Location;
            }

            Config.Save();
            return;
        }

        e.Cancel = true;
        WindowState = FormWindowState.Minimized;
        ShowInTaskbar = false;
        Task.Run(StartIpc);
    }

    private void ExitMenuItem_Click(object? sender, EventArgs e)
    {
        StopAll();
    }

    private void StopAll()
    {
        _closing = true;
        JournalHandler.Running = false;
        WindowState = FormWindowState.Normal;
        if (WindowState == FormWindowState.Normal)
        {
            Config.Instance.MainLocation = Location;
        }

        if (overlayForm is { IsDisposed: false })
        {
            Config.Instance.OverlayLocation = overlayForm.Location;
        }

        Config.Save();
        Application.Exit();
        Application.ExitThread();
    }

    private void RestoreClick(object? sender, EventArgs e)
    {
        RestoreClick();
    }

    private void RestoreClick()
    {
        WindowState = FormWindowState.Normal;
        ShowInTaskbar = true;
        try
        {
            Activate();
        }
        catch
        {
            // ignored
        }

        _pipeServer?.Dispose();
        _pipeServer = null;
    }

    public string GetSystemList()
    {
        return label_SystemList.Text;
    }

    private NamedPipeServerStream? _pipeServer;

    private void StartIpc()
    {
        _pipeServer ??= new NamedPipeServerStream("UGC App", PipeDirection.In);
        _pipeServer.WaitForConnection();
        while (_pipeServer != null)
        {
            if (_pipeServer.IsConnected)
            {
                Invoke(RestoreClick);
            }
        }
    }

    public string GetTickTime()
    {
        return label_Tick.Text;
    }

    internal void SetDesign()
    {
        var p0 = Config.Instance.DesignSel;
        if (_conf is { IsDisposed: false }) _conf.SetDesign(p0);
        if (overlayForm is { IsDisposed: false }) overlayForm.SetDesign(p0);
        switch (p0)
        {
            case 0:
                BackColor = Config.Instance.ColorDefaultBackgroundLight;
                statusStrip_Main.BackColor = Config.Instance.ColorDefaultBackgroundLight;
                foreach (Control control in Controls)
                {
                    switch (control)
                    {
                        case Label:
                        case CheckBox:
                        case GroupBox:
                            control.ForeColor = Config.Instance.ColorDefaultLabelLight;
                            break;
                    }
                }

                foreach (var item in statusStrip_Main.Items)
                {
                    if (item is ToolStripStatusLabel label)
                    {
                        label.ForeColor = Config.Instance.ColorDefaultLabelLight;
                    }
                }

                break;
            case 1:
                BackColor = Config.Instance.ColorDefaultBackgroundDark;
                statusStrip_Main.BackColor = Config.Instance.ColorDefaultBackgroundDark;
                foreach (Control control in Controls)
                {
                    switch (control)
                    {
                        case Label:
                        case CheckBox:
                        case GroupBox:
                            control.ForeColor = Config.Instance.ColorDefaultLabelDark;
                            break;
                    }
                }

                foreach (var item in statusStrip_Main.Items)
                {
                    if (item is ToolStripStatusLabel label)
                    {
                        label.ForeColor = Config.Instance.ColorDefaultLabelDark;
                    }
                }

                break;
            case 2:
                BackColor = Config.Instance.ColorMainBackground;
                statusStrip_Main.BackColor = Config.Instance.ColorMainBackground;
                foreach (Control control in Controls)
                {
                    switch (control)
                    {
                        case Label:
                        case CheckBox:
                        case GroupBox:
                            control.ForeColor = Config.Instance.ColorMainInfo;
                            break;
                    }
                }

                foreach (var item in statusStrip_Main.Items)
                {
                    if (item is ToolStripStatusLabel label)
                    {
                        label.ForeColor = Config.Instance.ColorMainInfo;
                    }
                }

                break;
        }
    }

    internal void GetSystemOrder(ulong systemAddress)
    {
        Task.Run(() =>
        {
            var orders = OrderAPI.GetSystemOrders(systemAddress);
            if (!orders.Any())
            {
                Invoke(() =>
                {
                    groupBox_Orders.Text = "";
                    groupBox_Orders.Visible = false;
                });
                overlayForm?.HideOrderList();
                FixLayout();
                return;
            }

            Invoke(() => { groupBox_Orders.Visible = true; });
            var final = orders.Aggregate("",
                (current, order) =>
                    current +
                    $"{order.Faction}:\nType: {order.Type}\n{order.Order.Replace("\r", " ").Replace("\n", " ").Replace("  ", " ")}\n------------------\n");
            overlayForm?.FillOrderList(final, $"BGS-Order: {orders.First().StarSystem}");
            Invoke(() =>
            {
                label_Orders.Text = final;
                var size = label_Orders.ClientSize;
                size.Height += 30;
                groupBox_Orders.ClientSize = size;
                groupBox_Orders.Text = $"BGS-Order: {orders.First().StarSystem}";
                FixLayout();
            });
        });
    }

    internal string GetOrder()
    {
        var rets = label_Orders.Text;
        if (string.IsNullOrWhiteSpace(rets))
        {
            overlayForm.HideOrderList();
        }

        return rets;
    }

    public void AddSucess()
    {
        if (IsDisposed || _closing || !JournalHandler.Running) return;
        toolStripStatusLabel_Spacer.Text = $"Sended: {_sends++}";
    }

    public void SetStatus(HttpResponseMessage response)
    {
        try
        {
            toolStripStatusLabel_Status.Text = $"Status: {response.StatusCode}";
        }
        catch
        {
            try
            {
                Invoke(() => { toolStripStatusLabel_Status.Text = $"Status: {response.StatusCode}"; });
            }
            catch
            {
                //ignore
            }
        }
    }

    internal void SetStatus(string response)
    {
        Invoke(() => { toolStripStatusLabel_Status.Text = $"Status: {response}"; });
    }
}
