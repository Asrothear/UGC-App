namespace UGC_App;

public partial class About : Form
{
    public About()
    {
        InitializeComponent();
        TopMost = Config.Instance.AlwaysOnTop;
        label2.Text = $"Version {Config.Instance.Version}{Config.Instance.VersionMeta}";
        foreach (Control controller in Controls)
        {
            if (controller is not Label) continue;
            controller.Left = (ClientSize.Width - controller.Width) / 2;
        }
    }
}