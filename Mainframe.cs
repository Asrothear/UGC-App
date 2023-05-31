using System.Drawing.Drawing2D;
using System.IO.Pipes;
using Org.BouncyCastle.Math.EC;
using Squirrel;
using UGC_App.EDLog;
using UGC_App.Order;
using UGC_App.Order.DashViews;
using UGC_App.WebClient;

namespace UGC_App;
public partial class Mainframe : Form
{
    private Konfiguration? _conf;
    private About? _about;
    private Dashboard? _Dashboard;
    private const int LabelSpacing = 35;
    private bool _closing;
    private int _sends;

    public Mainframe()
    {
        InitializeComponent();
        SizeChanged += (_, _) => FixLayout();
        Task.Run(() => { JournalHandler.Start(this); });
        contextMenuStrip.Items.AddRange(new ToolStripItem[]
        {
            CreateToolStripMenuItem("Wiederherstellen", RestoreClick),
            CreateToolStripMenuItem("Toogle Overlay", ToogleOverlayClick),
            CreateToolStripMenuItem("Einstellungen", ShowKonfig),
            new ToolStripSeparator(),
            CreateToolStripMenuItem("Über", ShowAbout),
            CreateToolStripMenuItem("auf Updates Prüfen", CheckUpdates),
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
        dashboardToolStripMenuItem.Click += ShowOrderDashboard;
        SetCircles();
        StartWorker();
        CenterObjectHorizontally(label_SystemList);
        toolStripStatusLabel_Version.Text = $"Version {Config.Instance.Version}";
        Height = label_SystemList.Bottom + LabelSpacing + 46;
        label_CMDr.Text = $"CMDr: {Config.Instance.Cmdr}";
        label_System.Text = $"System: {Config.Instance.LastSystem}";
        label_Docked.Text = $"Angedockt: {Config.Instance.LastDocked}";
        SetDesign();
        TopMost = Config.Instance.AlwaysOnTop;
        Program.SetStartup(Config.Instance.AutoStart);
        Task.Run(() =>
        {
            Thread.Sleep(10);
            if (WindowState == FormWindowState.Normal)
            {
                Invoke(() =>
                {
                    Location = Config.Instance.MainLocation;
                });
            }
        });
    }

    private void ShowOrderDashboard(object? sender, EventArgs e)
    {
        var def = Cursor.Current;
        Cursor.Current = Cursors.WaitCursor;
        if (_Dashboard == null || _Dashboard.IsDisposed) _Dashboard = new Dashboard();
        _Dashboard.Visible = true;
        _Dashboard.Activate();
        _Dashboard.AttachView(new SystemList());
        Cursor.Current = def;
        //SetDesign();
    }

    private static async void CheckUpdates(object? sender, EventArgs e)
    {
        using var mgr = new UpdateManager(Config.Instance.UpdateUrl, "UGC-App");
        var infos = await mgr.CheckForUpdate();
        if (infos.CurrentlyInstalledVersion.Version >= infos.FutureReleaseEntry.Version &&
            infos.CurrentlyInstalledVersion.SHA1 == infos.FutureReleaseEntry.SHA1)
        {
            MessageBox.Show("Die aktuellste Version ist breits Installiert.", "Updater");
            return;
        }
        var dialogResult = MessageBox.Show($"Eine neue Version ist verfügbar.\n{infos.CurrentlyInstalledVersion.Version}->{infos.FutureReleaseEntry.Version}\nUpdate Installieren?", "Updater", MessageBoxButtons.YesNo);
        if (dialogResult != DialogResult.Yes) return;
        var newVersion = await mgr.UpdateApp();
        if (newVersion != null)
        {
            MessageBox.Show("Version {newVersion.Version} Installiert,\nVersion nach Neustart der Anwendung verfügbar.", "Updater");
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
        var list = string.Join(", ", Config.Instance.ShowAll ? StateReceiver.SystemList : StateReceiver.SystemList.Take(Convert.ToInt32(Config.Instance.ListCount)).ToArray());
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
        { Activate(); }
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

    public void AddSucess()
    {
        if (IsDisposed || _closing || !JournalHandler.Running) return;
        toolStripStatusLabel_Spacer.Text = $"Sended: {_sends++}";
    }

    public void SetStatus(HttpResponseMessage response)
    {
        Invoke(() =>
        {
            toolStripStatusLabel_Status.Text = $"Status: {response.StatusCode}";
        });
    }
}
