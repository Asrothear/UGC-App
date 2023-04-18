using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        public Konfiguration(Mainframe parrent)
        {
            InitializeComponent();
            Closing += (sender, args) =>
            {
                if (parrent.desg == null || parrent.desg.IsDisposed) return;
                if (parrent.desg.Visible)
                {
                    args.Cancel = true;
                    MessageBox.Show("Bitte erst das Design Fenster schließen!", "Achtung");
                }
            };
            Load += (sender, args) =>
            {
                LoadKonfigs();
            };
            button_Save.Click += (sender, args) =>
            {
                SaveChanges();
                parrent.RefreshListOnKonfigChange();
                Close();
            };
            button_Design.Click += (sender, args) =>
            {
                parrent.ShowDesign();
            };
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
        }

        void LoadKonfigs()
        {
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
            checkBox_RichPresence.Checked = Config.Instance.Use_RichPresence;
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
            Config.Instance.Use_RichPresence = checkBox_RichPresence.Checked;
            Config.Save();
            Program.SetStartup(checkBoxy_AutoStart.Checked);
            if(Config.Instance.Use_RichPresence)RichPresence.DiscordHandler.Start();
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
                    }
                    break;
                case 1:
                    BackColor = Config.Instance.Color_Default_Background_Dark;
                    foreach (Control control in Controls)
                    {
                        if (control is Label) control.ForeColor = Config.Instance.Color_Default_Label_Dark;
                        if (control is CheckBox) control.ForeColor = Config.Instance.Color_Default_Label_Dark;
                    }
                    break;
                case 2:
                    BackColor = Config.Instance.Color_Main_Background;
                    foreach (Control control in Controls)
                    {
                        if (control is Label) control.ForeColor = Config.Instance.Color_Main_Info;
                        if (control is CheckBox) control.ForeColor = Config.Instance.Color_Main_Info;
                    }
                    break;
            }
        }
    }
}
