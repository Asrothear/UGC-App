﻿using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using UGC_App.Forms.Order.Model;
using UGC_App.LocalCache;

namespace UGC_App.Order
{
    public partial class SystemEditor : Form
    {
        private ulong SystemAddress = 0;
        private readonly dynamic _parrent;

        internal class FacDressing
        {
            internal string Name;
            internal int Adress;
        }
        public SystemEditor(ulong systemAddress, dynamic systemList)
        {
            InitializeComponent();
            TopMost = Config.Instance.AlwaysOnTop;
            _parrent = systemList;
            SystemAddress = systemAddress;
            dataGridView_Orders.CellDoubleClick += CheckEdit;
            var history = CacheHandler.GetHistoryFromCache().FirstOrDefault(x=> x.systemAddress ==SystemAddress) ?? new SystemHistoryData();
            Text = $"SystemEditor: {history.starSystem} - {history.lastBGSData}";
            textBox_StarSystem.Text = history.starSystem;
            textBox_Address.Text = systemAddress.ToString();
            textBox_DistSOL.Text = $"{GetDistance(JsonSerializer.Deserialize<double[]>(history.starPos), new double[] { 0, 0, 0 })} Ly";
            textBox_DistHome.Text = $"{GetDistance(JsonSerializer.Deserialize<double[]>(history.starPos), new double[] { 54.21875, -154.84375, 30.625 })} Ly";
            textBox_StarPos.Text = history.starPos.Replace("[", "").Replace("]", "");
            textBox_Population.Text = history.population.ToString();
            textBox_Allegiance.Text = history.systemAllegiance;
            LoadBGSData(history);
            dataGridView_Orders.DataBindingComplete += LoadOrderColors;
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
            LoadConflitsTable(data);
        }

        internal void Refresh()
        {
            LoadOrdersTable(CacheHandler.GetHistoryFromCache().FirstOrDefault(x => x.systemAddress == SystemAddress));
            _parrent?.Refresh();
        }

        private void LoadConflitsTable(SystemHistoryData data)
        {
            var table = new DataTable();
            table.Columns.Add("Typ", typeof(string));
            table.Columns.Add("Status", typeof(string));
            table.Columns.Add("Fraction 1", typeof(string));
            table.Columns.Add("Fraction 2", typeof(string));
            table.Columns.Add("Stand (Tie)", typeof(string));
            table.Columns.Add("Dauer", typeof(string));
            var conflitcs = data.systemHistory.Last().conflicts;
            if (conflitcs != null && conflitcs.Any())
            {
                foreach (var conflict in conflitcs)
                {
                    var points = $"{conflict.faction1.wonDays}:{conflict.faction2.wonDays}";
                    var duartion = 0;
                    var confd = GetConflictDuration(data.systemHistory, conflict);
                    if(conflict.status == null || conflict.status.ToLower() != "pending")duartion = confd[0];
                    if (confd[1] > 0) points += $" ({confd[1]})";
                    table.Rows.Add(conflict.warType, conflict.status ?? "", conflict.faction1.name, conflict.faction2.name,
                        points, $"{duartion.ToString()} Tage");
                }
            }
            dataGridView_Conflicts.DataSource = table;
        }

        private int[] GetConflictDuration(HashSet<SystemHistoryData.HisoryModel> dataSystemHistory,
            SystemHistoryData.HisoryModel.ConflictsO conflictsO)
        {
            var fac1 = conflictsO.faction1.name;
            var fac2 = conflictsO.faction2.name;
            var conf = conflictsO.warType;
            var result = 1;
            var irt = 1;
            var tie = 0;
            var lastdate = DateTime.MinValue;
            for (var i = dataSystemHistory.Count - 1; i >= 0; i--)
            {
                var dat = dataSystemHistory.ElementAt(i);
                if (dat.conflicts != null && dat.conflicts.Any())
                {
                    foreach (var conflict in dat.conflicts.Where(conflict => conflict.warType == conf && conflict.faction1.name == fac1 && conflict.faction2.name == fac2 && lastdate != dat.TickTime ))
                    {
                        if (conflict.status?.ToLower() == "pending")continue;
                        if (conflict.faction1.wonDays == 0 && conflict.faction2.wonDays == 0) tie++;
                        result++;
                    }
                }
                lastdate = dat.TickTime;
                irt++;
                if(irt>=8)break;
                
            }

            var fi = new[] { result, tie };
            return fi;
        }

        private void LoadOrdersTable(SystemHistoryData data)
        {
            var table = new DataTable();
            table.Columns.Add("Prio", typeof(int));
            table.Columns.Add("Fraction", typeof(string));
            table.Columns.Add("Typ", typeof(string));
            table.Columns.Add("Order", typeof(string));
            var Orders = CacheHandler.GetOrderFromCache().Where(x=> x.SystemAddress ==SystemAddress).ToHashSet();
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
            dataGridView_Orders.Sort(dataGridView_Orders.Columns["Prio"], ListSortDirection.Ascending);
        }

        private void LoadOrderColors(object? sender, DataGridViewBindingCompleteEventArgs e)
        {
            for (var i = 0; i < dataGridView_Orders.Rows.Count; i++)
            {
                dataGridView_Orders.Rows[i].DefaultCellStyle.BackColor =
                    Convert.ToInt32(dataGridView_Orders.Rows[i].Cells[0].Value) switch
                    {
                        0 => Color.FromArgb(208, 228, 252),
                        1 => Color.FromArgb(255, 216, 216),
                        2 => Color.FromArgb(255, 234, 120),
                        3 => Color.FromArgb(154, 255, 180),
                        _ => dataGridView_Orders.Rows[i].DefaultCellStyle.BackColor
                    };
            }
        }

        private void LoadHistoryColors(object? sender, DataGridViewBindingCompleteEventArgs e)
        {
            for (var i = 0; i < dataGridView_BgsHistory.Rows.Count; i++)
            {
                var first = true;
                var a = 0.0;
                for (var ii = 2; ii < dataGridView_BgsHistory.Columns.Count; ii++)
                {
                    if (string.IsNullOrWhiteSpace(dataGridView_BgsHistory.Rows[i].Cells[ii].Value.ToString()))
                        dataGridView_BgsHistory.Rows[i].Cells[ii].Value = "-1";
                    if (first)
                    {
                        a = Math.Round(Convert.ToDouble(dataGridView_BgsHistory.Rows[i].Cells[ii].Value), 3, MidpointRounding.AwayFromZero);
                        first = false;
                        continue;
                    }
                    var b = Math.Round(Convert.ToDouble(dataGridView_BgsHistory.Rows[i].Cells[ii].Value), 3, MidpointRounding.AwayFromZero);
                    if (a > b)
                    {
                        dataGridView_BgsHistory.Rows[i].Cells[ii].Style.ForeColor = Color.Brown;
                    }
                    if (b > a)
                    {
                        dataGridView_BgsHistory.Rows[i].Cells[ii].Style.ForeColor = Color.DarkGreen;
                    }
                    a = b;
                    if(ii == dataGridView_BgsHistory.Columns.Count-1) dataGridView_BgsHistory.Rows[i].Cells[ii].Style.Font = new Font(dataGridView_BgsHistory.Font, FontStyle.Bold);
                    if (dataGridView_BgsHistory.Rows[i].Cells[ii].Value == "-1") dataGridView_BgsHistory.Rows[i].Cells[ii].Value = "-";
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
                var activeStates = new HashSet<string>();
                if (systemFaction.activeStates?.Any() != null) foreach (var active in systemFaction.activeStates) activeStates.Add(active.name);
                var pendingStates = new HashSet<string>();
                if (systemFaction.pendingStates?.Any() != null) foreach (var pending in systemFaction.pendingStates) pendingStates.Add($"{pending.name}({pending.trend}");

                table.Rows.Add(systemFaction.name, Properties.language.ResourceManager.GetString(systemFaction.happiness), Math.Round(100 * systemFaction.influence, 2),
                    systemFaction.allegiance, systemFaction.government, string.Join(", ", activeStates), systemFaction.factionState, string.Join(", ", pendingStates));
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

            var FacDress = new List<FacDressing>();
            var dress = 0;
            foreach (var systemFaction in data.systemHistory.Last().factions)
            {
                FacDress.Add(new FacDressing{Name = systemFaction.name, Adress = dress});
                dress++;
                table.Rows.Add(systemFaction.name, systemFaction.allegiance);
            }

            dataGridView_BgsHistory.DataSource = table;
            Debug.WriteLine(dataGridView_BgsHistory.Rows.Count);
            Debug.WriteLine(dataGridView_BgsHistory.Columns.Count);
            var vday = 2;
            foreach (var hisory in data.systemHistory)
            {
                var vfac = 0;
                foreach (var fraction in hisory.factions)
                {
                    try
                    {
                        var fdress = FacDress.Find(x => string.Equals(x.Name, fraction.name, StringComparison.CurrentCultureIgnoreCase));
                        if(fdress == null)continue;
                        dataGridView_BgsHistory[vday, fdress.Adress].Value = Math.Round(100 * fraction.influence, 3);
                    }
                    catch
                    {
                        Program.Log($"Das System {data.starSystem}:{data.systemAddress} hat {hisory.factions.Count-dataGridView_BgsHistory.Rows.Count} Fraktionen zu viel.");
                    }
                    finally
                    {
                        vfac++;
                    }
                }
                vday++;
            }
            dataGridView_BgsHistory.DataBindingComplete += LoadHistoryColors;
        }
        private static DateTime ConvertTime(string timestamp)
        {
            return DateTime.ParseExact(timestamp, "yyyy-MM-dd'T'HH:mm:ss'Z'", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal).ToUniversalTime();
        }
        private void CheckEdit(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            CacheHandler.CacheOrder();
            LoadOrdersTable(CacheHandler.GetHistoryFromCache().FirstOrDefault(x => x.systemAddress == SystemAddress));
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
