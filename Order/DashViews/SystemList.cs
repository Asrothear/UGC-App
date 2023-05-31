using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using UGC_App.WebClient;

namespace UGC_App.Order.DashViews
{
    public partial class SystemList : UserControl
    {
        public SystemList()
        {
            InitializeComponent();
            KeyPress += KeyDetect;
            dataGridView_SystemList.CellDoubleClick += CheckEdit;
            var table = new DataTable();
            table.Columns.Add("System", typeof(string));
            table.Columns.Add("Address", typeof(string));
            table.Columns.Add("stand BGS zahlen", typeof(string));
            table.Columns.Add("Anweisungen", typeof(string));
            //dataGridView_SystemList.Columns["Adress"].Visible = false;
            foreach (var systemData in OrderAPI.GetSystemList())
            {
                table.Rows.Add(systemData.GetProperty("starSystem"), systemData.GetProperty("systemAddress"), systemData.GetProperty("lastBGSData"), "Keine gefunden" );
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
                
            }
        }

        private void CheckEdit(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            OpenEditor(dataGridView_SystemList.Rows[e.RowIndex].Cells[1].Value.ToString());
        }

        private void OpenEditor(string systemAddress)
        {
            var def = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            var edit = new SystemEditor(systemAddress);
            if(edit is { IsDisposed: false }){edit.ShowDialog(this);}
            Cursor.Current = def;
        }
    }
}
