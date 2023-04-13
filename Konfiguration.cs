﻿using System;
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
        public Konfiguration(Mainframe parrent)
        {
            InitializeComponent();
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
            checkBox_FullList.CheckStateChanged += (sender, args) =>
            {
                if (checkBox_FullList.Checked)
                {
                    checkBox_FullList.Text = "Gesamte Liste zeigen";
                    numericUpDown_ListCount.Visible = false;
                }
                else
                {
                    checkBox_FullList.Text = "Zeige nur x Systeme x=";
                    numericUpDown_ListCount.Visible = true;
                }
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
            checkBoxy_AutoStart.Checked = Properties.Settings.Default.AutoStart;
            numericUpDown_ListCount.Value = Properties.Settings.Default.ListCount;
            checkBox_CloseMini.Checked = Properties.Settings.Default.CloseMini;
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
            Properties.Settings.Default.AutoStart = checkBoxy_AutoStart.Checked;
            Properties.Settings.Default.ListCount = numericUpDown_ListCount.Value;
            Properties.Settings.Default.CloseMini = checkBox_CloseMini.Checked;
            Properties.Settings.Default.Save();
            Program.SetStartup(checkBoxy_AutoStart.Checked);
        }
    }
}
