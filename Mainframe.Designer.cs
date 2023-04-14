using System.Drawing.Drawing2D;
using UGC_App.EDLog;

namespace UGC_App;

partial class Mainframe
{
    private NotifyIcon notifyIcon;
    private ContextMenuStrip contextMenuStrip;
    internal PictureBox redLight;
    internal PictureBox yellowLight;
    internal PictureBox greenLight;
    private Overlay overlayForm;
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Mainframe));
        notifyIcon = new NotifyIcon(components);
        contextMenuStrip = new ContextMenuStrip(components);
        redLight = new PictureBox();
        yellowLight = new PictureBox();
        greenLight = new PictureBox();
        menuStrip1 = new MenuStrip();
        toolStripMenuItem_Menu = new ToolStripMenuItem();
        toolStripMenuItem_Overlay = new ToolStripMenuItem();
        toolStripMenuItem_Settings = new ToolStripMenuItem();
        toolStripMenuItem_About = new ToolStripMenuItem();
        toolStripSeparator1 = new ToolStripSeparator();
        toolStripMenuItem_Exit = new ToolStripMenuItem();
        statusStrip_Main = new StatusStrip();
        toolStripStatusLabel_Status = new ToolStripStatusLabel();
        toolStripStatusLabel_Spacer = new ToolStripStatusLabel();
        toolStripStatusLabel_Version = new ToolStripStatusLabel();
        label_CMDr = new Label();
        label_System = new Label();
        label_Docked = new Label();
        label_TickTitle = new Label();
        label_SystemListLabel = new Label();
        label_SystemList = new Label();
        pictureBox1 = new PictureBox();
        label_Tick = new Label();
        ((System.ComponentModel.ISupportInitialize)redLight).BeginInit();
        ((System.ComponentModel.ISupportInitialize)yellowLight).BeginInit();
        ((System.ComponentModel.ISupportInitialize)greenLight).BeginInit();
        menuStrip1.SuspendLayout();
        statusStrip_Main.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
        SuspendLayout();
        // 
        // notifyIcon
        // 
        notifyIcon.BalloonTipText = "UGC App";
        notifyIcon.Icon = (Icon)resources.GetObject("notifyIcon.Icon");
        notifyIcon.Text = "UGC App";
        notifyIcon.Visible = true;
        // 
        // contextMenuStrip
        // 
        contextMenuStrip.Name = "contextMenuStrip";
        contextMenuStrip.Size = new Size(61, 4);
        // 
        // redLight
        // 
        redLight.BackColor = Color.Gray;
        redLight.Location = new Point(265, 87);
        redLight.Name = "redLight";
        redLight.Size = new Size(10, 10);
        redLight.TabIndex = 1;
        redLight.TabStop = false;
        redLight.Tag = Color.Red;
        // 
        // yellowLight
        // 
        yellowLight.BackColor = Color.Gray;
        yellowLight.Location = new Point(265, 103);
        yellowLight.Name = "yellowLight";
        yellowLight.Size = new Size(10, 10);
        yellowLight.TabIndex = 2;
        yellowLight.TabStop = false;
        yellowLight.Tag = Color.Yellow;
        // 
        // greenLight
        // 
        greenLight.BackColor = Color.Gray;
        greenLight.Location = new Point(265, 119);
        greenLight.Name = "greenLight";
        greenLight.Size = new Size(10, 10);
        greenLight.TabIndex = 3;
        greenLight.TabStop = false;
        greenLight.Tag = Color.Green;
        // 
        // menuStrip1
        // 
        menuStrip1.BackColor = Color.LightGray;
        menuStrip1.Items.AddRange(new ToolStripItem[] { toolStripMenuItem_Menu });
        menuStrip1.Location = new Point(0, 0);
        menuStrip1.Name = "menuStrip1";
        menuStrip1.RenderMode = ToolStripRenderMode.Professional;
        menuStrip1.Size = new Size(287, 24);
        menuStrip1.TabIndex = 4;
        menuStrip1.Text = "menuStrip1";
        // 
        // toolStripMenuItem_Menu
        // 
        toolStripMenuItem_Menu.DropDownItems.AddRange(new ToolStripItem[] { toolStripMenuItem_Overlay, toolStripMenuItem_Settings, toolStripMenuItem_About, toolStripSeparator1, toolStripMenuItem_Exit });
        toolStripMenuItem_Menu.Name = "toolStripMenuItem_Menu";
        toolStripMenuItem_Menu.Size = new Size(50, 23);
        toolStripMenuItem_Menu.Text = "Menu";
        // 
        // toolStripMenuItem_Overlay
        // 
        toolStripMenuItem_Overlay.Name = "toolStripMenuItem_Overlay";
        toolStripMenuItem_Overlay.Size = new Size(152, 22);
        toolStripMenuItem_Overlay.Text = "Toggle Overlay";
        // 
        // toolStripMenuItem_Settings
        // 
        toolStripMenuItem_Settings.Name = "toolStripMenuItem_Settings";
        toolStripMenuItem_Settings.Size = new Size(152, 22);
        toolStripMenuItem_Settings.Text = "Einstellungen";
        // 
        // toolStripMenuItem_About
        // 
        toolStripMenuItem_About.Name = "toolStripMenuItem_About";
        toolStripMenuItem_About.Size = new Size(152, 22);
        toolStripMenuItem_About.Text = "Über";
        // 
        // toolStripSeparator1
        // 
        toolStripSeparator1.Name = "toolStripSeparator1";
        toolStripSeparator1.Size = new Size(149, 6);
        // 
        // toolStripMenuItem_Exit
        // 
        toolStripMenuItem_Exit.Name = "toolStripMenuItem_Exit";
        toolStripMenuItem_Exit.Size = new Size(152, 22);
        toolStripMenuItem_Exit.Text = "Beenden";
        toolStripMenuItem_Exit.Click += ExitMenuItem_Click;
        // 
        // statusStrip_Main
        // 
        statusStrip_Main.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel_Status, toolStripStatusLabel_Spacer, toolStripStatusLabel_Version });
        statusStrip_Main.Location = new Point(0, 237);
        statusStrip_Main.Name = "statusStrip_Main";
        statusStrip_Main.Size = new Size(287, 22);
        statusStrip_Main.SizingGrip = false;
        statusStrip_Main.TabIndex = 5;
        statusStrip_Main.Text = "statusStrip1";
        // 
        // toolStripStatusLabel_Status
        // 
        toolStripStatusLabel_Status.AutoSize = false;
        toolStripStatusLabel_Status.Name = "toolStripStatusLabel_Status";
        toolStripStatusLabel_Status.Size = new Size(130, 17);
        toolStripStatusLabel_Status.Text = "Status:";
        toolStripStatusLabel_Status.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // toolStripStatusLabel_Spacer
        // 
        toolStripStatusLabel_Spacer.Name = "toolStripStatusLabel_Spacer";
        toolStripStatusLabel_Spacer.Size = new Size(1, 17);
        toolStripStatusLabel_Spacer.Spring = true;
        // 
        // toolStripStatusLabel_Version
        // 
        toolStripStatusLabel_Version.Name = "toolStripStatusLabel_Version";
        toolStripStatusLabel_Version.Size = new Size(72, 17);
        toolStripStatusLabel_Version.Text = "Version 0.0.1";
        toolStripStatusLabel_Version.TextAlign = ContentAlignment.MiddleRight;
        // 
        // label_CMDr
        // 
        label_CMDr.AutoSize = true;
        label_CMDr.Location = new Point(12, 42);
        label_CMDr.Name = "label_CMDr";
        label_CMDr.Size = new Size(41, 15);
        label_CMDr.TabIndex = 6;
        label_CMDr.Text = "CMDr:";
        // 
        // label_System
        // 
        label_System.AutoSize = true;
        label_System.Location = new Point(12, 72);
        label_System.Name = "label_System";
        label_System.Size = new Size(48, 15);
        label_System.TabIndex = 7;
        label_System.Text = "System:";
        label_System.TextAlign = ContentAlignment.TopCenter;
        // 
        // label_Docked
        // 
        label_Docked.AutoSize = true;
        label_Docked.Location = new Point(12, 103);
        label_Docked.Name = "label_Docked";
        label_Docked.Size = new Size(68, 15);
        label_Docked.TabIndex = 8;
        label_Docked.Text = "Angedockt:";
        // 
        // label_TickTitle
        // 
        label_TickTitle.AutoSize = true;
        label_TickTitle.Location = new Point(12, 133);
        label_TickTitle.Name = "label_TickTitle";
        label_TickTitle.Size = new Size(31, 15);
        label_TickTitle.TabIndex = 9;
        label_TickTitle.Text = "Tick:";
        // 
        // label_SystemListLabel
        // 
        label_SystemListLabel.AutoSize = true;
        label_SystemListLabel.Location = new Point(12, 163);
        label_SystemListLabel.Name = "label_SystemListLabel";
        label_SystemListLabel.Size = new Size(54, 15);
        label_SystemListLabel.TabIndex = 11;
        label_SystemListLabel.Text = "Systeme:";
        // 
        // label_SystemList
        // 
        label_SystemList.AutoSize = true;
        label_SystemList.Location = new Point(67, 193);
        label_SystemList.MaximumSize = new Size(213, 0);
        label_SystemList.Name = "label_SystemList";
        label_SystemList.Size = new Size(49, 15);
        label_SystemList.TabIndex = 12;
        label_SystemList.Text = "Start Up";
        label_SystemList.TextAlign = ContentAlignment.TopCenter;
        // 
        // pictureBox1
        // 
        pictureBox1.BackColor = Color.Transparent;
        pictureBox1.Image = Properties.Resources.UGC_Logo;
        pictureBox1.Location = new Point(187, 27);
        pictureBox1.Name = "pictureBox1";
        pictureBox1.Size = new Size(88, 50);
        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
        pictureBox1.TabIndex = 16;
        pictureBox1.TabStop = false;
        // 
        // label_Tick
        // 
        label_Tick.Location = new Point(58, 133);
        label_Tick.Name = "label_Tick";
        label_Tick.Size = new Size(183, 45);
        label_Tick.TabIndex = 17;
        label_Tick.Text = "12.04.2023 21:21:05\n(~13.04.2023 00:21:05(+3h)~)";
        label_Tick.TextAlign = ContentAlignment.TopCenter;
        // 
        // Mainframe
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(287, 259);
        Controls.Add(label_TickTitle);
        Controls.Add(label_Tick);
        Controls.Add(pictureBox1);
        Controls.Add(label_SystemList);
        Controls.Add(label_SystemListLabel);
        Controls.Add(label_Docked);
        Controls.Add(label_System);
        Controls.Add(label_CMDr);
        Controls.Add(statusStrip_Main);
        Controls.Add(menuStrip1);
        Controls.Add(greenLight);
        Controls.Add(yellowLight);
        Controls.Add(redLight);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        Icon = (Icon)resources.GetObject("$this.Icon");
        MainMenuStrip = menuStrip1;
        MaximizeBox = false;
        MdiChildrenMinimizedAnchorBottom = false;
        Name = "Mainframe";
        SizeGripStyle = SizeGripStyle.Hide;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "UGC App";
        FormClosing += Form1_FormClosing;
        ((System.ComponentModel.ISupportInitialize)redLight).EndInit();
        ((System.ComponentModel.ISupportInitialize)yellowLight).EndInit();
        ((System.ComponentModel.ISupportInitialize)greenLight).EndInit();
        menuStrip1.ResumeLayout(false);
        menuStrip1.PerformLayout();
        statusStrip_Main.ResumeLayout(false);
        statusStrip_Main.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
    #endregion
    private MenuStrip menuStrip1;
    private ToolStripMenuItem toolStripMenuItem_Menu;
    private ToolStripMenuItem toolStripMenuItem_Settings;
    private ToolStripMenuItem toolStripMenuItem_About;
    private ToolStripSeparator toolStripSeparator1;
    private ToolStripMenuItem toolStripMenuItem_Exit;
    private StatusStrip statusStrip_Main;
    private ToolStripStatusLabel toolStripStatusLabel_Status;
    private ToolStripStatusLabel toolStripStatusLabel_Version;
    private Label label_CMDr;
    private Label label_System;
    private Label label_Docked;
    private Label label_TickTitle;
    private Label label_SystemListLabel;
    private Label label_SystemList;
    private PictureBox pictureBox1;
    private ToolStripMenuItem toolStripMenuItem_Overlay;
    private ToolStripStatusLabel toolStripStatusLabel_Spacer;
    private Label label_Tick;
}