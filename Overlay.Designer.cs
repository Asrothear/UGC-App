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
            groupBox_Orders = new GroupBox();
            label_Orders = new Label();
            greenLight = new PictureBox();
            label_SystemTitle = new Label();
            yellowLight = new PictureBox();
            redLight = new PictureBox();
            label_TickTime = new Label();
            label_TickTitle = new Label();
            label_SystemList = new Label();
            _mouseTimer = new System.Windows.Forms.Timer(components);
            panel.SuspendLayout();
            groupBox_Orders.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)greenLight).BeginInit();
            ((System.ComponentModel.ISupportInitialize)yellowLight).BeginInit();
            ((System.ComponentModel.ISupportInitialize)redLight).BeginInit();
            SuspendLayout();
            // 
            // panel
            // 
            panel.BackColor = Color.Transparent;
            panel.Controls.Add(groupBox_Orders);
            panel.Controls.Add(greenLight);
            panel.Controls.Add(label_SystemTitle);
            panel.Controls.Add(yellowLight);
            panel.Controls.Add(redLight);
            panel.Controls.Add(label_TickTime);
            panel.Controls.Add(label_TickTitle);
            panel.Controls.Add(label_SystemList);
            panel.Dock = DockStyle.Fill;
            panel.Location = new Point(0, 0);
            panel.Name = "panel";
            panel.Size = new Size(260, 248);
            panel.TabIndex = 0;
            panel.MouseDown += OverlayForm_MouseDown;
            panel.MouseMove += OverlayForm_MouseMove;
            panel.MouseUp += OverlayForm_MouseUp;
            // 
            // groupBox_Orders
            // 
            groupBox_Orders.Controls.Add(label_Orders);
            groupBox_Orders.Location = new Point(10, 66);
            groupBox_Orders.Name = "groupBox_Orders";
            groupBox_Orders.Size = new Size(238, 100);
            groupBox_Orders.TabIndex = 19;
            groupBox_Orders.TabStop = false;
            groupBox_Orders.Text = "BGS Order: TestSystem";
            groupBox_Orders.Visible = false;
            // 
            // label_Orders
            // 
            label_Orders.AutoSize = true;
            label_Orders.Dock = DockStyle.Fill;
            label_Orders.Location = new Point(3, 19);
            label_Orders.MaximumSize = new Size(230, 0);
            label_Orders.MinimumSize = new Size(230, 0);
            label_Orders.Name = "label_Orders";
            label_Orders.Size = new Size(230, 15);
            label_Orders.TabIndex = 1;
            // 
            // greenLight
            // 
            greenLight.BackColor = Color.Gray;
            greenLight.Location = new Point(238, 39);
            greenLight.Name = "greenLight";
            greenLight.Size = new Size(10, 10);
            greenLight.TabIndex = 6;
            greenLight.TabStop = false;
            greenLight.Tag = Color.Green;
            // 
            // label_SystemTitle
            // 
            label_SystemTitle.AutoSize = true;
            label_SystemTitle.Location = new Point(103, 169);
            label_SystemTitle.Name = "label_SystemTitle";
            label_SystemTitle.Size = new Size(54, 15);
            label_SystemTitle.TabIndex = 4;
            label_SystemTitle.Text = "Systeme:";
            // 
            // yellowLight
            // 
            yellowLight.BackColor = Color.Gray;
            yellowLight.Location = new Point(238, 23);
            yellowLight.Name = "yellowLight";
            yellowLight.Size = new Size(10, 10);
            yellowLight.TabIndex = 5;
            yellowLight.TabStop = false;
            yellowLight.Tag = Color.Yellow;
            // 
            // redLight
            // 
            redLight.BackColor = Color.Gray;
            redLight.Location = new Point(238, 7);
            redLight.Name = "redLight";
            redLight.Size = new Size(10, 10);
            redLight.TabIndex = 4;
            redLight.TabStop = false;
            redLight.Tag = Color.Red;
            // 
            // label_TickTime
            // 
            label_TickTime.AutoSize = true;
            label_TickTime.Location = new Point(82, 34);
            label_TickTime.Name = "label_TickTime";
            label_TickTime.Size = new Size(86, 15);
            label_TickTime.TabIndex = 3;
            label_TickTime.Text = "Loading Data...";
            label_TickTime.TextAlign = ContentAlignment.TopCenter;
            // 
            // label_TickTitle
            // 
            label_TickTitle.AutoSize = true;
            label_TickTitle.Location = new Point(103, 18);
            label_TickTitle.Name = "label_TickTitle";
            label_TickTitle.Size = new Size(31, 15);
            label_TickTitle.TabIndex = 2;
            label_TickTitle.Text = "Tick:";
            // 
            // label_SystemList
            // 
            label_SystemList.AutoSize = true;
            label_SystemList.ForeColor = Color.White;
            label_SystemList.Location = new Point(25, 194);
            label_SystemList.MaximumSize = new Size(215, 0);
            label_SystemList.Name = "label_SystemList";
            label_SystemList.Size = new Size(212, 45);
            label_SystemList.TabIndex = 1;
            label_SystemList.Text = "label1 abcd   sdf  sdf  df efghijklmnopq\nlabel2\nlabel3";
            label_SystemList.TextAlign = ContentAlignment.TopCenter;
            // 
            // _mouseTimer
            // 
            _mouseTimer.Interval = 200;
            _mouseTimer.Tick += MouseTimer_Tick;
            // 
            // Overlay
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BackColor = Color.DarkGoldenrod;
            ClientSize = new Size(260, 248);
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
            groupBox_Orders.ResumeLayout(false);
            groupBox_Orders.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)greenLight).EndInit();
            ((System.ComponentModel.ISupportInitialize)yellowLight).EndInit();
            ((System.ComponentModel.ISupportInitialize)redLight).EndInit();
            ResumeLayout(false);
        }

        private Label label_SystemList;
        private Label label_SystemTitle;
        private Label label_TickTime;
        private Label label_TickTitle;
        internal PictureBox greenLight;
        internal PictureBox yellowLight;
        internal PictureBox redLight;
        private GroupBox groupBox_Orders;
        private Label label_Orders;
    }
}