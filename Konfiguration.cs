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
        public Konfiguration()
        {
            InitializeComponent();
            Load += (sender, args) =>
            {
                LoadKonfigs();
            };
            button_Save.Click += (sender, args) =>
            {
                SaveChanges();
                Close();
            };
        }

        void LoadKonfigs()
        {
            textBox_Send.Text = Properties.Settings.Default.Send_Url;
            textBox_State.Text = Properties.Settings.Default.State_Url;
            textBox_Token.Text = Properties.Settings.Default.Token;
            checkBox_CMDr.Checked = Properties.Settings.Default.Send_Name;
            checkBox_FullList.Checked = Properties.Settings.Default.Show_All;
            checkBox_OnlyBGS.Checked = Properties.Settings.Default.BGS_Only;
            checkBox_AutoUpdate.Checked = Properties.Settings.Default.Auto_Update;
            checkBox_SlowState.Checked = Properties.Settings.Default.SlowState;
            checkBox_Debug.Checked = Properties.Settings.Default.Debug;
            Activate();
        }

        void SaveChanges()
        {
            Properties.Settings.Default.Send_Url = textBox_Send.Text;
            Properties.Settings.Default.State_Url = textBox_State.Text;
            Properties.Settings.Default.Token = textBox_Token.Text;
            Properties.Settings.Default.Debug = checkBox_Debug.Checked;
            Properties.Settings.Default.BGS_Only = checkBox_OnlyBGS.Checked;
            Properties.Settings.Default.Send_Name = checkBox_CMDr.Checked;
            Properties.Settings.Default.Show_All = checkBox_FullList.Checked;
            Properties.Settings.Default.Auto_Update = checkBox_AutoUpdate.Checked;
            Properties.Settings.Default.SlowState = checkBox_SlowState.Checked;
            Properties.Settings.Default.Save();
        }
    }
}
