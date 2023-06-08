using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using UGC_App.Order.DashViews;
using UGC_App.Order.Model;
using UGC_App.WebClient;

namespace UGC_App.Order
{
    public partial class SystemEditor : Form
    {
        private ulong SystemAddress = 0;
        private SystemList parrent;
        public SystemEditor(ulong systemAddress, SystemList systemList)
        {
            InitializeComponent();
            TopMost = Config.Instance.AlwaysOnTop;
            parrent = systemList;
            SystemAddress = systemAddress;
            dataGridView_Orders.CellDoubleClick += CheckEdit;
            var history = OrderAPI.GetSystemHistory(SystemAddress);
            Text = $"SystemEditor: {history.starSystem} - {history.lastBGSData}";
            textBox_StarSystem.Text = history.starSystem;
            textBox_Address.Text = systemAddress.ToString();
            textBox_DistSOL.Text = $"{GetDistance(JsonSerializer.Deserialize<double[]>(history.starPos), new double[] { 0, 0, 0 })} Ly";
            textBox_DistHome.Text = $"{GetDistance(JsonSerializer.Deserialize<double[]>(history.starPos), new double[] { 54.21875, -154.84375, 30.625 })} Ly";
            textBox_StarPos.Text = history.starPos.ToString().Replace("[", "").Replace("]", "");
            textBox_Population.Text = history.population.ToString();
            textBox_Allegiance.Text = history.systemAllegiance;
            LoadBGSData(history);
        }

        private void LoadBGSData(SystemHistoryData data)
        {
            if (data.systemHistory.Any())
            {
                textBox_Factions.Text = data.systemHistory.Last().factions.Count.ToString();
            }
            LoadFactionTable(data);
            LoadHistoryTable(data);
            LoadOrdersTable(data);
        }

        internal void Reload()
        {
            LoadOrdersTable(OrderAPI.GetSystemHistory(SystemAddress));
            parrent.ReloadTable();
        }
        private void LoadOrdersTable(SystemHistoryData data)
        {
            var table = new DataTable();
            table.Columns.Add("Prio", typeof(int));
            //table.Columns.Add("System", typeof(string));
            table.Columns.Add("Fraction", typeof(string));
            table.Columns.Add("Typ", typeof(string));
            table.Columns.Add("Order", typeof(string));
            var Orders = OrderAPI.GetSystemOrders(SystemAddress);
            if (!Orders.Any())
            {
                foreach (var fraction in data.systemHistory.Last().factions)
                {
                    table.Rows.Add(0, fraction.name, "", "");
                }
            }
            else
            {
                foreach (var fraction in data.systemHistory.Last().factions)
                {
                    var facorder = Orders.FirstOrDefault(x => x.Faction == fraction.name);
                    if (facorder == null)
                    {
                        table.Rows.Add(0, fraction.name, "", "");
                        continue;
                    };
                    table.Rows.Add(facorder.Priority, fraction.name, facorder.Type, facorder.Order);
                }
            }

            dataGridView_Orders.DataSource = table;
            dataGridView_Orders.DataBindingComplete += LoadColors;
            dataGridView_Orders.Sort(dataGridView_Orders.Columns["Prio"], ListSortDirection.Ascending);
        }
        
        private void LoadColors(object? sender, DataGridViewBindingCompleteEventArgs e)
        {
            for (var i = 0; i < dataGridView_Orders.Rows.Count; i++)
            {
                switch (Convert.ToInt32(dataGridView_Orders.Rows[i].Cells[0].Value))
                {
                    case 0:
                        dataGridView_Orders.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(208, 228, 252);
                        break;
                    case 1:
                        dataGridView_Orders.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(255, 216, 216);
                        break;
                    case 2:
                        dataGridView_Orders.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(255, 234, 120);
                        break;
                    case 3:
                        dataGridView_Orders.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(154, 255, 180);
                        break;
                    default:
                        dataGridView_Orders.Rows[i].DefaultCellStyle.BackColor =
                            dataGridView_Orders.Rows[i].DefaultCellStyle.BackColor;
                        break;
                }
            }
        }

        private void LoadFactionTable(SystemHistoryData data)
        {
            var table = new DataTable();
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Happiness", typeof(string));
            table.Columns.Add("Infl", typeof(double));
            table.Columns.Add("Allegiance", typeof(string));
            table.Columns.Add("Government", typeof(string));
            table.Columns.Add("ActiveStates", typeof(string));
            table.Columns.Add("FactionState", typeof(string));
            table.Columns.Add("PendingStates", typeof(string));
            foreach (var systemFaction in data.systemHistory.Last().factions)
            {
                table.Rows.Add(systemFaction.name, systemFaction.happiness, Math.Round(100 * systemFaction.influence, 2),
                    systemFaction.allegiance, systemFaction.government);
            }
            dataGridView_Factions.DataSource = table;
            dataGridView_Factions.Sort(dataGridView_Factions.Columns["Name"], ListSortDirection.Ascending);
        }
        private void LoadHistoryTable(SystemHistoryData data)
        {
            var table = new DataTable();
            table.Columns.Add("Fraction", typeof(string));
            table.Columns.Add("Datum", typeof(string));
            foreach (var history in data.systemHistory)
            {
                try
                {
                    table.Columns.Add($"{ConvertTime(history.timestamp).Date:dd.MM.}", typeof(string));
                }
                catch
                {
                    data.systemHistory.Remove(history);
                }
            }
            foreach (var systemFaction in data.systemHistory.Last().factions)
            {
                table.Rows.Add(systemFaction.name, systemFaction.allegiance);
            }

            dataGridView_BgsHistory.DataSource = table;
            var vday = 2; //third columm is first day
            foreach (var hisory in data.systemHistory)
            {

                var vfac = 0; //second row is first fraction
                foreach (var fraction in hisory.factions)
                {
                    dataGridView_BgsHistory[vday, vfac].Value = Math.Round(100 * fraction.influence, 3).ToString();
                    vfac++;
                }
                vday++;
            }
        }
        private static DateTime ConvertTime(string timestamp)
        {
            return DateTime.ParseExact(timestamp, "yyyy-MM-dd'T'HH:mm:ss'Z'", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal).ToUniversalTime();
        }
        private void CheckEdit(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            OpenEditor(dataGridView_Orders.Rows[e.RowIndex]);
        }

        private void OpenEditor(DataGridViewRow fractionData)
        {
            var def = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            var edit = new OrderEditor(fractionData, SystemAddress, this);
            if (edit is { IsDisposed: false }) { edit.ShowDialog(this); }
            Cursor.Current = def;
        }

        private static double GetDistance(IReadOnlyList<double> cords1, IReadOnlyList<double> cords2, int digits = 2)
        {
            return Math.Round(Math.Sqrt(Math.Pow(cords1[0] - cords2[0], 2) + Math.Pow(cords1[1] - cords2[1], 2) + Math.Pow(cords1[2] - cords2[2], 2)), digits);
        }
    }
}
