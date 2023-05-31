using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UGC_App.Order.Model;
using UGC_App.WebClient;

namespace UGC_App.Order
{
    public partial class SystemEditor : Form
    {
        public SystemEditor(string systemAddress)
        {
            InitializeComponent();
            var history = OrderAPI.GetSystemHistory(systemAddress);
            Text = $"SystemEditor: {history.starSystem} - {history.lastBGSData}";
            textBox_StarSystem.Text = history.starSystem;
            textBox_Address.Text = systemAddress;
            textBox_StarPos.Text = history.starPos.ToString().Replace("[", "").Replace("]", "");
            textBox_Population.Text = history.population.ToString();
            textBox_Allegiance.Text = history.systemAllegiance;
            LoadBGSData(history);
        }

        private void LoadBGSData(SystemHistoryData data)
        {
            if (data.systemHistory.history.Any())
            {
                textBox_Factions.Text = data.systemHistory.history.Last().factions.Count.ToString();
            }

            LoadFactionTable(data);
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
            //dataGridView_SystemList.Columns["Adress"].Visible = false;
            foreach (var systemFaction in data.systemHistory.history.Last().factions)
            {
                table.Rows.Add(systemFaction.name, systemFaction.happiness, Math.Round(100 * systemFaction.influence, 2),
                    systemFaction.allegiance, systemFaction.government);
            }

            dataGridView_Factions.DataSource = table;
            //dataGridView_SystemList.Columns["Address"].Visible = false;
            dataGridView_Factions.Sort(dataGridView_Factions.Columns["Name"], ListSortDirection.Ascending);
        }
    }
}
