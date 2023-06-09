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

namespace UGC_App.Order.DashViews
{
    public partial class OrderList : UserControl
    {
        public OrderList()
        {
            InitializeComponent();
            LoadAllOrdersTable();
            dataGridView_OrderList.CellDoubleClick += CheckEdit;
        }

        internal void Reload()
        {
            LoadAllOrdersTable();
        }
        private void LoadAllOrdersTable()
        {
            var table = new DataTable();
            table.Columns.Add("Priority", typeof(int));
            table.Columns.Add("System", typeof(string));
            table.Columns.Add("Fraktion", typeof(string));
            table.Columns.Add("Typ", typeof(string));
            table.Columns.Add("Infos", typeof(string));
            table.Columns.Add("Address", typeof(ulong));
            var lister = OrderAPI.GetAllOrders();
            if (lister == null) return;
            foreach (var OrderData in lister)
            {
                table.Rows.Add(OrderData.Priority, OrderData.StarSystem, OrderData.Faction, OrderData.Type, OrderData.Order, OrderData.SystemAddress);
            }
            dataGridView_OrderList.DataSource = table;
            Height = dataGridView_OrderList.Height + menuStrip1.Height + 5;
            dataGridView_OrderList.Columns["Address"].Visible = false;
            dataGridView_OrderList.Sort(dataGridView_OrderList.Columns["Priority"], ListSortDirection.Ascending);
            dataGridView_OrderList.DataBindingComplete += LoadColors;
        }

        private void LoadColors(object? sender, DataGridViewBindingCompleteEventArgs e)
        {
            for (var i = 0; i < dataGridView_OrderList.Rows.Count; i++)
            {
                dataGridView_OrderList.Rows[i].DefaultCellStyle.BackColor =
                    Convert.ToInt32(dataGridView_OrderList.Rows[i].Cells[0].Value) switch
                    {
                        0 => Color.FromArgb(208, 228, 252),
                        1 => Color.FromArgb(255, 216, 216),
                        2 => Color.FromArgb(255, 234, 120),
                        3 => Color.FromArgb(154, 255, 180),
                        _ => dataGridView_OrderList.Rows[i].DefaultCellStyle.BackColor
                    };
            }

            var size = dataGridView_OrderList.ClientSize;
            size.Height = (int)GetDgvMinHeight(dataGridView_OrderList);
            dataGridView_OrderList.ClientSize = size;
            size.Width = GetDgvMinWidth(dataGridView_OrderList);
            Size = size;
        }

        private void CheckEdit(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            OpenEditor(dataGridView_OrderList.Rows[e.RowIndex]);
        }

        private void OpenEditor(DataGridViewRow fractionData)
        {
            var def = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            var edit = new OrderEditor(fractionData, Convert.ToUInt64(fractionData.Cells[5].Value), this, true);
            if (edit is { IsDisposed: false }) { edit.ShowDialog(this); }
            Cursor.Current = def;
        }
        /// <summary>
        /// Return the minimum width in pixels a DataGridView can be before the control's vertical scrollbar would be displayed.
        /// </summary>
        private int GetDgvMinWidth(DataGridView dgv)
        {
            // Add two pixels for the border for BorderStyles other than None.
            var controlBorderWidth = (dgv.BorderStyle == BorderStyle.None) ? 0 : 2;

            // Return the width of all columns plus the row header, and adjusted for the DGV's BorderStyle.
            if (dgv.ClientSize.Height <= dgv.MaximumSize.Height)
                controlBorderWidth += SystemInformation.VerticalScrollBarWidth;
            return dgv.Columns.GetColumnsWidth(DataGridViewElementStates.Visible) + dgv.RowHeadersWidth + controlBorderWidth;
        }
        private double GetDgvMinHeight(DataGridView dgv)
        {
            // Add two pixels for the border for BorderStyles other than None.
            var controlBorderWidth = (dgv.BorderStyle == BorderStyle.None) ? 0 : 2;

            // Return the width of all columns plus the row header, and adjusted for the DGV's BorderStyle.
            return dgv.Rows.GetRowsHeight(DataGridViewElementStates.Visible) + controlBorderWidth;
        }
    }
}
