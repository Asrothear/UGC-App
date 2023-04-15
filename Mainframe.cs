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
    private int sends = 0;
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
        toolStripStatusLabel_Version.Text = $"Version {Config.Instance.Version}";
        Height = label_SystemList.Bottom + LabelSpacing + 46;
        label_CMDr.Text = $"CMDr: {Config.Instance.CMDR}";
        label_System.Text = $"System: {Config.Instance.LastSystem}";
        label_Docked.Text = $"Angedockt: {Config.Instance.LastDocked}";
        SetDesign();
    }

    private async void CheckUpdates(object? sender, EventArgs e)
    {
        using var mgr = new UpdateManager(Config.Instance.Update_Url,"UGC-App");
        var Infos = await mgr.CheckForUpdate();
        if(Infos.CurrentlyInstalledVersion.Version >= Infos.FutureReleaseEntry.Version)return;
        DialogResult dialogResult = MessageBox.Show($"Eine neue Version ist verfügbar.\n{Infos.CurrentlyInstalledVersion.Version}->{Infos.FutureReleaseEntry.Version}\nUpdate Installieren?", "Updater", MessageBoxButtons.YesNo);
        if(dialogResult == DialogResult.Yes)
        {
            var newVersion = await mgr.UpdateApp();
            if (newVersion != null)
            {
                MessageBox.Show($"Version {newVersion.Version} Installiert,\nbitte Anwendung neustarten.", "Updater");
            }
        }
    }

    internal void SetCMDrText(string text)
    {
        Invoke(() => { label_CMDr.Text = $"CMDr: {text}"; });
    }
    internal void SetSystemText(string text)
    {
        Invoke(() => { label_System.Text = $"System: {text}"; });
    }
    internal void SetDockedText(string text)
    {
        Invoke(() => { label_Docked.Text = $"Angedockt: {text}"; });
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
                Invoke(() =>
                {
                    Height = label_SystemList.Bottom + LabelSpacing + 46;
                    CenterObjectHorizontally(label_SystemList);
                });
                Thread.Sleep(Config.Instance.SlowState
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
        var list = string.Join(", ", Config.Instance.Show_All ? StateReceiver.SystemList : StateReceiver.SystemList.Take(Convert.ToInt32(Config.Instance.ListCount)).ToArray());
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
        if (!Config.Instance.CloseMini)
        {
            closing = true;
            JournalHandler.running = false;
            WindowState = FormWindowState.Normal;
            Config.Instance.FormSize = this.Size;
            Config.Instance.FormLocation = this.Location;
            Config.Save();
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
        Config.Instance.FormSize = this.Size;
        Config.Instance.FormLocation = this.Location;
        Config.Save();
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
        var p0 = Config.Instance.Design_Sel;
        if (conf != null && !conf.IsDisposed) conf.SetDesign(p0);
        if (desg != null && !desg.IsDisposed) desg.SetDesign(p0);
        if (overlayForm != null && !overlayForm.IsDisposed) overlayForm.SetDesign(p0);
        switch (p0)
        {
            case 0:
                BackColor = Config.Instance.Color_Default_Background_Light;
                statusStrip_Main.BackColor = Config.Instance.Color_Default_Background_Light;
                foreach (Control control in Controls)
                {
                    if (control is Label) control.ForeColor = Config.Instance.Color_Default_Label_Light;
                    if (control is CheckBox) control.ForeColor = Config.Instance.Color_Default_Label_Light;
                }
                break;
            case 1:
                BackColor = Config.Instance.Color_Default_Background_Dark;
                statusStrip_Main.BackColor = Config.Instance.Color_Default_Background_Dark;
                foreach (Control control in Controls)
                {
                    if (control is Label) control.ForeColor = Config.Instance.Color_Default_Label_Dark;
                    if (control is CheckBox) control.ForeColor = Config.Instance.Color_Default_Label_Dark;
                }
                break;
            case 2:
                BackColor = Config.Instance.Color_Main_Background;
                statusStrip_Main.BackColor = Config.Instance.Color_Main_Background;
                foreach (Control control in Controls)
                {
                    if (control is Label) control.ForeColor = Config.Instance.Color_Main_Info;
                    if (control is CheckBox) control.ForeColor = Config.Instance.Color_Main_Info;
                }
                break;
        }
    }

    public void AddSucess()
    {
        toolStripStatusLabel_Spacer.Text = $"Sended: {sends++}";
    }
}
