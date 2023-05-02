using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UGC_App
{
    public partial class Konfiguration : Form
    {
        private Design desg;
        private Mainframe parent;
        int i = 0;

        public Konfiguration(Mainframe parrent)
        {
            parent = parrent;
            InitializeComponent();
            tabControl1.SelectedIndex = 0;
            button_Save.Left = (ClientSize.Width - button_Save.Width) / 2;
            TopMost = Config.Instance.AlwaysOnTop;
            Closing += (sender, args) =>
            {
                if (parent.desg == null || parent.desg.IsDisposed) return;
                if (parent.desg.Visible)
                {
                    args.Cancel = true;
                    MessageBox.Show("Bitte erst das Design Fenster schließen!", "Achtung");
                }
            };
            Load += (sender, args) => { LoadKonfigs(); };
            button_Save.Click += (sender, args) =>
            {
                SaveChanges();
                parent.RefreshListOnKonfigChange();
                Close();
            };
            //button_Design.Click += (sender, args) => { parent.ShowDesign(); };
            checkBox_FullList.CheckStateChanged += (sender, args) =>
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
            if (!Config.Instance.Show_All)
            {
                checkBox_FullList.Text = "Zeige nur x Systeme    x=";
                numericUpDown_ListCount.Visible = true;
            }

            SetDesign(Config.Instance.Design_Sel);
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


            foreach (Control grp in tabPage2.Controls)
            {
                if (grp is not GroupBox) continue;
                Debug.WriteLine($"Grp {i++}");
                foreach (Control control in grp.Controls)
                {
                    if (control is PictureBox)
                    {
                        control.Click += (sender, args) => { SelectColor(control); };
                    }
                }
            }

            GetSetting();
            radioButton_Light.Click += (sender, args) => { ChangeTheme(0); };
            radioButton_Dark.Click += (sender, args) => { ChangeTheme(1); };
            radioButton_Custom.Click += (sender, args) => { ChangeTheme(2); };
            parent.SetDesign();
            checkBox_Override.Click += (sender, args) =>
            {
                groupBox_Overlay.Enabled = checkBox_Override.Checked;
                label_Disclaimer.Visible = checkBox_Override.Checked;
                ColorPick_Overlay_Background.BackColor = checkBox_Override.Checked
                    ? Config.Instance.Color_Overlay_Background
                    : Config.Instance.Color_Default_Chroma;
                ColorPick_Overlay_Systeme_Light.BackColor = checkBox_Override.Checked
                    ? Config.Instance.Color_Overlay_Systeme_Light
                    : Config.Instance.Color_Default_Label_Light;
                ColorPick_Overlay_Systeme_Dark.BackColor = checkBox_Override.Checked
                    ? Config.Instance.Color_Overlay_Systeme_Dark
                    : Config.Instance.Color_Default_Label_Dark;
                ColorPick_Overlay_Tick_Light.BackColor = checkBox_Override.Checked
                    ? Config.Instance.Color_Overlay_Tick_Light
                    : Config.Instance.Color_Default_Label_Light;
                ColorPick_Overlay_Tick_Dark.BackColor = checkBox_Override.Checked
                    ? Config.Instance.Color_Overlay_Tick_Dark
                    : Config.Instance.Color_Default_Label_Dark;
                Config.Instance.Color_Overlay_Override = checkBox_Override.Checked;
                Config.Save();
                GetSetting();
                parent.SetDesign();
            };
            button_ResetToLight.Click += (sender, args) => { ResetColors(true); };
            button_ResetToDark.Click += (sender, args) => { ResetColors(false); };
        }

        void LoadKonfigs()
        {
            tabControl1.SelectedIndex = 0;
            textBox_Send.Text = Config.Instance.Send_Url;
            textBox_State.Text = Config.Instance.State_Url;
            textBox_Token.Text = Config.Instance.Token;
            checkBox_CMDr.Checked = Config.Instance.Send_Name;
            checkBox_FullList.Checked = Config.Instance.Show_All;
            checkBox_OnlyBGS.Checked = Config.Instance.BGS_Only;
            checkBox_AutoUpdate.Checked = Config.Instance.Auto_Update;
            checkBox_SlowState.Checked = Config.Instance.SlowState;
            checkBox_Debug.Checked = Config.Instance.Debug;
            checkBoxy_AutoStart.Checked = Config.Instance.AutoStart;
            numericUpDown_ListCount.Value = Config.Instance.ListCount;
            checkBox_CloseMini.Checked = Config.Instance.CloseMini;
            checkBox_AlwaysTop.Checked = Config.Instance.AlwaysOnTop;
            checkBox_EDDN.Checked = Config.Instance.EDDN;
            Activate();
        }

        void SaveChanges()
        {
            Config.Instance.Send_Url = textBox_Send.Text;
            Config.Instance.State_Url = textBox_State.Text;
            Config.Instance.Token = textBox_Token.Text;
            Config.Instance.Debug = checkBox_Debug.Checked;
            Config.Instance.BGS_Only = checkBox_OnlyBGS.Checked;
            Config.Instance.Send_Name = checkBox_CMDr.Checked;
            Config.Instance.Show_All = checkBox_FullList.Checked;
            Config.Instance.Auto_Update = checkBox_AutoUpdate.Checked;
            Config.Instance.SlowState = checkBox_SlowState.Checked;
            Config.Instance.AutoStart = checkBoxy_AutoStart.Checked;
            Config.Instance.ListCount = numericUpDown_ListCount.Value;
            Config.Instance.CloseMini = checkBox_CloseMini.Checked;
            Config.Instance.AlwaysOnTop = checkBox_AlwaysTop.Checked;
            Config.Instance.EDDN = checkBox_EDDN.Checked;
            Config.Save();
            Program.SetStartup(checkBoxy_AutoStart.Checked);
            parent.TopMost = Config.Instance.AlwaysOnTop;
        }

        public void SetDesign(int p0)
        {
            switch (p0)
            {
                case 0:
                    BackColor = Config.Instance.Color_Default_Background_Light;
                    foreach (Control control in Controls)
                    {
                        if (control is Label) control.ForeColor = Config.Instance.Color_Default_Label_Light;
                        if (control is CheckBox) control.ForeColor = Config.Instance.Color_Default_Label_Light;
                        if (control is TabControl)
                        {
                            control.BackColor = Config.Instance.Color_Default_Background_Light;
                            foreach (Control lab in control.Controls)
                            {
                                if (lab is Label) lab.ForeColor = Config.Instance.Color_Default_Label_Light;
                                if (lab is CheckBox) lab.ForeColor = Config.Instance.Color_Default_Label_Light;
                                if (lab is TabPage)
                                {
                                    lab.BackColor = Config.Instance.Color_Default_Background_Light;
                                    foreach (Control labs in lab.Controls)
                                    {
                                        if (labs is Label) labs.ForeColor = Config.Instance.Color_Default_Label_Light;
                                        if (labs is CheckBox)
                                            labs.ForeColor = Config.Instance.Color_Default_Label_Light;
                                        if (labs is RadioButton)
                                            labs.ForeColor = Config.Instance.Color_Default_Label_Light;
                                        if (labs is GroupBox)
                                        {
                                            labs.ForeColor = Config.Instance.Color_Default_Label_Light;
                                            foreach (Control sub in labs.Controls)
                                            {
                                                if (sub is Label) sub.ForeColor = Config.Instance.Color_Default_Label_Light;
                                                if (sub is CheckBox)
                                                    sub.ForeColor = Config.Instance.Color_Default_Label_Light;
                                                if (sub is RadioButton)
                                                    sub.ForeColor = Config.Instance.Color_Default_Label_Light;
                                                if (sub is GroupBox)
                                                    sub.ForeColor = Config.Instance.Color_Default_Label_Light;
                                                if (sub is Button)
                                                    sub.ForeColor = Config.Instance.Color_Default_Label_Light;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    break;
                case 1:
                    BackColor = Config.Instance.Color_Default_Background_Dark;
                    foreach (Control control in Controls)
                    {
                        if (control is Label) control.ForeColor = Config.Instance.Color_Default_Label_Dark;
                        if (control is CheckBox) control.ForeColor = Config.Instance.Color_Default_Label_Dark;
                        if (control is TabControl)
                        {
                            control.BackColor = Config.Instance.Color_Default_Background_Dark;
                            foreach (Control lab in control.Controls)
                            {
                                if (lab is Label) lab.ForeColor = Config.Instance.Color_Default_Label_Dark;
                                if (lab is CheckBox) lab.ForeColor = Config.Instance.Color_Default_Label_Dark;
                                if (lab is TabPage)
                                {
                                    lab.BackColor = Config.Instance.Color_Default_Background_Dark;
                                    foreach (Control labs in lab.Controls)
                                    {
                                        if (labs is Label) labs.ForeColor = Config.Instance.Color_Default_Label_Dark;
                                        if (labs is CheckBox) labs.ForeColor = Config.Instance.Color_Default_Label_Dark;
                                        if (labs is RadioButton)
                                            labs.ForeColor = Config.Instance.Color_Default_Label_Dark;
                                        if (labs is GroupBox)
                                        {
                                            labs.ForeColor = Config.Instance.Color_Default_Label_Dark;
                                            foreach (Control sub in labs.Controls)
                                            {
                                                if (sub is Label) sub.ForeColor = Config.Instance.Color_Default_Label_Dark;
                                                if (sub is CheckBox)
                                                    sub.ForeColor = Config.Instance.Color_Default_Label_Dark;
                                                if (sub is RadioButton)
                                                    sub.ForeColor = Config.Instance.Color_Default_Label_Dark;
                                                if (sub is GroupBox)
                                                    sub.ForeColor = Config.Instance.Color_Default_Label_Dark;
                                                if (sub is Button)
                                                    sub.ForeColor = Config.Instance.Color_Default_Label_Light;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    break;
                case 2:
                    BackColor = Config.Instance.Color_Main_Background;
                    foreach (Control control in Controls)
                    {
                        if (control is Label) control.ForeColor = Config.Instance.Color_Main_Info;
                        if (control is CheckBox) control.ForeColor = Config.Instance.Color_Main_Info;
                        if (control is TabControl)
                        {
                            control.BackColor = Config.Instance.Color_Main_Background;
                            foreach (Control lab in control.Controls)
                            {
                                if (lab is Label) lab.ForeColor = Config.Instance.Color_Main_Info;
                                if (lab is CheckBox) lab.ForeColor = Config.Instance.Color_Main_Info;
                                if (lab is TabPage)
                                {
                                    lab.BackColor = Config.Instance.Color_Main_Background;
                                    foreach (Control labs in lab.Controls)
                                    {
                                        if (labs is Label) labs.ForeColor = Config.Instance.Color_Main_Info;
                                        if (labs is CheckBox)
                                            labs.ForeColor = Config.Instance.Color_Main_Info;
                                        if (labs is RadioButton)
                                            labs.ForeColor = Config.Instance.Color_Main_Info;
                                        if (labs is GroupBox)
                                        {
                                            labs.ForeColor = Config.Instance.Color_Main_Info;
                                            foreach (Control sub in labs.Controls)
                                            {
                                                if (sub is Label) sub.ForeColor = Config.Instance.Color_Main_Info;
                                                if (sub is CheckBox)
                                                    sub.ForeColor = Config.Instance.Color_Main_Info;
                                                if (sub is RadioButton)
                                                    sub.ForeColor = Config.Instance.Color_Main_Info;
                                                if (sub is GroupBox)
                                                    sub.ForeColor = Config.Instance.Color_Main_Info;
                                                if (sub is Button)
                                                    sub.ForeColor = Config.Instance.Color_Default_Label_Light;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    break;
            }
        }

        private void ResetColors(bool p0)
        {
            ColorPick_MainFrame_Background.BackColor = p0
                ? Config.Instance.Color_Default_Background_Light
                : Config.Instance.Color_Default_Background_Dark;
            ColorPick_MainFrame_Infos.BackColor = p0
                ? Config.Instance.Color_Default_Label_Light
                : Config.Instance.Color_Default_Label_Dark;
            ColorPick_MainFrame_Tick.BackColor = p0
                ? Config.Instance.Color_Default_Label_Light
                : Config.Instance.Color_Default_Label_Dark;
            ColorPick_MainFrame_Systeme.BackColor = p0
                ? Config.Instance.Color_Default_Label_Light
                : Config.Instance.Color_Default_Label_Dark;
            ColorPick_Overlay_Background.BackColor = Config.Instance.Color_Default_Chroma;
            ColorPick_Overlay_Systeme_Light.BackColor = Config.Instance.Color_Default_Label_Light;
            ColorPick_Overlay_Systeme_Dark.BackColor = Config.Instance.Color_Default_Label_Dark;
            ColorPick_Overlay_Tick_Light.BackColor = Config.Instance.Color_Default_Label_Light;
            ColorPick_Overlay_Tick_Dark.BackColor = Config.Instance.Color_Default_Label_Dark;
            Config.Instance.Color_Main_Background = ColorPick_MainFrame_Background.BackColor;
            Config.Instance.Color_Main_Info = ColorPick_MainFrame_Infos.BackColor;
            Config.Instance.Color_Main_Tick = ColorPick_MainFrame_Tick.BackColor;
            Config.Instance.Color_Main_Systeme = ColorPick_MainFrame_Systeme.BackColor;
            if (checkBox_Override.Checked)
            {

                Config.Instance.Color_Overlay_Background = ColorPick_Overlay_Background.BackColor;
                Config.Instance.Color_Overlay_Systeme_Light = ColorPick_Overlay_Systeme_Light.BackColor;
                Config.Instance.Color_Overlay_Systeme_Dark = ColorPick_Overlay_Systeme_Dark.BackColor;
                Config.Instance.Color_Overlay_Tick_Light = ColorPick_Overlay_Tick_Light.BackColor;
                Config.Instance.Color_Overlay_Tick_Dark = ColorPick_Overlay_Tick_Dark.BackColor;
                Config.Instance.Color_Overlay_Override = checkBox_Override.Checked;
            }

            Config.Save();
            parent.SetDesign();
        }

        private void ChangeTheme(int p0)
        {
            Config.Instance.Design_Sel = p0;
            Config.Save();
            parent.SetDesign();
            GetSetting();
        }

        private void GetSetting()
        {
            switch (Config.Instance.Design_Sel)
            {
                case 0:
                case 1:
                    radioButton_Light.Checked = true;
                    radioButton_Dark.Checked = false;
                    radioButton_Custom.Checked = false;
                    if (Config.Instance.Design_Sel == 1)
                    {
                        radioButton_Light.Checked = false;
                        radioButton_Dark.Checked = true;
                    }

                    ColorPick_MainFrame_Background.BackColor = radioButton_Light.Checked
                        ? Config.Instance.Color_Default_Background_Light
                        : Config.Instance.Color_Default_Background_Dark;
                    ColorPick_MainFrame_Infos.BackColor = radioButton_Light.Checked
                        ? Config.Instance.Color_Default_Label_Light
                        : Config.Instance.Color_Default_Label_Dark;
                    ColorPick_MainFrame_Tick.BackColor = radioButton_Light.Checked
                        ? Config.Instance.Color_Default_Label_Light
                        : Config.Instance.Color_Default_Label_Dark;
                    ColorPick_MainFrame_Systeme.BackColor = radioButton_Light.Checked
                        ? Config.Instance.Color_Default_Label_Light
                        : Config.Instance.Color_Default_Label_Dark;
                    ColorPick_Overlay_Background.BackColor = Config.Instance.Color_Default_Chroma;
                    ColorPick_Overlay_Systeme_Light.BackColor = Config.Instance.Color_Default_Label_Light;
                    ColorPick_Overlay_Systeme_Dark.BackColor = Config.Instance.Color_Default_Label_Dark;
                    ColorPick_Overlay_Tick_Light.BackColor = Config.Instance.Color_Default_Label_Light;
                    ColorPick_Overlay_Tick_Dark.BackColor = Config.Instance.Color_Default_Label_Dark;
                    break;
                case 2:
                    radioButton_Light.Checked = false;
                    radioButton_Dark.Checked = false;
                    radioButton_Custom.Checked = true;
                    ColorPick_MainFrame_Background.BackColor = Config.Instance.Color_Main_Background;
                    ColorPick_MainFrame_Infos.BackColor = Config.Instance.Color_Main_Info;
                    ColorPick_MainFrame_Tick.BackColor = Config.Instance.Color_Main_Tick;
                    ColorPick_MainFrame_Systeme.BackColor = Config.Instance.Color_Main_Systeme;
                    ColorPick_Overlay_Background.BackColor = checkBox_Override.Checked
                        ? Config.Instance.Color_Overlay_Background
                        : Config.Instance.Color_Default_Chroma;
                    ColorPick_Overlay_Systeme_Light.BackColor = checkBox_Override.Checked
                        ? Config.Instance.Color_Overlay_Systeme_Light
                        : Config.Instance.Color_Default_Label_Light;
                    ColorPick_Overlay_Systeme_Dark.BackColor = checkBox_Override.Checked
                        ? Config.Instance.Color_Overlay_Systeme_Dark
                        : Config.Instance.Color_Default_Label_Dark;
                    ColorPick_Overlay_Tick_Light.BackColor = checkBox_Override.Checked
                        ? Config.Instance.Color_Overlay_Tick_Light
                        : Config.Instance.Color_Default_Label_Light;
                    ColorPick_Overlay_Tick_Dark.BackColor = checkBox_Override.Checked
                        ? Config.Instance.Color_Overlay_Tick_Dark
                        : Config.Instance.Color_Default_Label_Dark;
                    break;
            }

            checkBox_Override.Checked = Config.Instance.Color_Overlay_Override;
            ColorPick_MainFrame_Background.Enabled = radioButton_Custom.Checked;
            ColorPick_MainFrame_Infos.Enabled = radioButton_Custom.Checked;
            ColorPick_MainFrame_Tick.Enabled = radioButton_Custom.Checked;
            ColorPick_MainFrame_Systeme.Enabled = radioButton_Custom.Checked;
            groupBox_Overlay.Enabled = checkBox_Override.Checked;
            label_Disclaimer.Visible = checkBox_Override.Checked;
            checkBox_Override.Visible = radioButton_Custom.Checked;
            button_ResetToLight.Visible = radioButton_Custom.Checked;
            button_ResetToDark.Visible = radioButton_Custom.Checked;
            if (!radioButton_Custom.Checked)
            {
                groupBox_Overlay.Enabled = radioButton_Custom.Checked;
                label_Disclaimer.Visible = radioButton_Custom.Checked;
                checkBox_Override.Visible = radioButton_Custom.Checked;
            }
        }

        private void SelectColor(Control control)
        {
            colorDialog1.Color = control.BackColor;
            colorDialog1.ShowDialog(this);
            control.BackColor = colorDialog1.Color;
            Config.Instance.Color_Main_Background = ColorPick_MainFrame_Background.BackColor;
            Config.Instance.Color_Main_Info = ColorPick_MainFrame_Infos.BackColor;
            Config.Instance.Color_Main_Tick = ColorPick_MainFrame_Tick.BackColor;
            Config.Instance.Color_Main_Systeme = ColorPick_MainFrame_Systeme.BackColor;
            Config.Instance.Color_Overlay_Background = ColorPick_Overlay_Background.BackColor;
            Config.Instance.Color_Overlay_Systeme_Light = ColorPick_Overlay_Systeme_Light.BackColor;
            Config.Instance.Color_Overlay_Systeme_Dark = ColorPick_Overlay_Systeme_Dark.BackColor;
            Config.Instance.Color_Overlay_Tick_Light = ColorPick_Overlay_Tick_Light.BackColor;
            Config.Instance.Color_Overlay_Tick_Dark = ColorPick_Overlay_Tick_Dark.BackColor;
            Config.Instance.Color_Overlay_Override = checkBox_Override.Checked;
            Config.Save();
            parent.SetDesign();
        }
    }
}
