using System.Runtime.InteropServices;
using UGC_App.EDLog;

namespace UGC_App
{
    public partial class Overlay : Form
    {
        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out Rectangle lpRect);
        const int ButtonBottomMargin = 35;
        const int LabelButtonSpacing = 10;
        private Color SystemeLight = Color.Black;
        private Color SystemeDark = Color.White;
        private Color TickLight = Color.Black;
        private Color TickDark = Color.White;
        public Overlay(Mainframe parrent)
        {
            InitializeComponent();
            parent = parrent;
            Location = parrent.Location;
            Height = label_SystemList.Bottom + LabelButtonSpacing + button1.Height + ButtonBottomMargin;
            button1.Top = label_SystemList.Bottom + LabelButtonSpacing;
            label_SystemList.SizeChanged += SystemListReSized;
            label_SystemList.Text = parrent.GetSystemList();
            label_TickTime.Text = parrent.GetTickTime();
            foreach (Control controls in panel.Controls)
            {
                if (controls is not Label) continue;
                CenterLabelHorizontally(controls);
                controls.MouseDown += OverlayForm_MouseDown;
                controls.MouseMove += OverlayForm_MouseMove;
                controls.MouseUp += OverlayForm_MouseUp;
            }
        }

        private void SystemListReSized(object? sender, EventArgs e)
        {
            Height = label_SystemList.Bottom + LabelButtonSpacing + button1.Height + ButtonBottomMargin;
            button1.Top = label_SystemList.Bottom + LabelButtonSpacing;
            CenterLabelHorizontally(label_SystemList);
            UpdateLabelTextColorBasedOnBackgroundBrightness();
        }

        private void CenterLabelHorizontally(dynamic label)
        {
            label.Left = (this.ClientSize.Width - label.Width) / 2;
        }


        private void ButtonClick(object sender, EventArgs e)
        {
            if (isDragging) return;

            //JournalHandler.Start(parent);
        }

        private void OverlayForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isMouseDown = true;
                mouseOffset = new Point(e.X, e.Y);
                lastMousePosition = Cursor.Position;
                mouseTimer.Start();
            }
        }

        private void OverlayForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown && isDragging)
            {
                Point currentScreenPos = PointToScreen(e.Location);
                Location = new Point(currentScreenPos.X - mouseOffset.X, currentScreenPos.Y - mouseOffset.Y);
            }
        }

        private void OverlayForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (isMouseDown && isDragging) UpdateLabelTextColorBasedOnBackgroundBrightness();
                isMouseDown = false;
                isDragging = false;
                mouseTimer.Stop();
            }
        }

        private void MouseTimer_Tick(object sender, EventArgs e)
        {
            Point currentMousePosition = Cursor.Position;
            if (!isDragging)
            {
                mouseTimer.Stop();
                isDragging = true;
                lastMousePosition = currentMousePosition;
            }
            else
            {
                isDragging = false;
                mouseTimer.Stop();
            }
        }

        public void FillList(string list, string tick)
        {
            label_SystemList.Text = list;
            label_TickTime.Text = tick;
        }
        internal void UpdateLabelTextColorBasedOnBackgroundBrightness()
        {
            Rectangle windowRect = this.Bounds;
            Rectangle virtualScreenRect = SystemInformation.VirtualScreen;
            windowRect.Offset(-virtualScreenRect.Left, -virtualScreenRect.Top);
            int captureWidth = windowRect.Width;
            int captureHeight = windowRect.Height;
            if (captureWidth <= 0 || captureHeight <= 0)
            {
                return;
            }
            //Point captureLocation = new Point(windowRect.X - Width / 2, Convert.ToInt32(windowRect.Y - Height / 2.5));
            Point captureLocation = new Point(windowRect.X, Convert.ToInt32(windowRect.Y));
            Size captureSize = new Size(Width, Height);
            captureLocation.X = Math.Max(0, captureLocation.X);
            captureLocation.Y = Math.Max(0, captureLocation.Y);
            captureLocation.Offset(virtualScreenRect.Left, virtualScreenRect.Top);

            using (Bitmap screenCapture = new Bitmap(captureWidth, captureHeight))
            {
                using (Graphics g = Graphics.FromImage(screenCapture))
                {
                    g.CopyFromScreen(captureLocation, Point.Empty, captureSize);
                }

                //Clipboard.SetDataObject(screenCapture, true);
                int brightnessSum = 0;
                int count = 0;
                for (int x = 0; x < screenCapture.Width; x += 10)
                {
                    for (int y = 0; y < screenCapture.Height; y += 10)
                    {
                        Color pixelColor = screenCapture.GetPixel(x, y);
                        brightnessSum += pixelColor.R + pixelColor.G + pixelColor.B;
                        count++;
                    }
                }

                int averageBrightness = brightnessSum / (count * 3);

                if (averageBrightness < 128)
                {
                    label_TickTitle.ForeColor = TickDark;
                    label_TickTime.ForeColor = TickDark;
                    label_SystemTitle.ForeColor = SystemeDark;
                    label_SystemList.ForeColor = SystemeDark;
                }
                else
                {
                    label_TickTitle.ForeColor = TickLight;
                    label_TickTime.ForeColor = TickLight;
                    label_SystemTitle.ForeColor = SystemeLight;
                    label_SystemList.ForeColor = SystemeLight;
                }


            }
        }
        internal void SetDesign(int p0)
        {
            switch (p0)
            {
                case 0:
                case 1:
                    BackColor = Config.Instance.Color_Default_Chroma;
                    TickLight = Config.Instance.Color_Default_Label_Light;
                    TickDark = Config.Instance.Color_Default_Label_Dark;
                    SystemeLight = Config.Instance.Color_Default_Label_Light;
                    SystemeDark = Config.Instance.Color_Default_Label_Dark;
                    break;
                case 2:
                    BackColor = Config.Instance.Color_Overlay_Override ? Config.Instance.Color_Overlay_Background : Config.Instance.Color_Default_Chroma;
                    TickLight = Config.Instance.Color_Overlay_Override ? Config.Instance.Color_Overlay_Tick_Light : Config.Instance.Color_Default_Label_Light;
                    TickDark = Config.Instance.Color_Overlay_Override ? Config.Instance.Color_Overlay_Tick_Dark : Config.Instance.Color_Default_Label_Dark;
                    SystemeLight = Config.Instance.Color_Overlay_Override ? Config.Instance.Color_Overlay_Systeme_Light : Config.Instance.Color_Default_Label_Light;
                    SystemeDark = Config.Instance.Color_Overlay_Override ? Config.Instance.Color_Overlay_Systeme_Dark : Config.Instance.Color_Default_Label_Dark;
                    break;
            }
            TransparencyKey = BackColor;
            UpdateLabelTextColorBasedOnBackgroundBrightness();
        }
    }
}
