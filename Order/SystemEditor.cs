using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UGC_App.WebClient;

namespace UGC_App.Order
{
    public partial class SystemEditor : Form
    {
        public SystemEditor(string systemAddress)
        {
            InitializeComponent();
            var history = OrderAPI.GetSystemHistory(systemAddress);
            if (history.ToString() == "Unkown System or no Data!")
            {
                Close();
                return;
            }
            textBox1.Text = history.GetProperty("starSystem").ToString();
            textBox2.Text = systemAddress;
            textBox3.Text = history.GetProperty("starPos").ToString();
            textBox4.Text = history.GetProperty("starSystem").ToString();
            textBox5.Text = history.GetProperty("starSystem").ToString();
        }
    }
}
