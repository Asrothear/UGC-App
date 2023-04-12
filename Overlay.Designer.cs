namespace UGC_App
{
    partial class Overlay
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        //private TransparentPanel panel;
        private Mainframe parent;
        private Panel panel;
        private Button button1;
        private bool isDragging;
        private bool isMouseDown;
        private Point mouseOffset;
        private Point lastMousePosition;
        private System.Windows.Forms.Timer mouseTimer;
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Overlay));
            panel = new Panel();
            label_SystemTitle = new Label();
            label_TickTime = new Label();
            label_TickTitle = new Label();
            label_SystemList = new Label();
            button1 = new Button();
            mouseTimer = new System.Windows.Forms.Timer(components);
            panel.SuspendLayout();
            SuspendLayout();
            // 
            // panel
            // 
            panel.BackColor = Color.Transparent;
            panel.Controls.Add(label_SystemTitle);
            panel.Controls.Add(label_TickTime);
            panel.Controls.Add(label_TickTitle);
            panel.Controls.Add(label_SystemList);
            panel.Controls.Add(button1);
            panel.Dock = DockStyle.Fill;
            panel.Location = new Point(0, 0);
            panel.Name = "panel";
            panel.Size = new Size(260, 220);
            panel.TabIndex = 0;
            panel.MouseDown += OverlayForm_MouseDown;
            panel.MouseMove += OverlayForm_MouseMove;
            panel.MouseUp += OverlayForm_MouseUp;
            // 
            // label_SystemTitle
            // 
            label_SystemTitle.AutoSize = true;
            label_SystemTitle.Location = new Point(109, 76);
            label_SystemTitle.Name = "label_SystemTitle";
            label_SystemTitle.Size = new Size(54, 15);
            label_SystemTitle.TabIndex = 4;
            label_SystemTitle.Text = "Systeme:";
            // 
            // label_TickTime
            // 
            label_TickTime.AutoSize = true;
            label_TickTime.Location = new Point(109, 33);
            label_TickTime.Name = "label_TickTime";
            label_TickTime.Size = new Size(86, 15);
            label_TickTime.TabIndex = 3;
            label_TickTime.Text = "Loading Data...";
            // 
            // label_TickTitle
            // 
            label_TickTitle.AutoSize = true;
            label_TickTitle.Location = new Point(109, 18);
            label_TickTitle.Name = "label_TickTitle";
            label_TickTitle.Size = new Size(31, 15);
            label_TickTitle.TabIndex = 2;
            label_TickTitle.Text = "Tick:";
            // 
            // label_SystemList
            // 
            label_SystemList.AutoSize = true;
            label_SystemList.ForeColor = Color.White;
            label_SystemList.Location = new Point(25, 110);
            label_SystemList.MaximumSize = new Size(215, 0);
            label_SystemList.Name = "label_SystemList";
            label_SystemList.Size = new Size(212, 45);
            label_SystemList.TabIndex = 1;
            label_SystemList.Text = "label1 abcd   sdf  sdf  df efghijklmnopq\nlabel2\nlabel3";
            label_SystemList.TextAlign = ContentAlignment.TopCenter;
            // 
            // button1
            // 
            button1.BackColor = Color.Transparent;
            button1.Location = new Point(95, 185);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 0;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = false;
            button1.Click += ButtonClick;
            button1.MouseDown += OverlayForm_MouseDown;
            button1.MouseMove += OverlayForm_MouseMove;
            button1.MouseUp += OverlayForm_MouseUp;
            // 
            // mouseTimer
            // 
            mouseTimer.Interval = 200;
            mouseTimer.Tick += MouseTimer_Tick;
            // 
            // Overlay
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BackColor = Color.DarkGoldenrod;
            ClientSize = new Size(260, 220);
            Controls.Add(panel);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 3, 4, 3);
            Name = "Overlay";
            ShowInTaskbar = false;
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.Manual;
            Text = "UGC App - Overlay";
            TopMost = true;
            TransparencyKey = Color.DarkGoldenrod;
            MouseDown += OverlayForm_MouseDown;
            MouseMove += OverlayForm_MouseMove;
            MouseUp += OverlayForm_MouseUp;
            panel.ResumeLayout(false);
            panel.PerformLayout();
            ResumeLayout(false);
        }

        private Label label_SystemList;
        private Label label_SystemTitle;
        private Label label_TickTime;
        private Label label_TickTitle;
    }
}