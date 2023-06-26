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
    private Dashboard? _Dashboard;
    private const int LabelSpacing = 35;
    private bool _closing;
    private int _sends;
    KeyboardHookManager keyboardHookManager = new();
    private static readonly Timer CacheTimer = new Timer();

    public Mainframe()
    {
        InitializeComponent();
        Task.Run(() =>
        {
            CacheHandler.InitAll();
        });
        CacheTimer.Interval += 60 * (60 * 1000);
        CacheTimer.Elapsed += (sender, args) => { Program.Log("CacheTimer");CacheHandler.InitAll(true); };
        CacheTimer.Start();
        keyboardHookManager.Start();
        Task.Run(() => { JournalHandler.Start(this); });
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
        StartWorker();
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
        Task.Run(() =>
        {
            Thread.Sleep(10);
            if (WindowState == FormWindowState.Normal)
            {
                Invoke(() => { Location = Config.Instance.MainLocation; });
            }

            SizeChanged += (_, _) => FixLayout();
        });
        keyboardHookManager.RegisterHotkey(
            new[]
            {
                NonInvasiveKeyboardHookLibrary.ModifierKeys.Control, NonInvasiveKeyboardHookLibrary.ModifierKeys.Alt
            }, (int)Keys.C, FisrtToClip);

    }

    private void ResetPos(object? sender, EventArgs e)
    {
        var res = MessageBox.Show("Hiermit werden die Positionen des Hauptfensters und des Overlays der UGC-App zurückgesetzt.\nDies ist für den fall das die UGC-App nicht mehr sichtbar ist.\nFortfahren?","Position Reset",MessageBoxButtons.YesNo, MessageBoxIcon.Information,MessageBoxDefaultButton.Button2,
            MessageBoxOptions.ServiceNotification
        );
        if(res != DialogResult.Yes)return;
        Config.Instance.MainLocation = new Point(50, 50);
        Config.Instance.OverlayLocation = new Point(50, 50);
        Config.Save();
        Location = Config.Instance.MainLocation;
        if (overlayForm is { IsDisposed: false }) overlayForm.Location = Config.Instance.OverlayLocation;
        Activate();
    }

    private void FisrtToClip()
    {
        Regex regex = new Regex(@"(.*?)(?:\s*:\s*\d+(?:\.\d+)?\s*ly)?$");
        var text = label_SystemList.Text.Split(",").First();
        if (string.IsNullOrWhiteSpace(text) || text == "Alles Aktuell!") return;
        SystemSounds.Asterisk.Play();
        Match match = regex.Match(text);
        if (match.Success)
        {
            string result = match.Groups[1].Value;

            Invoke(() => Clipboard.SetText(result.Replace("\u00A0", " ")));
        }
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
        if (_Dashboard == null || _Dashboard.IsDisposed)
        {
            _Dashboard = new Dashboard();
            _Dashboard.Show(this);
        }

        _Dashboard.Activate();
        switch (type)
        {
            case 1:
                _Dashboard.SwitchView(new OrderList(_Dashboard));
                break;
            case 2:
                _Dashboard.SwitchView(new SystemList());
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
            MessageBox.Show("Die aktuellste Version ist breits Installiert.", "Updater");
            return;
        }

        TopMost = false;
        var dialogResult =
            MessageBox.Show(
                $"Eine neue Version ist verfügbar.\n{infos.CurrentlyInstalledVersion.Version}->{infos.FutureReleaseEntry.Version}\nUpdate Installieren?",
                "Updater", MessageBoxButtons.YesNo);
        if (dialogResult != DialogResult.Yes) return;
        var newVersion = await mgr.UpdateApp();
        if (newVersion != null)
        {
            MessageBox.Show(
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
                }
                else
                {
                    SetLightActive(redLight, false);
                }

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
        var tick = string.Join(", ", StateReceiver.Tick ?? new[] { "-Warte auf Daten-" });
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
        if(!string.IsNullOrWhiteSpace(groupBox_Orders.Text))overlayForm.FillOrderList(label_Orders.Text,groupBox_Orders.Text);
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

    internal void GetSystemOrder(ulong SystemAddress)
    {
        Task.Run(() =>
        {
            var orders = OrderAPI.GetSystemOrders(SystemAddress);
            if (orders == null || !orders.Any())
            {
                Invoke(() =>
                {
                    groupBox_Orders.Text = "";
                    groupBox_Orders.Visible = false; });
                overlayForm?.HideOrderList();
                FixLayout();
                return;
            }

            ;
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
        Invoke(() => { toolStripStatusLabel_Status.Text = $"Status: {response.StatusCode}"; });
    }
}
