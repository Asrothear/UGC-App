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

namespace UGC_App.Order.DashViews
{
    public partial class OrderList : UserControl
    {
        private DataGridViewRow lastrow;
        private Dashboard parrent;

        public OrderList(Dashboard dashboard)
        {
            parrent = dashboard;
            InitializeComponent();
            LoadAllOrdersTable();
            dataGridView_OrderList.DataBindingComplete += (sender, args) => LoadColors();
            dataGridView_OrderList.CellMouseDoubleClick += CheckEdit;
            dataGridView_OrderList.CellMouseClick += CheckContext;
            bearbeitenToolStripMenuItem.Click += (o, args) =>
            {
                if (lastrow == null) return;
                Refresh();
                OpenEditor(lastrow);
            };
            systemEditorÖffnenToolStripMenuItem.Click += (o, args) =>
            {
                if (lastrow == null) return;
                Refresh();
                OpenSysEditor(Convert.ToUInt64(lastrow.Cells[5].Value));
            };
            löschenToolStripMenuItem.Click += (sender, args) =>
            {
                var fractionData = lastrow;
                var res = MessageBox.Show(this, "Diese Anweisung wirklich entfernen?",
                    $"Anweisung für {fractionData.Cells[2].Value} löschen?", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (res != DialogResult.Yes) return;
                var def = Cursor.Current;
                Cursor.Current = Cursors.WaitCursor;
                var npriority = Convert.ToInt32(fractionData.Cells[0].Value);
                var nfractionName = fractionData.Cells[2].Value;
                var neelde = new Orders
                {
                    SystemAddress = Convert.ToUInt64(fractionData.Cells[5].Value),
                    Faction = fractionData.Cells[2].Value.ToString(),
                    Type = "",
                    Order = "",
                    Priority = npriority
                };
                var resp = OrderAPI.SaveOrders(neelde);
                if (resp)
                {
                    Refresh();
                    Cursor.Current = def;
                    return;
                }
                Cursor.Current = def;

            };
        }

        private void CheckContext(object? sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right || e.RowIndex < 0 || e.ColumnIndex < 0) return;
            dataGridView_OrderList.CurrentCell = dataGridView_OrderList.Rows[e.RowIndex].Cells[e.ColumnIndex];
            contextMenuStrip_Options.Show(Cursor.Position);
            lastrow = dataGridView_OrderList.Rows[e.RowIndex];
        }

        internal void Refresh()
        {
            //LoadAllOrdersTable();
            parrent.RefreshView(true);
        }

        private void LoadAllOrdersTable()
        {
            var table = new DataTable();
            table.Columns.Add("Priority", typeof(int)); //0
            table.Columns.Add("System", typeof(string)); //1
            table.Columns.Add("Fraktion", typeof(string)); //2
            table.Columns.Add("Typ", typeof(string)); //3
            table.Columns.Add("Infos", typeof(string)); //4
            table.Columns.Add("Address", typeof(ulong)); //5
            table.Columns.Add("letze Änderung", typeof(DateTime)); //6
            var lister = CacheHandler.GetOrderFromCache();
            if (lister == null) return;
            foreach (var OrderData in lister)
            {
                table.Rows.Add(OrderData.Priority, OrderData.StarSystem, OrderData.Faction, OrderData.Type,
                    OrderData.Order, OrderData.SystemAddress, OrderData.TimeStamp);
            }

            dataGridView_OrderList.DataSource = table;
            dataGridView_OrderList.Columns["Address"].Visible = false;
            dataGridView_OrderList.Sort(dataGridView_OrderList.Columns["Priority"], ListSortDirection.Ascending);
        }

        private void LoadColors()
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
            Invoke(() => dataGridView_OrderList.ClientSize = size);
            size.Width = GetDgvMinWidth(dataGridView_OrderList);
            Invoke(() => Size = size);
        }

        private void CheckEdit(object? sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            //Refresh();
            FinalCheck(e.RowIndex);
        }

        private void FinalCheck(int i)
        {
            OpenEditor(dataGridView_OrderList.Rows[i]);
        }

        private void OpenEditor(DataGridViewRow fractionData)
        {
            var def = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            var edit = new OrderEditor(fractionData, Convert.ToUInt64(fractionData.Cells[5].Value), this, true);
            if (edit is { IsDisposed: false })
            {
                edit.ShowDialog(this);
            }

            Cursor.Current = def;
        }

        private void OpenSysEditor(ulong systemAddress)
        {
            var def = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            var edit = new SystemEditor(systemAddress, this);
            if (edit is { IsDisposed: false })
            {
                edit.ShowDialog(this);
            }

            Cursor.Current = def;
        }

        private int GetDgvMinWidth(DataGridView dgv)
        {
            var controlBorderWidth = (dgv.BorderStyle == BorderStyle.None) ? 0 : 2;
            if (dgv.ClientSize.Height >= Size.Height)
                controlBorderWidth += SystemInformation.VerticalScrollBarWidth;
            return dgv.Columns.GetColumnsWidth(DataGridViewElementStates.Visible) + dgv.RowHeadersWidth +
                   controlBorderWidth;
        }

        private double GetDgvMinHeight(DataGridView dgv)
        {
            var controlBorderWidth = (dgv.BorderStyle == BorderStyle.None) ? 0 : 2;
            var mh = dgv.Rows.GetRowsHeight(DataGridViewElementStates.None) + controlBorderWidth;
            return mh;
        }
    }
}
