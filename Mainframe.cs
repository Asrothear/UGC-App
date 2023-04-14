using System.Drawing.Drawing2D;
using System.IO.Pipes;
using System.Reflection;
using System.Runtime.CompilerServices;
using Squirrel;
using UGC_App.EDLog;
using UGC_App.WebClient;

namespace UGC_App;
public partial class Mainframe : Form
{
    private Konfiguration conf;
    private About about;
    internal Design desg;
    private Updater _updater;
    const int LabelSpacing = 35;
    private bool closing = false;
    public Mainframe()
    {
        InitializeComponent();
        Task.Run(() => { JournalHandler.Start(this); });
        contextMenuStrip.Items.AddRange(new ToolStripItem[]
        {
            CreateToolStripMenuItem("Wiederherstellen", RestoreClick),
            CreateToolStripMenuItem("Toogle Overlay", ToogleOverlayClick),
            new ToolStripSeparator(),
            CreateToolStripMenuItem("Über", ShowAbout),
            CreateToolStripMenuItem("Beenden", ExitMenuItem_Click),
        });
        notifyIcon.ContextMenuStrip = contextMenuStrip;
        notifyIcon.MouseDoubleClick += NotifyIcon_MouseDoubleClick;
        label_SystemList.SizeChanged += SystemListChanged;
        toolStripMenuItem_Overlay.Click += ToogleOverlayClick;
        toolStripMenuItem_About.Click += ShowAbout;
        toolStripMenuItem_Settings.Click += ShowKonfig;
        toolStripMenuItem_CheckForUpdates.Click += CheckUpdates;
        SetCircles();
        StartWorker();
        CenterObjectHorizontally(label_SystemList);
        toolStripStatusLabel_Version.Text = $"Version {Properties.Settings.Default.Version}";
        Height = label_SystemList.Bottom + LabelSpacing + 46;
        label_CMDrText = Properties.Settings.Default.CMDR;
        SetDesign();
    }

    private async void CheckUpdates(object? sender, EventArgs e)
    {
        using var mgr = new UpdateManager(Properties.Settings.Default.Update_Url,"UGC-App");
        var Infos = await mgr.CheckForUpdate();
        if(Infos.CurrentlyInstalledVersion.Version >= Infos.FutureReleaseEntry.Version)return;
        DialogResult dialogResult = MessageBox.Show($"Eine neue Version ist verfügbar.\n{Infos.CurrentlyInstalledVersion.Version}->{Infos.FutureReleaseEntry.Version}\nUpdate Installieren?", "Updater", MessageBoxButtons.YesNo);
        if(dialogResult == DialogResult.Yes)
        {
            var newVersion = await mgr.UpdateApp();
            if (newVersion != null)
            {
                UpdateManager.RestartApp(null);
            }
        }
    }


    // Option 2: Verwenden Sie eine öffentliche Eigenschaft, um den Label-Text zu aktualisieren
    internal string label_CMDrText
    {
        get { return label_CMDr.Text; }
        set { label_CMDr.Text = $"CMDr: {value}"; }
    }

    private void StartWorker()
    {

        Task.Run(() =>
        {
            while (!IsDisposed && !closing)
            {
                if (!JournalHandler.running)
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
                Invoke(() =>
                {
                    label_SystemList.Text = list;
                    label_Tick.Text = tick;
                    if (overlayForm == null || overlayForm.IsDisposed) return;
                    overlayForm.FillList(list, tick);
                });
                Thread.Sleep(Properties.Settings.Default.SlowState
                    ? TimeSpan.FromSeconds(5)
                    : TimeSpan.FromSeconds(15));
            }
        });
    }
    private void ShowKonfig(object sender, EventArgs e)
    {
        if (conf == null || conf.IsDisposed) conf = new Konfiguration(this);
        conf.ShowDialog(this);
    }

    private void ShowAbout(object? sender, EventArgs e)
    {
        if (about == null || about.IsDisposed) about = new();
        about.ShowDialog(this);
    }

    internal void ShowDesign()
    {
        if (desg == null || desg.IsDisposed) desg = new(this);
        desg.Show();
    }

    private void SystemListChanged(object? sender, EventArgs e)
    {
        Height = label_SystemList.Bottom + LabelSpacing + 46;
        CenterObjectHorizontally(label_SystemList);
    }
    private void CenterObjectHorizontally(dynamic label)
    {
        label.Left = (this.ClientSize.Width - label.Width) / 2;
    }
    internal void RefreshListOnKonfigChange()
    {
        var list = string.Join(", ", Properties.Settings.Default.Show_All ? StateReceiver.SystemList : StateReceiver.SystemList.Take(Convert.ToInt32(Properties.Settings.Default.ListCount)).ToArray());
        var tick = string.Join(", ", StateReceiver.Tick);
        label_SystemList.Text = list;
        if (overlayForm == null || overlayForm.IsDisposed) return;
        overlayForm.FillList(list, tick);
    }

    internal void SetLightActive(PictureBox light, bool active)
    {
        light.BackColor = active ? (Color)light.Tag : Color.Gray;
    }
    private void ToogleOverlayClick(object sender, EventArgs e)
    {
        if (overlayForm == null || overlayForm.IsDisposed) overlayForm = new Overlay(this);
        overlayForm.Visible = !overlayForm.Visible;
        SetDesign();
    }
    private void SetCircles()
    {
        using (GraphicsPath path = new GraphicsPath())
        {
            path.AddEllipse(new Rectangle(Point.Empty, redLight.Size));
            path.AddEllipse(new Rectangle(Point.Empty, yellowLight.Size));
            path.AddEllipse(new Rectangle(Point.Empty, greenLight.Size));
            redLight.Region = new Region(path);
            yellowLight.Region = new Region(path);
            greenLight.Region = new Region(path);
        }
    }
    private ToolStripMenuItem CreateToolStripMenuItem(string text, EventHandler onClick)
    {
        ToolStripMenuItem menuItem = new ToolStripMenuItem
        {
            Text = text
        };
        menuItem.Click += onClick;
        return menuItem;
    }
    private void NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            WindowState = FormWindowState.Normal;
            ShowInTaskbar = true;
            Activate();
        }
    }
    private void Form1_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (!Properties.Settings.Default.CloseMini)
        {
            closing = true;
            JournalHandler.running = false;
            WindowState = FormWindowState.Normal;
            Properties.Settings.Default.FormSize = this.Size;
            Properties.Settings.Default.FormLocation = this.Location;
            Properties.Settings.Default.Save();
            return;
        };
        e.Cancel = true;
        WindowState = FormWindowState.Minimized;
        ShowInTaskbar = false;
        Task.Run(StartIPC);
    }
    private void ExitMenuItem_Click(object sender, EventArgs e)
    {
        closing = true;
        JournalHandler.running = false;
        WindowState = FormWindowState.Normal;
        Properties.Settings.Default.FormSize = this.Size;
        Properties.Settings.Default.FormLocation = this.Location;
        Properties.Settings.Default.Save();
        Application.Exit();
        Application.ExitThread();
    }

    private void RestoreClick(object sender, EventArgs e)
    {
        RestoreClick();
    }

    internal void RestoreClick()
    {
        WindowState = FormWindowState.Normal;
        ShowInTaskbar = true;
        Activate();
        pipeServer.Dispose();
        pipeServer = null;
    }

    public string GetSystemList()
    {
        return label_SystemList.Text;
    }
    private NamedPipeServerStream pipeServer;

    internal void StartIPC()
    {
        if (pipeServer == null) pipeServer = new NamedPipeServerStream("UGC App", PipeDirection.In);
        pipeServer.WaitForConnection();
        while (pipeServer != null)
        {
            if (pipeServer.IsConnected)
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
        var p0 = Properties.Settings.Default.Design_Sel;
        if (conf != null && !conf.IsDisposed) conf.SetDesign(p0);
        if (desg != null && !desg.IsDisposed) desg.SetDesign(p0);
        if (overlayForm != null && !overlayForm.IsDisposed) overlayForm.SetDesign(p0);
        switch (p0)
        {
            case 0:
                BackColor = Properties.Settings.Default.Color_Default_Background_Light;
                statusStrip_Main.BackColor = Properties.Settings.Default.Color_Default_Background_Light;
                foreach (Control control in Controls)
                {
                    if (control is Label) control.ForeColor = Properties.Settings.Default.Color_Default_Label_Light;
                    if (control is CheckBox) control.ForeColor = Properties.Settings.Default.Color_Default_Label_Light;
                }
                break;
            case 1:
                BackColor = Properties.Settings.Default.Color_Default_Background_Dark;
                statusStrip_Main.BackColor = Properties.Settings.Default.Color_Default_Background_Dark;
                foreach (Control control in Controls)
                {
                    if (control is Label) control.ForeColor = Properties.Settings.Default.Color_Default_Label_Dark;
                    if (control is CheckBox) control.ForeColor = Properties.Settings.Default.Color_Default_Label_Dark;
                }
                break;
            case 2:
                BackColor = Properties.Settings.Default.Color_Main_Background;
                statusStrip_Main.BackColor = Properties.Settings.Default.Color_Main_Background;
                foreach (Control control in Controls)
                {
                    if (control is Label) control.ForeColor = Properties.Settings.Default.Color_Main_Info;
                    if (control is CheckBox) control.ForeColor = Properties.Settings.Default.Color_Main_Info;
                }
                break;
        }
    }
}
