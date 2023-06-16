using System.Diagnostics;
using UGC_App.LocalCache;
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

        internal void SwitchView(dynamic? view = null)
        {
            var def = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            if (view?.GetType() == typeof(SystemList))
            {
                views = 2;
                
            }
            else if (view?.GetType() == typeof(OrderList))
            {
                views = 1;
            }

            switch (views)
            {
                case 0:
                    break;
                case 1:
                    var sys = Controls.Find("SystemList", true);
                    if (sys.Any()) foreach (var v in sys) Controls.Remove(v);
                    AttachView(new OrderList(this));
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
        internal void RefreshView()
        {
            CacheHandler.InitAll();
            switch (views)
            {
                case 0:
                    break;
                case 1:
                    var sys = Controls.Find("SystemList", true);
                    if (sys.Any()) foreach (var v in sys) Controls.Remove(v);
                    Controls.Remove(sys.First());
                    CacheHandler.CacheSystemList(true);
                    AttachView(new SystemList());
                    Refresh();
                    break;
                case 2:
                    var ord = Controls.Find("OrderList", true);
                    if (ord.Any()) foreach (var v in ord) Controls.Remove(v);
                    CacheHandler.CacheOrder(true);
                    AttachView(new OrderList(this));
                    Refresh();
                    break;
            }
        }
        private void AttachView(dynamic view)
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