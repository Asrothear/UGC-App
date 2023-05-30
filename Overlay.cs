using System.Drawing.Drawing2D;
using Timer = System.Windows.Forms.Timer;

namespace UGC_App;

public partial class Overlay : Form
{
    const int LabelBottomMargin = 35;
    private Color _systemeLight = Color.Black;
    private Color _systemeDark = Color.White;
    private Color _tickLight = Color.Black;
    private Color _tickDark = Color.White;
    private bool _isDragging;
    private bool _isMouseDown;
    private bool _locked;
    private Point _mouseOffset;
    private Timer _mouseTimer = new();

    public Overlay(Mainframe parrent)
    {
        InitializeComponent();
        parent = parrent;
        Location = parrent.Location;
        label_SystemList.SizeChanged += SystemListReSized;
        label_SystemList.Text = parrent.GetSystemList();
        label_TickTime.Text = parrent.GetTickTime();
        Height = label_SystemList.Bottom + LabelBottomMargin;
        foreach (Control controls in panel.Controls)
        {
            if (controls is not Label) continue;
            CenterLabelHorizontally(controls);
            controls.MouseDown += OverlayForm_MouseDown;
            controls.MouseMove += OverlayForm_MouseMove;
            controls.MouseUp += OverlayForm_MouseUp;
        }

        Task.Run(() =>
        {
            Thread.Sleep(10);
            Invoke(() => { Location = Config.Instance.OverlayLocation; });

        });
        SetCircles();
    }

    private void SystemListReSized(object? sender, EventArgs e)
    {
        CenterLabelHorizontally(label_SystemList);
        Height = label_SystemList.Bottom + LabelBottomMargin;
        UpdateLabelTextColorBasedOnBackgroundBrightness();
    }

    private void CenterLabelHorizontally(dynamic label)
    {
        label.Left = (ClientSize.Width - label.Width) / 2;
    }

    private void OverlayForm_MouseDown(object? sender, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Left) return;
        _isMouseDown = true;
        _mouseOffset = new Point(e.X, e.Y);
        _mouseTimer.Start();
    }

    private void OverlayForm_MouseMove(object? sender, MouseEventArgs e)
    {
        if (!_isMouseDown || !_isDragging) return;
        var currentScreenPos = PointToScreen(e.Location);
        Location = new Point(currentScreenPos.X - _mouseOffset.X, currentScreenPos.Y - _mouseOffset.Y);
    }

    private void OverlayForm_MouseUp(object? sender, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Left) return;
        _locked = false;
        if (_isMouseDown && _isDragging) UpdateLabelTextColorBasedOnBackgroundBrightness();
        _isMouseDown = false;
        _isDragging = false;
        _mouseTimer.Stop();
        Config.Instance.OverlayLocation = Location;
        Config.Save();
    }

    private void MouseTimer_Tick(object sender, EventArgs e)
    {
        if (!_isDragging)
        {
            _mouseTimer.Stop();
            _isDragging = true;
            _locked = true;
        }
        else
        {
            _locked = false;
            _isDragging = false;
            _mouseTimer.Stop();
        }
    }

    public void FillList(string list, string tick)
    {
        label_SystemList.Text = list;
        label_TickTime.Text = tick;
    }
    internal void UpdateLabelTextColorBasedOnBackgroundBrightness()
    {
        if (_locked) return;
        _locked = true;
        var windowRect = Bounds;
        var virtualScreenRect = SystemInformation.VirtualScreen;
        windowRect.Offset(-virtualScreenRect.Left, -virtualScreenRect.Top);
        var captureWidth = windowRect.Width;
        var captureHeight = windowRect.Height;
        if (captureWidth <= 0 || captureHeight <= 0)
        {
            _locked = false;
            return;
        }
        //Point captureLocation = new Point(windowRect.X - Width / 2, Convert.ToInt32(windowRect.Y - Height / 2.5));
        var captureLocation = new Point(windowRect.X, Convert.ToInt32(windowRect.Y));
        var captureSize = new Size(Width, Height);
        captureLocation.X = Math.Max(0, captureLocation.X);
        captureLocation.Y = Math.Max(0, captureLocation.Y);
        captureLocation.Offset(virtualScreenRect.Left, virtualScreenRect.Top);

        using var screenCapture = new Bitmap(captureWidth, captureHeight);
        using (var g = Graphics.FromImage(screenCapture))
        {
            try
            {
                g.CopyFromScreen(captureLocation, Point.Empty, captureSize);
            }
            catch
            {
                _locked = false;
                return;
            }
        }
        //Clipboard.SetDataObject(screenCapture, true);
        float col = 0;
        var count = 0;
        for (var x = 0; x < screenCapture.Width; x += 10)
        {
            for (var y = 0; y < screenCapture.Height; y += 10)
            {
                var pixelColor = screenCapture.GetPixel(x, y);
                col += pixelColor.GetBrightness();
                count++;
            }
        }
        var cold = col / count * 100;
        var colds = Math.Floor(cold);
        switch (colds)
        {
            case < 48:
                label_TickTitle.ForeColor = _tickDark;
                label_TickTime.ForeColor = _tickDark;
                label_SystemTitle.ForeColor = _systemeDark;
                label_SystemList.ForeColor = _systemeDark;
                break;
            case > 49:
                label_TickTitle.ForeColor = _tickLight;
                label_TickTime.ForeColor = _tickLight;
                label_SystemTitle.ForeColor = _systemeLight;
                label_SystemList.ForeColor = _systemeLight;
                break;
        }
        _locked = false;
    }
    internal void SetDesign(int p0)
    {
        switch (p0)
        {
            case 0:
            case 1:
                BackColor = Config.Instance.ColorDefaultChroma;
                _tickLight = Config.Instance.ColorDefaultLabelLight;
                _tickDark = Config.Instance.ColorDefaultLabelDark;
                _systemeLight = Config.Instance.ColorDefaultLabelLight;
                _systemeDark = Config.Instance.ColorDefaultLabelDark;
                break;
            case 2:
                BackColor = Config.Instance.ColorOverlayOverride ? Config.Instance.ColorOverlayBackground : Config.Instance.ColorDefaultChroma;
                _tickLight = Config.Instance.ColorOverlayOverride ? Config.Instance.ColorOverlayTickLight : Config.Instance.ColorDefaultLabelLight;
                _tickDark = Config.Instance.ColorOverlayOverride ? Config.Instance.ColorOverlayTickDark : Config.Instance.ColorDefaultLabelDark;
                _systemeLight = Config.Instance.ColorOverlayOverride ? Config.Instance.ColorOverlaySystemeLight : Config.Instance.ColorDefaultLabelLight;
                _systemeDark = Config.Instance.ColorOverlayOverride ? Config.Instance.ColorOverlaySystemeDark : Config.Instance.ColorDefaultLabelDark;
                break;
        }
        TransparencyKey = BackColor;
        UpdateLabelTextColorBasedOnBackgroundBrightness();
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
    internal void SetLightActive(string? lightTag, bool active)
    {
        var trg = lightTag switch
        {
            "Color [Red]" => redLight,
            "Color [Yellow]" => yellowLight,
            "Color [Green]" => greenLight,
            _ => null
        };
        if (trg == null) return;
        SetLightActive(trg, active);
    }

    private void SetLightActive(Control light, bool active)
    {
        if (IsDisposed) return;
        light.BackColor = active ? (Color)(light.Tag ?? "Color [Magenta]") : Color.Gray;
    }
}