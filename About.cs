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
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
            label2.Text = $"Version {Config.Instance.Version}{Config.Instance.Version_Meta}";
            foreach (Control controller in Controls)
            {
                if (controller is not Label) continue;
                controller.Left = (ClientSize.Width - controller.Width) / 2;
            }
        }

        public void SetDesign(int p0)
        {
            switch (p0)
            {
                case 0:
                    foreach (Control control in Controls)
                    {
                        if (control is Label) control.ForeColor = Config.Instance.Color_Default_Label_Light;
                        if (control is CheckBox) control.ForeColor = Config.Instance.Color_Default_Label_Light;
                    }
                    break;
                case 1:
                    foreach (Control control in Controls)
                    {
                        if (control is Label) control.ForeColor = Config.Instance.Color_Default_Label_Dark;
                        if (control is CheckBox) control.ForeColor = Config.Instance.Color_Default_Label_Dark;
                    }
                    break;
                case 2:
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
