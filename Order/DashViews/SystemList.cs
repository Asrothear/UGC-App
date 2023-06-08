using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using UGC_App.Order.Model;
using UGC_App.WebClient;

namespace UGC_App.Order.DashViews
{
    public partial class SystemList : UserControl
    { 
        public SystemList()
        {
            InitializeComponent();
            KeyPress += KeyDetect;
            if (Parent != null) Parent.KeyPress += KeyDetect;
            dataGridView_SystemList.CellDoubleClick += CheckEdit;
            ReloadTable();
        }

        internal void Refresh(object? sender, EventArgs e)
        {
            ReloadTable();
        }

        internal void ReloadTable()
        {
            var table = new DataTable();
            table.Columns.Add("System", typeof(string));
            table.Columns.Add("Address", typeof(string));
            table.Columns.Add("stand BGS zahlen (UTC)", typeof(string));
            table.Columns.Add("Anweisungen", typeof(string));
            var lister = OrderAPI.GetSystemList();
            if (lister == null) return;
            foreach (var systemData in lister)
            {
                table.Rows.Add(systemData.GetProperty("starSystem"), systemData.GetProperty("systemAddress"), systemData.GetProperty("lastBGSData"), systemData.GetProperty("count"));
            }
            dataGridView_SystemList.DataSource = table;
            dataGridView_SystemList.Columns["Address"].Visible = false;
            Height = dataGridView_SystemList.Height + menuStrip1.Height + 5;
            dataGridView_SystemList.Sort(dataGridView_SystemList.Columns["System"], ListSortDirection.Ascending);
            
        }

        private void KeyDetect(object? sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (int)Keys.F5)
            {
                ReloadTable();
            }
        }

        private void CheckEdit(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            var adress = Convert.ToUInt64(dataGridView_SystemList.Rows[e.RowIndex].Cells[1].Value);
            if (adress == 0) return;
            OpenEditor(adress);
        }

        private void OpenEditor(ulong systemAddress)
        {
            var def = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            var edit = new SystemEditor(systemAddress, this);
            if (edit is { IsDisposed: false }) { edit.ShowDialog(this); }
            Cursor.Current = def;
        }
    }
}
