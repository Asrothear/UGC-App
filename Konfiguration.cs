using UGC_App.EDLog;
using UGC_App.ErrorReporter;

namespace UGC_App;

public partial class Konfiguration : Form
{
    private readonly Mainframe _parent;
    public Konfiguration(Mainframe parrent)
    {
        _parent = parrent;
        InitializeComponent();
        button_Report_Senden.Click += (_, _) => SendReport();
        tabControl1.SelectedIndex = 0;
        button_Save.Left = (ClientSize.Width - button_Save.Width) / 2;
        button_Save.Top = ClientSize.Height - 30;
        TopMost = Config.Instance.AlwaysOnTop;
        Load += (_, _) => { LoadKonfigs(); };

        button_Save.Click += (_, _) =>
        {
            SaveChanges();
            _parent.RefreshListOnKonfigChange();
            Close();
        };
        //button_Design.Click += (sender, args) => { parent.ShowDesign(); };
        checkBox_FullList.CheckStateChanged += (_, _) =>
        {
            if (checkBox_FullList.Checked)
            {
                checkBox_FullList.Text = "Gesamte Liste zeigen";
                numericUpDown_ListCount.Visible = false;
            }
            else
            {
                checkBox_FullList.Text = "Zeige nur x Systeme    x=";
                numericUpDown_ListCount.Visible = true;
            }
        };
        if (!Config.Instance.ShowAll)
        {
            checkBox_FullList.Text = "Zeige nur x Systeme    x=";
            numericUpDown_ListCount.Visible = true;
        }

        SetDesign(Config.Instance.DesignSel);
        toolTip_Konfig.SetToolTip(label_SendUrl, "Web-Adresse der API zum Senden der Eilte Events");
        toolTip_Konfig.SetToolTip(textBox_Send, "Web-Adresse der API zum Senden der Eilte Events");
        toolTip_Konfig.SetToolTip(label_StateUrl, "Web-Adresse der API zum empfangen von Daten");
        toolTip_Konfig.SetToolTip(textBox_State, "Web-Adresse der API zum empfangen von Daten");
        toolTip_Konfig.SetToolTip(checkBox_CMDr, "Wird benötigt um z.b. die Entfernung berechnen zu können.");
        toolTip_Konfig.SetToolTip(checkBox_SlowState,
            "Die Übertragungabstände zum Server werden erhöht um bandbreite zu sparen.");
        toolTip_Konfig.SetToolTip(checkBox_CloseMini,
            "Die App wird beim schließen im Hintergrund weiter ausgeführt.");
        toolTip_Konfig.SetToolTip(checkBox_Debug, "Erzeugt eine Datei mit Fehlermeldungen um diese zu beheben.");
        toolTip_Konfig.SetToolTip(checkBox_AlwaysTop, "Setzt das Hauptfenster immer in den Vordergrund.");
        toolTip_Konfig.SetToolTip(label_AutoKontrast, "Stellt die Zeitliche Verzögerung in Millisekunden, für den Wechsel zwischen Dunklem und Hellem Hintergrund, zur Anpassung des Overlaytextes ein.");
        toolTip_Konfig.SetToolTip(numericUpDown_AutoKontrast, "Stellt die Zeitliche Verzögerung in Millisekunden, für den Wechsel zwischen Dunklem und Hellem Hintergrund, zur Anpassung des Overlaytextes ein.");



        foreach (Control grp in tabPage2.Controls)
        {
            if (grp is not GroupBox) continue;
            foreach (Control control in grp.Controls)
            {
                if (control is PictureBox)
                {
                    control.Click += (_, _) => { SelectColor(control); };
                }
            }
        }

        foreach (Control text in tabPage3.Controls)
        {
            if (text is TextBox)
            {
                text.DoubleClick += (_, _) => { OpenFileSelect(text); };
            }
        }


        GetSetting();
        radioButton_Light.Click += (_, _) => { ChangeTheme(0); };
        radioButton_Dark.Click += (_, _) => { ChangeTheme(1); };
        radioButton_Custom.Click += (_, _) => { ChangeTheme(2); };
        _parent.SetDesign();
        checkBox_Override.Click += (_, _) =>
        {
            groupBox_Overlay.Enabled = checkBox_Override.Checked;
            label_Disclaimer.Visible = checkBox_Override.Checked;
            ColorPick_Overlay_Background.BackColor = checkBox_Override.Checked
                ? Config.Instance.ColorOverlayBackground
                : Config.Instance.ColorDefaultChroma;
            ColorPick_Overlay_Systeme_Light.BackColor = checkBox_Override.Checked
                ? Config.Instance.ColorOverlaySystemeLight
                : Config.Instance.ColorDefaultLabelLight;
            ColorPick_Overlay_Systeme_Dark.BackColor = checkBox_Override.Checked
                ? Config.Instance.ColorOverlaySystemeDark
                : Config.Instance.ColorDefaultLabelDark;
            ColorPick_Overlay_Tick_Light.BackColor = checkBox_Override.Checked
                ? Config.Instance.ColorOverlayTickLight
                : Config.Instance.ColorDefaultLabelLight;
            ColorPick_Overlay_Tick_Dark.BackColor = checkBox_Override.Checked
                ? Config.Instance.ColorOverlayTickDark
                : Config.Instance.ColorDefaultLabelDark;
            Config.Instance.ColorOverlayOverride = checkBox_Override.Checked;
            Config.Save();
            GetSetting();
            _parent.SetDesign();
        };
        button_ResetToLight.Click += (_, _) => { ResetColors(true); };
        button_ResetToDark.Click += (_, _) => { ResetColors(false); };
    }

    private void SendReport()
    {
        button_Report_Senden.Enabled = false;
        Task.Run(() =>
        {

            if (MailClient.Send(textBox_Support.Text))
            {
                Invoke(() =>
                {
                    textBox_Support.Enabled = false;
                    button_Report_Senden.Enabled = false;
                });
                MessageBox.Show(this, "Report gesendet!", "Reporter");
            }
            else
            {
                textBox_Support.Enabled = true;
                button_Report_Senden.Enabled = true;
                MessageBox.Show(this, "Es konnte kein Report gesendet werden!", "Reporter");
            }
        });

    }
    private void OpenFileSelect(Control text)
    {
        folderBrowserDialog1.InitialDirectory = text.Text;
        var res = folderBrowserDialog1.ShowDialog();
        if (res == DialogResult.OK) text.Text = folderBrowserDialog1.SelectedPath;
        if (Config.Instance.PathJournal != textBox_path_journal.Text) JournalHandler.SwitchToLatestFile(textBox_path_journal.Text, "Journal");
        Config.Instance.PathConfig = textBox_path_config.Text;
        Config.Instance.PathJournal = textBox_path_journal.Text;
        Config.Instance.PathLogs = textBox_path_logs.Text;
        Config.Save();
    }

    private void LoadKonfigs()
    {
        if (!textBox_Support.Enabled) textBox_Support.Text = "";
        textBox_Support.Enabled = true;
        button_Report_Senden.Enabled = true;
        tabControl1.SelectedIndex = 0;
        numericUpDown_AutoKontrast.Value = Config.Instance.CheckBackgroundIntervall;
        textBox_Send.Text = Config.Instance.SendUrl;
        textBox_State.Text = Config.Instance.StateUrl;
        textBox_Token.Text = Config.Instance.Token;
        checkBox_CMDr.Checked = Config.Instance.SendName;
        checkBox_FullList.Checked = Config.Instance.ShowAll;
        checkBox_OnlyBGS.Checked = Config.Instance.BgsOnly;
        checkBox_AutoUpdate.Checked = Config.Instance.AutoUpdate;
        checkBox_SlowState.Checked = Config.Instance.SlowState;
        checkBox_Debug.Checked = Config.Instance.Debug;
        checkBoxy_AutoStart.Checked = Config.Instance.AutoStart;
        numericUpDown_ListCount.Value = Config.Instance.ListCount;
        checkBox_CloseMini.Checked = Config.Instance.CloseMini;
        checkBox_AlwaysTop.Checked = Config.Instance.AlwaysOnTop;
        checkBox_EDDN.Checked = Config.Instance.Eddn;
        textBox_path_logs.Text = Config.Instance.PathLogs;
        textBox_path_config.Text = Config.Instance.PathConfig;
        textBox_path_journal.Text = Config.Instance.PathJournal;
        checkBox_EDMC.Checked = Config.Instance.AlertEDMC;
        checkBox_RemoteMode.Checked = Config.Instance.RemoteMode;
        Activate();
    }

    private void SaveChanges()
    {
        Config.Instance.SendUrl = textBox_Send.Text;
        Config.Instance.StateUrl = textBox_State.Text;
        Config.Instance.Token = textBox_Token.Text;
        Config.Instance.Debug = checkBox_Debug.Checked;
        Config.Instance.BgsOnly = checkBox_OnlyBGS.Checked;
        Config.Instance.SendName = checkBox_CMDr.Checked;
        Config.Instance.ShowAll = checkBox_FullList.Checked;
        Config.Instance.AutoUpdate = checkBox_AutoUpdate.Checked;
        Config.Instance.SlowState = checkBox_SlowState.Checked;
        Config.Instance.AutoStart = checkBoxy_AutoStart.Checked;
        Config.Instance.ListCount = numericUpDown_ListCount.Value;
        Config.Instance.CloseMini = checkBox_CloseMini.Checked;
        Config.Instance.AlwaysOnTop = checkBox_AlwaysTop.Checked;
        Config.Instance.AlertEDMC = checkBox_EDMC.Checked;
        Config.Instance.Eddn = checkBox_EDDN.Checked;
        Config.Instance.CheckBackgroundIntervall = numericUpDown_AutoKontrast.Value;
        Config.Instance.PathLogs = textBox_path_logs.Text;
        Config.Instance.PathConfig = textBox_path_config.Text;
        Config.Instance.PathJournal = textBox_path_journal.Text;
        Config.Instance.RemoteMode = checkBox_RemoteMode.Checked;
        Config.Save();
        Program.SetStartup(checkBoxy_AutoStart.Checked);
        _parent.TopMost = Config.Instance.AlwaysOnTop;
        TopMost = Config.Instance.AlwaysOnTop;
    }

    public void SetDesign(int p0)
    {
        switch (p0)
        {
            case 0:
                BackColor = Config.Instance.ColorDefaultBackgroundLight;
                foreach (Control control in Controls)
                {
                    switch (control)
                    {
                        case Label:
                        case CheckBox:
                            control.ForeColor = Config.Instance.ColorDefaultLabelLight;
                            break;
                        case TabControl:
                            {
                                control.BackColor = Config.Instance.ColorDefaultBackgroundLight;
                                foreach (Control lab in control.Controls)
                                {
                                    switch (lab)
                                    {
                                        case Label:
                                        case CheckBox:
                                            lab.ForeColor = Config.Instance.ColorDefaultLabelLight;
                                            break;
                                        case TabPage:
                                            {
                                                lab.BackColor = Config.Instance.ColorDefaultBackgroundLight;
                                                foreach (Control labs in lab.Controls)
                                                {
                                                    switch (labs)
                                                    {
                                                        case Label:
                                                        case CheckBox:
                                                        case RadioButton:
                                                            labs.ForeColor = Config.Instance.ColorDefaultLabelLight;
                                                            break;
                                                        case GroupBox:
                                                            {
                                                                labs.ForeColor = Config.Instance.ColorDefaultLabelLight;
                                                                foreach (Control sub in labs.Controls)
                                                                {
                                                                    switch (sub)
                                                                    {
                                                                        case Label:
                                                                        case CheckBox:
                                                                        case RadioButton:
                                                                        case GroupBox:
                                                                        case Button:
                                                                            sub.ForeColor = Config.Instance.ColorDefaultLabelLight;
                                                                            break;
                                                                    }
                                                                }

                                                                break;
                                                            }
                                                    }
                                                }

                                                break;
                                            }
                                    }
                                }

                                break;
                            }
                    }
                }

                break;
            case 1:
                BackColor = Config.Instance.ColorDefaultBackgroundDark;
                foreach (Control control in Controls)
                {
                    switch (control)
                    {
                        case Label:
                        case CheckBox:
                            control.ForeColor = Config.Instance.ColorDefaultLabelDark;
                            break;
                        case TabControl:
                            {
                                control.BackColor = Config.Instance.ColorDefaultBackgroundDark;
                                foreach (Control lab in control.Controls)
                                {
                                    switch (lab)
                                    {
                                        case Label:
                                        case CheckBox:
                                            lab.ForeColor = Config.Instance.ColorDefaultLabelDark;
                                            break;
                                        case TabPage:
                                            {
                                                lab.BackColor = Config.Instance.ColorDefaultBackgroundDark;
                                                foreach (Control labs in lab.Controls)
                                                {
                                                    switch (labs)
                                                    {
                                                        case Label:
                                                        case CheckBox:
                                                        case RadioButton:
                                                            labs.ForeColor = Config.Instance.ColorDefaultLabelDark;
                                                            break;
                                                        case GroupBox:
                                                            {
                                                                labs.ForeColor = Config.Instance.ColorDefaultLabelDark;
                                                                foreach (Control sub in labs.Controls)
                                                                {
                                                                    switch (sub)
                                                                    {
                                                                        case Label:
                                                                        case CheckBox:
                                                                        case RadioButton:
                                                                        case GroupBox:
                                                                            sub.ForeColor = Config.Instance.ColorDefaultLabelDark;
                                                                            break;
                                                                        case Button:
                                                                            sub.ForeColor = Config.Instance.ColorDefaultLabelLight;
                                                                            break;
                                                                    }
                                                                }

                                                                break;
                                                            }
                                                    }
                                                }

                                                break;
                                            }
                                    }
                                }

                                break;
                            }
                    }
                }

                break;
            case 2:
                BackColor = Config.Instance.ColorMainBackground;
                foreach (Control control in Controls)
                {
                    switch (control)
                    {
                        case Label:
                        case CheckBox:
                            control.ForeColor = Config.Instance.ColorMainInfo;
                            break;
                        case TabControl:
                            {
                                control.BackColor = Config.Instance.ColorMainBackground;
                                foreach (Control lab in control.Controls)
                                {
                                    switch (lab)
                                    {
                                        case Label:
                                        case CheckBox:
                                            lab.ForeColor = Config.Instance.ColorMainInfo;
                                            break;
                                        case TabPage:
                                            {
                                                lab.BackColor = Config.Instance.ColorMainBackground;
                                                foreach (Control labs in lab.Controls)
                                                {
                                                    switch (labs)
                                                    {
                                                        case Label:
                                                        case CheckBox:
                                                        case RadioButton:
                                                            labs.ForeColor = Config.Instance.ColorMainInfo;
                                                            break;
                                                        case GroupBox:
                                                            {
                                                                labs.ForeColor = Config.Instance.ColorMainInfo;
                                                                foreach (Control sub in labs.Controls)
                                                                {
                                                                    switch (sub)
                                                                    {
                                                                        case Label:
                                                                        case CheckBox:
                                                                        case RadioButton:
                                                                        case GroupBox:
                                                                            sub.ForeColor = Config.Instance.ColorMainInfo;
                                                                            break;
                                                                        case Button:
                                                                            sub.ForeColor = Config.Instance.ColorDefaultLabelLight;
                                                                            break;
                                                                    }
                                                                }

                                                                break;
                                                            }
                                                    }
                                                }

                                                break;
                                            }
                                    }
                                }

                                break;
                            }
                    }
                }

                break;
        }
    }

    private void ResetColors(bool p0)
    {
        ColorPick_MainFrame_Background.BackColor = p0
            ? Config.Instance.ColorDefaultBackgroundLight
            : Config.Instance.ColorDefaultBackgroundDark;
        ColorPick_MainFrame_Infos.BackColor = p0
            ? Config.Instance.ColorDefaultLabelLight
            : Config.Instance.ColorDefaultLabelDark;
        ColorPick_MainFrame_Tick.BackColor = p0
            ? Config.Instance.ColorDefaultLabelLight
            : Config.Instance.ColorDefaultLabelDark;
        ColorPick_MainFrame_Systeme.BackColor = p0
            ? Config.Instance.ColorDefaultLabelLight
            : Config.Instance.ColorDefaultLabelDark;
        ColorPick_Overlay_Background.BackColor = Config.Instance.ColorDefaultChroma;
        ColorPick_Overlay_Systeme_Light.BackColor = Config.Instance.ColorDefaultLabelLight;
        ColorPick_Overlay_Systeme_Dark.BackColor = Config.Instance.ColorDefaultLabelDark;
        ColorPick_Overlay_Tick_Light.BackColor = Config.Instance.ColorDefaultLabelLight;
        ColorPick_Overlay_Tick_Dark.BackColor = Config.Instance.ColorDefaultLabelDark;
        Config.Instance.ColorMainBackground = ColorPick_MainFrame_Background.BackColor;
        Config.Instance.ColorMainInfo = ColorPick_MainFrame_Infos.BackColor;
        Config.Instance.ColorMainTick = ColorPick_MainFrame_Tick.BackColor;
        Config.Instance.ColorMainSysteme = ColorPick_MainFrame_Systeme.BackColor;
        if (checkBox_Override.Checked)
        {

            Config.Instance.ColorOverlayBackground = ColorPick_Overlay_Background.BackColor;
            Config.Instance.ColorOverlaySystemeLight = ColorPick_Overlay_Systeme_Light.BackColor;
            Config.Instance.ColorOverlaySystemeDark = ColorPick_Overlay_Systeme_Dark.BackColor;
            Config.Instance.ColorOverlayTickLight = ColorPick_Overlay_Tick_Light.BackColor;
            Config.Instance.ColorOverlayTickDark = ColorPick_Overlay_Tick_Dark.BackColor;
            Config.Instance.ColorOverlayOverride = checkBox_Override.Checked;
        }

        Config.Save();
        _parent.SetDesign();
    }

    private void ChangeTheme(int p0)
    {
        Config.Instance.DesignSel = p0;
        Config.Save();
        _parent.SetDesign();
        GetSetting();
    }

    private void GetSetting()
    {
        switch (Config.Instance.DesignSel)
        {
            case 0:
            case 1:
                radioButton_Light.Checked = true;
                radioButton_Dark.Checked = false;
                radioButton_Custom.Checked = false;
                if (Config.Instance.DesignSel == 1)
                {
                    radioButton_Light.Checked = false;
                    radioButton_Dark.Checked = true;
                }

                ColorPick_MainFrame_Background.BackColor = radioButton_Light.Checked
                    ? Config.Instance.ColorDefaultBackgroundLight
                    : Config.Instance.ColorDefaultBackgroundDark;
                ColorPick_MainFrame_Infos.BackColor = radioButton_Light.Checked
                    ? Config.Instance.ColorDefaultLabelLight
                    : Config.Instance.ColorDefaultLabelDark;
                ColorPick_MainFrame_Tick.BackColor = radioButton_Light.Checked
                    ? Config.Instance.ColorDefaultLabelLight
                    : Config.Instance.ColorDefaultLabelDark;
                ColorPick_MainFrame_Systeme.BackColor = radioButton_Light.Checked
                    ? Config.Instance.ColorDefaultLabelLight
                    : Config.Instance.ColorDefaultLabelDark;
                ColorPick_Overlay_Background.BackColor = Config.Instance.ColorDefaultChroma;
                ColorPick_Overlay_Systeme_Light.BackColor = Config.Instance.ColorDefaultLabelLight;
                ColorPick_Overlay_Systeme_Dark.BackColor = Config.Instance.ColorDefaultLabelDark;
                ColorPick_Overlay_Tick_Light.BackColor = Config.Instance.ColorDefaultLabelLight;
                ColorPick_Overlay_Tick_Dark.BackColor = Config.Instance.ColorDefaultLabelDark;
                break;
            case 2:
                radioButton_Light.Checked = false;
                radioButton_Dark.Checked = false;
                radioButton_Custom.Checked = true;
                ColorPick_MainFrame_Background.BackColor = Config.Instance.ColorMainBackground;
                ColorPick_MainFrame_Infos.BackColor = Config.Instance.ColorMainInfo;
                ColorPick_MainFrame_Tick.BackColor = Config.Instance.ColorMainTick;
                ColorPick_MainFrame_Systeme.BackColor = Config.Instance.ColorMainSysteme;
                ColorPick_Overlay_Background.BackColor = checkBox_Override.Checked
                    ? Config.Instance.ColorOverlayBackground
                    : Config.Instance.ColorDefaultChroma;
                ColorPick_Overlay_Systeme_Light.BackColor = checkBox_Override.Checked
                    ? Config.Instance.ColorOverlaySystemeLight
                    : Config.Instance.ColorDefaultLabelLight;
                ColorPick_Overlay_Systeme_Dark.BackColor = checkBox_Override.Checked
                    ? Config.Instance.ColorOverlaySystemeDark
                    : Config.Instance.ColorDefaultLabelDark;
                ColorPick_Overlay_Tick_Light.BackColor = checkBox_Override.Checked
                    ? Config.Instance.ColorOverlayTickLight
                    : Config.Instance.ColorDefaultLabelLight;
                ColorPick_Overlay_Tick_Dark.BackColor = checkBox_Override.Checked
                    ? Config.Instance.ColorOverlayTickDark
                    : Config.Instance.ColorDefaultLabelDark;
                break;
        }

        checkBox_Override.Checked = Config.Instance.ColorOverlayOverride;
        ColorPick_MainFrame_Background.Enabled = radioButton_Custom.Checked;
        ColorPick_MainFrame_Infos.Enabled = radioButton_Custom.Checked;
        ColorPick_MainFrame_Tick.Enabled = radioButton_Custom.Checked;
        ColorPick_MainFrame_Systeme.Enabled = radioButton_Custom.Checked;
        groupBox_Overlay.Enabled = checkBox_Override.Checked;
        label_Disclaimer.Visible = checkBox_Override.Checked;
        checkBox_Override.Visible = radioButton_Custom.Checked;
        button_ResetToLight.Visible = radioButton_Custom.Checked;
        button_ResetToDark.Visible = radioButton_Custom.Checked;
        if (radioButton_Custom.Checked) return;
        groupBox_Overlay.Enabled = radioButton_Custom.Checked;
        label_Disclaimer.Visible = radioButton_Custom.Checked;
        checkBox_Override.Visible = radioButton_Custom.Checked;
    }

    private void SelectColor(Control control)
    {
        colorDialog1.Color = control.BackColor;
        colorDialog1.ShowDialog(this);
        control.BackColor = colorDialog1.Color;
        Config.Instance.ColorMainBackground = ColorPick_MainFrame_Background.BackColor;
        Config.Instance.ColorMainInfo = ColorPick_MainFrame_Infos.BackColor;
        Config.Instance.ColorMainTick = ColorPick_MainFrame_Tick.BackColor;
        Config.Instance.ColorMainSysteme = ColorPick_MainFrame_Systeme.BackColor;
        Config.Instance.ColorOverlayBackground = ColorPick_Overlay_Background.BackColor;
        Config.Instance.ColorOverlaySystemeLight = ColorPick_Overlay_Systeme_Light.BackColor;
        Config.Instance.ColorOverlaySystemeDark = ColorPick_Overlay_Systeme_Dark.BackColor;
        Config.Instance.ColorOverlayTickLight = ColorPick_Overlay_Tick_Light.BackColor;
        Config.Instance.ColorOverlayTickDark = ColorPick_Overlay_Tick_Dark.BackColor;
        Config.Instance.ColorOverlayOverride = checkBox_Override.Checked;
        Config.Save();
        _parent.SetDesign();
    }
}