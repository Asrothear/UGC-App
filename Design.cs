﻿using System;
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
    public partial class Design : Form
    {
        private Mainframe parent;
        public Design(Mainframe pat)
        {
            InitializeComponent();
            parent = pat;
            int i = 0;
            foreach (Control grp in Controls)
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

            ColorPick_MainFrame_Background.BackColor = Properties.Settings.Default.Color_Main_Background;
            ColorPick_MainFrame_Infos.BackColor = Properties.Settings.Default.Color_Main_Info;
            ColorPick_MainFrame_Tick.BackColor = Properties.Settings.Default.Color_Main_Tick;
            ColorPick_MainFrame_Systeme.BackColor = Properties.Settings.Default.Color_Main_Systeme;
            ColorPick_Overlay_Background.BackColor = Properties.Settings.Default.Color_Overlay_Background;
            ColorPick_Overlay_Tick_Dark.BackColor = Properties.Settings.Default.Color_Overlay_Tick_Dark;
            ColorPick_Overlay_Tick_Light.BackColor = Properties.Settings.Default.Color_Overlay_Tick_Light;
            ColorPick_Overlay_Systeme_Dark.BackColor = Properties.Settings.Default.Color_Overlay_Systeme_Dark;
            ColorPick_Overlay_Systeme_Light.BackColor = Properties.Settings.Default.Color_Overlay_Systeme_Light;
            GetSetting();
            if (checkBox_Override.Checked)
            {
                groupBox_Overlay.Enabled = true;
                ColorPick_Overlay_Background.Enabled = true;
                ColorPick_Overlay_Tick_Light.Enabled = true;
                ColorPick_Overlay_Tick_Dark.Enabled = true;
                ColorPick_Overlay_Systeme_Light.Enabled = true;
                ColorPick_Overlay_Systeme_Dark.Enabled = true;
                label_Disclaimer.Visible = true;
            }
            else
            {
                groupBox_Overlay.Enabled = false;
                ColorPick_Overlay_Background.Enabled = false;
                ColorPick_Overlay_Tick_Light.Enabled = false;
                ColorPick_Overlay_Tick_Dark.Enabled = false;
                ColorPick_Overlay_Systeme_Light.Enabled = false;
                ColorPick_Overlay_Systeme_Dark.Enabled = false;
                label_Disclaimer.Visible = false;
            }

            checkBox_Override.CheckedChanged += (sender, args) =>
            {
                groupBox_Overlay.Enabled = checkBox_Override.Checked;
                ColorPick_Overlay_Background.Enabled = checkBox_Override.Checked;
                ColorPick_Overlay_Tick_Light.Enabled = checkBox_Override.Checked;
                ColorPick_Overlay_Tick_Dark.Enabled = checkBox_Override.Checked;
                ColorPick_Overlay_Systeme_Light.Enabled = checkBox_Override.Checked;
                ColorPick_Overlay_Systeme_Dark.Enabled = checkBox_Override.Checked;
                label_Disclaimer.Visible = checkBox_Override.Checked;
            };
            radioButton_Light.Click += (sender, args) => { ChangeTheme(0); };
            radioButton_Dark.Click += (sender, args) => { ChangeTheme(1); };
            radioButton_Custom.Click += (sender, args) => { ChangeTheme(2); };
            SetDesign(Properties.Settings.Default.Design_Sel);
        }

        private void ChangeTheme(int p0)
        {
            Properties.Settings.Default.Design_Sel = p0;
            Properties.Settings.Default.Save();
            parent.SetDesign(p0);
        }

        private void GetSetting()
        {
            switch (Properties.Settings.Default.Design_Sel)
            {
                case 0:
                case 1:

                    radioButton_Light.Checked = true;
                    radioButton_Dark.Checked = false;
                    radioButton_Custom.Checked = false;
                    if (Properties.Settings.Default.Design_Sel == 1)
                    {
                        radioButton_Light.Checked = false;
                        radioButton_Dark.Checked = true;
                    }
                    ColorPick_MainFrame_Background.BackColor = Properties.Settings.Default.Color_Default_Background_Light;
                    ColorPick_MainFrame_Infos.BackColor = Properties.Settings.Default.Color_Default_Label_Light;
                    ColorPick_MainFrame_Tick.BackColor = Properties.Settings.Default.Color_Default_Label_Light;
                    ColorPick_MainFrame_Systeme.BackColor = Properties.Settings.Default.Color_Default_Label_Light;
                    ColorPick_Overlay_Background.BackColor = Properties.Settings.Default.Color_Default_Chroma;
                    ColorPick_Overlay_Systeme_Light.BackColor = Properties.Settings.Default.Color_Default_Label_Light;
                    ColorPick_Overlay_Systeme_Dark.BackColor = Properties.Settings.Default.Color_Default_Label_Dark;
                    ColorPick_Overlay_Tick_Light.BackColor = Properties.Settings.Default.Color_Default_Label_Light;
                    ColorPick_Overlay_Tick_Dark.BackColor = Properties.Settings.Default.Color_Default_Label_Dark;
                    break;
                case 2:
                    radioButton_Light.Checked = false;
                    radioButton_Dark.Checked = false;
                    radioButton_Custom.Checked = true;
                    ColorPick_MainFrame_Background.BackColor = Properties.Settings.Default.Color_Main_Background;
                    ColorPick_MainFrame_Infos.BackColor = Properties.Settings.Default.Color_Main_Info;
                    ColorPick_MainFrame_Tick.BackColor = Properties.Settings.Default.Color_Main_Tick;
                    ColorPick_MainFrame_Systeme.BackColor = Properties.Settings.Default.Color_Main_Systeme;
                    ColorPick_Overlay_Background.BackColor = Properties.Settings.Default.Color_Overlay_Background;
                    ColorPick_Overlay_Systeme_Light.BackColor = Properties.Settings.Default.Color_Overlay_Systeme_Light;
                    ColorPick_Overlay_Systeme_Dark.BackColor = Properties.Settings.Default.Color_Overlay_Systeme_Dark;
                    ColorPick_Overlay_Tick_Light.BackColor = Properties.Settings.Default.Color_Overlay_Tick_Light;
                    ColorPick_Overlay_Tick_Dark.BackColor = Properties.Settings.Default.Color_Overlay_Tick_Dark;
                    break;
            }
        }

        private void SelectColor(Control control)
        {
            colorDialog1.Color = control.BackColor;
            colorDialog1.ShowDialog(this);
            control.BackColor = colorDialog1.Color;
        }

        public void SetDesign(int p0)
        {
            switch (p0)
            {
                case 0:
                    BackColor = Properties.Settings.Default.Color_Default_Background_Light;
                    foreach (Control control in Controls)
                    {
                        if (control is Label) control.ForeColor = Properties.Settings.Default.Color_Default_Label_Light;
                        if (control is CheckBox) control.ForeColor = Properties.Settings.Default.Color_Default_Label_Light;
                        if (control is RadioButton) control.ForeColor = Properties.Settings.Default.Color_Default_Label_Light;
                        if (control is GroupBox)
                        {
                            control.ForeColor = Properties.Settings.Default.Color_Default_Label_Light;
                            foreach (Control sub in Controls)
                            {
                                if (sub is Label) sub.ForeColor = Properties.Settings.Default.Color_Default_Label_Light;
                                if (sub is CheckBox) sub.ForeColor = Properties.Settings.Default.Color_Default_Label_Light;
                                if (sub is RadioButton) sub.ForeColor = Properties.Settings.Default.Color_Default_Label_Light;
                                if (sub is GroupBox) sub.ForeColor = Properties.Settings.Default.Color_Default_Label_Light;
                            }
                        }
                    }
                    break;
                case 1:
                    BackColor = Properties.Settings.Default.Color_Default_Background_Dark;
                    foreach (Control control in Controls)
                    {
                        if (control is Label) control.ForeColor = Properties.Settings.Default.Color_Default_Label_Dark;
                        if (control is CheckBox) control.ForeColor = Properties.Settings.Default.Color_Default_Label_Dark;
                        if (control is RadioButton) control.ForeColor = Properties.Settings.Default.Color_Default_Label_Dark;
                        if (control is GroupBox)
                        {
                            control.ForeColor = Properties.Settings.Default.Color_Default_Label_Dark;
                            foreach (Control sub in Controls)
                            {
                                if (sub is Label) sub.ForeColor = Properties.Settings.Default.Color_Default_Label_Dark;
                                if (sub is CheckBox) sub.ForeColor = Properties.Settings.Default.Color_Default_Label_Dark;
                                if (sub is RadioButton) sub.ForeColor = Properties.Settings.Default.Color_Default_Label_Dark;
                                if (sub is GroupBox) sub.ForeColor = Properties.Settings.Default.Color_Default_Label_Dark;
                            }
                        }
                    }
                    break;
                case 2:
                    BackColor = Properties.Settings.Default.Color_Main_Background;
                    foreach (Control control in Controls)
                    {
                        if (control is Label) control.ForeColor = Properties.Settings.Default.Color_Main_Info;
                        if (control is CheckBox) control.ForeColor = Properties.Settings.Default.Color_Main_Info;
                        if (control is RadioButton) control.ForeColor = Properties.Settings.Default.Color_Main_Info;
                        if (control is GroupBox)
                        {
                            control.ForeColor = Properties.Settings.Default.Color_Main_Info;
                            foreach (Control sub in Controls)
                            {
                                if (sub is Label) sub.ForeColor = Properties.Settings.Default.Color_Main_Info;
                                if (sub is CheckBox) sub.ForeColor = Properties.Settings.Default.Color_Main_Info;
                                if (sub is RadioButton) sub.ForeColor = Properties.Settings.Default.Color_Main_Info;
                                if (sub is GroupBox) sub.ForeColor = Properties.Settings.Default.Color_Main_Info;
                            }
                        }
                    }
                    break;
            }
        }
    }
}