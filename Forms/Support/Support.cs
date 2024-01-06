using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UGC_App.ErrorReporter;

namespace UGC_App.Forms.Support
{
    public partial class Support : Form
    {
        public Support()
        {
            InitializeComponent();
            if (!textBox_Support.Enabled) textBox_Support.Text = "";
            textBox_Support.Enabled = true;
            button_Report_Senden.Enabled = true;
            button_Report_Senden.Click += (_, _) => SendReport();
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
                        MessageBox.Show(this, "Report gesendet!", "Reporter");
                    });
            
                }
                else
                {
                    Invoke(() =>
                    {
                        textBox_Support.Enabled = true;
                        button_Report_Senden.Enabled = true;
                        MessageBox.Show(this, "Es konnte kein Report gesendet werden!", "Reporter");
                    });
                }
            });

        }
    }
    
}
