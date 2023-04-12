using System.Drawing.Drawing2D;
using System.IO.Pipes;
using System.Reflection;
using System.Runtime.CompilerServices;
using UGC_App.EDLog;
using UGC_App.WecClient;

namespace UGC_App;
public partial class Mainframe : Form
{
    Konfiguration conf;
    const int LabelSpacing = 35;
    public Mainframe()
    {
        InitializeComponent();
        contextMenuStrip.Items.AddRange(new ToolStripItem[]
        {
            CreateToolStripMenuItem("Wiederherstellen", RestoreClick),
            CreateToolStripMenuItem("Fill List", ListFiller),
            CreateToolStripMenuItem("Toogle Color", ToogleColorClick),
            CreateToolStripMenuItem("Toogle Overlay", ToogleOverlayClick),
            new ToolStripSeparator(),
            CreateToolStripMenuItem("Beenden", ExitMenuItem_Click),
        });
        notifyIcon.ContextMenuStrip = contextMenuStrip;
        notifyIcon.MouseDoubleClick += NotifyIcon_MouseDoubleClick;
        SetCircles();
        toolStripStatusLabel_Version.Text = $"Version {Application.ProductVersion}";
        label_SystemList.SizeChanged += SystemListChanged;
        CenterObjectHorizontally(label_SystemList);
        Height = label_SystemList.Bottom + LabelSpacing + 46;
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

    private void ListFiller(object? sender, EventArgs e)
    {
        Task.Run(() =>
        {
            var list = string.Join(", ", StateReceiver.GetState());
            Invoke(() =>
            {
                label_SystemList.Text = list;
                if (overlayForm == null || overlayForm.IsDisposed) return;
                overlayForm.FillList(list);
            });
        });
    }

    internal void SetLightActive(PictureBox light, bool active)
    {
        light.BackColor = active ? (Color)light.Tag : Color.Gray;
    }

    private void toolStripMenuItem2_Click(object sender, EventArgs e)
    {
        if (conf == null || conf.IsDisposed) conf = new Konfiguration();
        conf.ShowDialog(this);
    }
    private void ToogleColorClick(object sender, EventArgs e)
    {
        overlayForm?.UpdateLabelTextColorBasedOnBackgroundBrightness();
        SetLightActive(redLight, true);
    }
    private void ToogleOverlayClick(object sender, EventArgs e)
    {
        if (overlayForm == null || overlayForm.IsDisposed) overlayForm = new Overlay(this);
        overlayForm.Visible = !overlayForm.Visible;
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

            WindowState = FormWindowState.Maximized;
            WindowState = FormWindowState.Normal;
            ShowInTaskbar = true;
            Activate();
        }
    }
    private void Form1_FormClosing(object sender, FormClosingEventArgs e)
    {
        e.Cancel = true;
        WindowState = FormWindowState.Minimized;
        ShowInTaskbar = false;
        Task.Run(StartIPC);
    }
    private void ExitMenuItem_Click(object sender, EventArgs e)
    {
        notifyIcon.Visible = false;
        JournalHandler.running = false;
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
}
