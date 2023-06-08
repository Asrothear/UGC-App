using System.Diagnostics;
using UGC_App.Order.DashViews;

namespace UGC_App.Order
{
    public partial class Dashboard : Form
    {
        private int views = 0;
        public Dashboard()
        {
            InitializeComponent();
            TopMost = Config.Instance.AlwaysOnTop;
            refreshToolStripMenuItem.Click += (sender, args) => RefreshView();
            ansichtWechselnToolStripMenuItem.Click += (sender, args) => SwitchView();
        }

        private void SwitchView()
        {
            var def = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            switch (views)
            {
                case 0:
                    break;
                case 1:
                    var sys = Controls.Find("SystemList", true);
                    if (sys.Any()) foreach (var v in sys) Controls.Remove(v);
                    AttachView(new OrderList());
                    Refresh();
                    break;
                case 2:
                    var ord = Controls.Find("OrderList", true);
                    if (ord.Any()) foreach (var v in ord) Controls.Remove(v);
                    AttachView(new SystemList());
                    Refresh();
                    break;
            }
            Cursor.Current = def;
        }
        private void RefreshView()
        {
            switch (views)
            {
                case 0:
                    break;
                case 1:
                    var sys = Controls.Find("SystemList", true);
                    if (sys.Any()) foreach (var v in sys) Controls.Remove(v);
                    Controls.Remove(sys.First());
                    AttachView(new SystemList());
                    Refresh();
                    break;
                case 2:
                    var ord = Controls.Find("OrderList", true);
                    if (ord.Any()) foreach (var v in ord) Controls.Remove(v);
                    AttachView(new OrderList());
                    Refresh();
                    break;
            }
        }
        internal void AttachView(dynamic view)
        {
            Control[] vie;
            switch (view.GetType())
            {
                case Type needle when needle == typeof(SystemList):
                    if (!Controls.Contains(view)) Controls.Add(view);
                    vie = Controls.Find("SystemList", true);
                    Size = Size with { Width = view.Size.Width };
                    views = 1;
                    vie.First().Show();

                    break;
                case Type needle when needle == typeof(OrderList):
                    if (!Controls.Contains(view)) Controls.Add(view);
                    vie = Controls.Find("OrderList", true);
                    views = 2;
                    vie.First().Show();
                    break;
            }
        }
    }
}