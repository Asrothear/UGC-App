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
        toolStripSeparator1 = new ToolStripSeparator();
        toolStripMenuItem_Exit = new ToolStripMenuItem();
        anweisungenToolStripMenuItem = new ToolStripMenuItem();
        dashboardOrderToolStripMenuItem = new ToolStripMenuItem();
        dashboardSystemsToolStripMenuItem = new ToolStripMenuItem();
        hilfeToolStripMenuItem = new ToolStripMenuItem();
        toolStripMenuItem_help_update = new ToolStripMenuItem();
        toolStripSeparator2 = new ToolStripSeparator();
        toolStripTextBox1 = new ToolStripMenuItem();
        toolStripMenuItem_help_posreset = new ToolStripMenuItem();
        toolStripMenuItem_help_cachedel = new ToolStripMenuItem();
        toolStripMenuItem_help_prgstate = new ToolStripMenuItem();
        toolStripSeparator3 = new ToolStripSeparator();
        toolStripMenuItem_help_logSenden = new ToolStripMenuItem();
        toolStripSeparator4 = new ToolStripSeparator();
        toolStripMenuItem_help_buyMeACoffe = new ToolStripMenuItem();
        toolStripMenuItem_help_About = new ToolStripMenuItem();
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
        groupBox_Orders = new GroupBox();
        label_Orders = new Label();
        label_Suit = new Label();
        ((System.ComponentModel.ISupportInitialize)redLight).BeginInit();
        ((System.ComponentModel.ISupportInitialize)yellowLight).BeginInit();
        ((System.ComponentModel.ISupportInitialize)greenLight).BeginInit();
        menuStrip1.SuspendLayout();
        statusStrip_Main.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
        groupBox_Orders.SuspendLayout();
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
        menuStrip1.Items.AddRange(new ToolStripItem[] { toolStripMenuItem_Menu, anweisungenToolStripMenuItem, hilfeToolStripMenuItem });
        menuStrip1.Location = new Point(0, 0);
        menuStrip1.Name = "menuStrip1";
        menuStrip1.RenderMode = ToolStripRenderMode.Professional;
        menuStrip1.Size = new Size(287, 24);
        menuStrip1.TabIndex = 4;
        menuStrip1.Text = "menuStrip1";
        // 
        // toolStripMenuItem_Menu
        // 
        toolStripMenuItem_Menu.DropDownItems.AddRange(new ToolStripItem[] { toolStripMenuItem_Overlay, toolStripMenuItem_Settings, toolStripSeparator1, toolStripMenuItem_Exit });
        toolStripMenuItem_Menu.Name = "toolStripMenuItem_Menu";
        toolStripMenuItem_Menu.Size = new Size(50, 20);
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
        // anweisungenToolStripMenuItem
        // 
        anweisungenToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { dashboardOrderToolStripMenuItem, dashboardSystemsToolStripMenuItem });
        anweisungenToolStripMenuItem.Name = "anweisungenToolStripMenuItem";
        anweisungenToolStripMenuItem.Size = new Size(40, 20);
        anweisungenToolStripMenuItem.Text = "BGS";
        // 
        // dashboardOrderToolStripMenuItem
        // 
        dashboardOrderToolStripMenuItem.Name = "dashboardOrderToolStripMenuItem";
        dashboardOrderToolStripMenuItem.Size = new Size(180, 22);
        dashboardOrderToolStripMenuItem.Text = "Anweisungen";
        // 
        // dashboardSystemsToolStripMenuItem
        // 
        dashboardSystemsToolStripMenuItem.Name = "dashboardSystemsToolStripMenuItem";
        dashboardSystemsToolStripMenuItem.Size = new Size(180, 22);
        dashboardSystemsToolStripMenuItem.Text = "System Liste";
        // 
        // hilfeToolStripMenuItem
        // 
        hilfeToolStripMenuItem.Alignment = ToolStripItemAlignment.Right;
        hilfeToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { toolStripMenuItem_help_update, toolStripSeparator2, toolStripTextBox1, toolStripMenuItem_help_posreset, toolStripMenuItem_help_cachedel, toolStripMenuItem_help_prgstate, toolStripSeparator3, toolStripMenuItem_help_logSenden, toolStripSeparator4, toolStripMenuItem_help_buyMeACoffe, toolStripMenuItem_help_About });
        hilfeToolStripMenuItem.Name = "hilfeToolStripMenuItem";
        hilfeToolStripMenuItem.Size = new Size(44, 20);
        hilfeToolStripMenuItem.Text = "Hilfe";
        // 
        // toolStripMenuItem_help_update
        // 
        toolStripMenuItem_help_update.Name = "toolStripMenuItem_help_update";
        toolStripMenuItem_help_update.Size = new Size(180, 22);
        toolStripMenuItem_help_update.Text = "auf Updates Prüfen";
        // 
        // toolStripSeparator2
        // 
        toolStripSeparator2.Name = "toolStripSeparator2";
        toolStripSeparator2.Size = new Size(177, 6);
        // 
        // toolStripTextBox1
        // 
        toolStripTextBox1.Enabled = false;
        toolStripTextBox1.Name = "toolStripTextBox1";
        toolStripTextBox1.Size = new Size(180, 22);
        toolStripTextBox1.Text = "- Debug -";
        // 
        // toolStripMenuItem_help_posreset
        // 
        toolStripMenuItem_help_posreset.Name = "toolStripMenuItem_help_posreset";
        toolStripMenuItem_help_posreset.Size = new Size(180, 22);
        toolStripMenuItem_help_posreset.Text = "Position Reset";
        // 
        // toolStripMenuItem_help_cachedel
        // 
        toolStripMenuItem_help_cachedel.Name = "toolStripMenuItem_help_cachedel";
        toolStripMenuItem_help_cachedel.Size = new Size(180, 22);
        toolStripMenuItem_help_cachedel.Text = "Cache löschen";
        // 
        // toolStripMenuItem_help_prgstate
        // 
        toolStripMenuItem_help_prgstate.Name = "toolStripMenuItem_help_prgstate";
        toolStripMenuItem_help_prgstate.Size = new Size(180, 22);
        toolStripMenuItem_help_prgstate.Text = "Program State";
        // 
        // toolStripSeparator3
        // 
        toolStripSeparator3.Name = "toolStripSeparator3";
        toolStripSeparator3.Size = new Size(177, 6);
        // 
        // toolStripMenuItem_help_logSenden
        // 
        toolStripMenuItem_help_logSenden.Name = "toolStripMenuItem_help_logSenden";
        toolStripMenuItem_help_logSenden.Size = new Size(180, 22);
        toolStripMenuItem_help_logSenden.Text = "Log Senden";
        // 
        // toolStripSeparator4
        // 
        toolStripSeparator4.Name = "toolStripSeparator4";
        toolStripSeparator4.Size = new Size(177, 6);
        // 
        // toolStripMenuItem_help_buyMeACoffe
        // 
        toolStripMenuItem_help_buyMeACoffe.Name = "toolStripMenuItem_help_buyMeACoffe";
        toolStripMenuItem_help_buyMeACoffe.Size = new Size(180, 22);
        toolStripMenuItem_help_buyMeACoffe.Text = "Buy me a coffe";
        // 
        // toolStripMenuItem_help_About
        // 
        toolStripMenuItem_help_About.Name = "toolStripMenuItem_help_About";
        toolStripMenuItem_help_About.Size = new Size(180, 22);
        toolStripMenuItem_help_About.Text = "About";
        // 
        // statusStrip_Main
        // 
        statusStrip_Main.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel_Status, toolStripStatusLabel_Spacer, toolStripStatusLabel_Version });
        statusStrip_Main.Location = new Point(0, 386);
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
        toolStripStatusLabel_Spacer.Size = new Size(70, 17);
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
        label_CMDr.Location = new Point(12, 40);
        label_CMDr.Name = "label_CMDr";
        label_CMDr.Size = new Size(41, 15);
        label_CMDr.TabIndex = 6;
        label_CMDr.Text = "CMDr:";
        // 
        // label_System
        // 
        label_System.AutoSize = true;
        label_System.Location = new Point(12, 84);
        label_System.Name = "label_System";
        label_System.Size = new Size(48, 15);
        label_System.TabIndex = 7;
        label_System.Text = "System:";
        label_System.TextAlign = ContentAlignment.TopCenter;
        // 
        // label_Docked
        // 
        label_Docked.AutoSize = true;
        label_Docked.Location = new Point(12, 107);
        label_Docked.Name = "label_Docked";
        label_Docked.Size = new Size(68, 15);
        label_Docked.TabIndex = 8;
        label_Docked.Text = "Angedockt:";
        // 
        // label_TickTitle
        // 
        label_TickTitle.AutoSize = true;
        label_TickTitle.Location = new Point(12, 138);
        label_TickTitle.Name = "label_TickTitle";
        label_TickTitle.Size = new Size(31, 15);
        label_TickTitle.TabIndex = 9;
        label_TickTitle.Text = "Tick:";
        // 
        // label_SystemListLabel
        // 
        label_SystemListLabel.AutoSize = true;
        label_SystemListLabel.Location = new Point(12, 282);
        label_SystemListLabel.Name = "label_SystemListLabel";
        label_SystemListLabel.Size = new Size(54, 15);
        label_SystemListLabel.TabIndex = 11;
        label_SystemListLabel.Text = "Systeme:";
        // 
        // label_SystemList
        // 
        label_SystemList.AutoSize = true;
        label_SystemList.Location = new Point(61, 312);
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
        pictureBox1.Location = new Point(199, 27);
        pictureBox1.Name = "pictureBox1";
        pictureBox1.Size = new Size(88, 50);
        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
        pictureBox1.TabIndex = 16;
        pictureBox1.TabStop = false;
        // 
        // label_Tick
        // 
        label_Tick.Location = new Point(58, 138);
        label_Tick.Name = "label_Tick";
        label_Tick.Size = new Size(183, 45);
        label_Tick.TabIndex = 17;
        label_Tick.Text = "-Warte auf Daten-";
        label_Tick.TextAlign = ContentAlignment.TopCenter;
        // 
        // groupBox_Orders
        // 
        groupBox_Orders.BackColor = Color.Transparent;
        groupBox_Orders.Controls.Add(label_Orders);
        groupBox_Orders.Location = new Point(12, 179);
        groupBox_Orders.Name = "groupBox_Orders";
        groupBox_Orders.Size = new Size(263, 100);
        groupBox_Orders.TabIndex = 18;
        groupBox_Orders.TabStop = false;
        groupBox_Orders.Text = "BGS Order: TestSystem";
        groupBox_Orders.Visible = false;
        // 
        // label_Orders
        // 
        label_Orders.AutoSize = true;
        label_Orders.Location = new Point(6, 19);
        label_Orders.MaximumSize = new Size(245, 0);
        label_Orders.MinimumSize = new Size(245, 0);
        label_Orders.Name = "label_Orders";
        label_Orders.Size = new Size(245, 15);
        label_Orders.TabIndex = 1;
        // 
        // label_Suit
        // 
        label_Suit.AutoSize = true;
        label_Suit.Location = new Point(12, 61);
        label_Suit.Name = "label_Suit";
        label_Suit.Size = new Size(44, 15);
        label_Suit.TabIndex = 19;
        label_Suit.Text = "Anzug:";
        // 
        // Mainframe
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(287, 408);
        Controls.Add(label_Suit);
        Controls.Add(groupBox_Orders);
        Controls.Add(label_Docked);
        Controls.Add(label_SystemListLabel);
        Controls.Add(label_TickTitle);
        Controls.Add(label_Tick);
        Controls.Add(label_SystemList);
        Controls.Add(label_System);
        Controls.Add(label_CMDr);
        Controls.Add(statusStrip_Main);
        Controls.Add(menuStrip1);
        Controls.Add(greenLight);
        Controls.Add(yellowLight);
        Controls.Add(redLight);
        Controls.Add(pictureBox1);
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
        groupBox_Orders.ResumeLayout(false);
        groupBox_Orders.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }
    #endregion
    private MenuStrip menuStrip1;
    private ToolStripMenuItem toolStripMenuItem_Menu;
    private ToolStripMenuItem toolStripMenuItem_Settings;
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
    private ToolStripMenuItem anweisungenToolStripMenuItem;
    private ToolStripMenuItem dashboardOrderToolStripMenuItem;
    private GroupBox groupBox_Orders;
    private Label label_Orders;
    private ToolStripMenuItem dashboardSystemsToolStripMenuItem;
    private Label label_Suit;
    private ToolStripMenuItem hilfeToolStripMenuItem;
    private ToolStripMenuItem toolStripMenuItem_help_update;
    private ToolStripMenuItem toolStripMenuItem_help_posreset;
    private ToolStripMenuItem toolStripMenuItem_help_cachedel;
    private ToolStripSeparator toolStripSeparator2;
    private ToolStripMenuItem toolStripMenuItem_help_prgstate;
    private ToolStripSeparator toolStripSeparator3;
    private ToolStripMenuItem toolStripMenuItem_help_logSenden;
    private ToolStripMenuItem toolStripMenuItem_help_buyMeACoffe;
    private ToolStripTextBox toolStripTextBox2;
    private ToolStripMenuItem toolStripTextBox1;
    private ToolStripSeparator toolStripSeparator4;
    private ToolStripMenuItem toolStripMenuItem_help_About;
}