using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UGC_App.LocalCache;
using UGC_App.Order.Model;
using UGC_App.WebClient;

namespace UGC_App.Order
{
    public partial class OrderEditor : Form
    {
        private ulong _systemAddress;
        private int priority;
        private string fractionName;
        private string type;
        private string order;
        private dynamic? parent;
        public OrderEditor(DataGridViewRow fractionData, ulong systemAddress, dynamic parrent, bool direct = false)
        {
            InitializeComponent();
            TopMost = Config.Instance.AlwaysOnTop;
            _systemAddress = systemAddress;
            parent = parrent;
            priority = Convert.ToInt32(fractionData.Cells[0].Value);
            if (direct) Text = $"OrderEditor: {fractionData.Cells[1].Value}";
            fractionName = !direct ? fractionData.Cells[1].Value.ToString() : fractionData.Cells[2].Value.ToString();
            type = !direct ? fractionData.Cells[2].Value.ToString() : fractionData.Cells[3].Value.ToString();
            order = !direct ? fractionData.Cells[3].Value.ToString() : fractionData.Cells[4].Value.ToString();
            numericUpDown_Prio.Value = priority;
            comboBox_Type.Text = type;
            textBox_Orders.Text = order;
            label3.Text = fractionName;
            CenterObjectHorizontally(label3);
            CenterObjectHorizontally(textBox_Orders);
            button_abort.Click += (sender, args) => Close();
            button_Save.Click += (sender, args) => Save();
            button_Remove.Click += (sender, args) => Remove();
            if (string.IsNullOrWhiteSpace(comboBox_Type.Text) && string.IsNullOrWhiteSpace(textBox_Orders.Text))
                button_Remove.Visible = false;
        }

        private void Remove()
        {
            var res = MessageBox.Show(this, "Diese Anweisung wirklich entfernen?", $"Anweisung {label3.Text} löschen?", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (res != DialogResult.Yes) return;
            var def = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            var npriority = Convert.ToInt32(numericUpDown_Prio.Value);
            var nfractionName = label3.Text;
            var neelde = new Orders
            {
                SystemAddress = _systemAddress,
                Faction = nfractionName,
                Type = "",
                Order = "",
                Priority = npriority
            };
            var resp = OrderAPI.SaveOrders(neelde);
            if (resp)
            {
                parent?.Refresh();
                Cursor.Current = def;
                Close();
                return;
            };
            Cursor.Current = def;
        }

        private void CenterObjectHorizontally(dynamic label)
        {
            if (IsDisposed) return;
            label.Left = (ClientSize.Width - label.Width) / 2;
        }

        private void Save()
        {
            var def = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            var npriority = Convert.ToInt32(numericUpDown_Prio.Value);
            var nfractionName = label3.Text;
            var ntype = comboBox_Type.Text;
            var norder = textBox_Orders.Text;
            var neelde = new Orders
            {
                SystemAddress = _systemAddress,
                Faction = nfractionName,
                Type = ntype,
                Order = norder,
                Priority = npriority
            };
            var resp = false;
            if(npriority != priority || ntype != type || norder != order) resp = OrderAPI.SaveOrders(neelde);
            if (resp)
            {
                CacheHandler.CacheOrder(true);
                parent?.Refresh();
                Cursor.Current = def;
                Close();
                return;
            };
            numericUpDown_Prio.Value = priority;
            comboBox_Type.Text = type;
            textBox_Orders.Text = order;
            label3.Text = fractionName;
            Cursor.Current = def;
            Close();
        }
    }
}
