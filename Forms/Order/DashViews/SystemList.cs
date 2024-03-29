﻿using System.ComponentModel;
using System.Data;
using UGC_App.LocalCache;

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
            Reload();
        }

        internal void Refresh()
        {
            CacheHandler.CacheSystemList(true);
            Reload();
        }

        internal void Reload()
        {
            var table = new DataTable();
            table.Columns.Add("System", typeof(string));
            table.Columns.Add("Address", typeof(string));
            table.Columns.Add("stand BGS zahlen (UTC)", typeof(string));
            table.Columns.Add("Anweisungen", typeof(string));
            var lister = CacheHandler.GetSystemListFromCache();
            foreach (var systemData in lister)
            {
                table.Rows.Add(systemData.StarSystem, systemData.SystemAddress, systemData.LastBgsData, systemData.Count);
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
                Reload();
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
