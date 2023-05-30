using UGC_App.Order.DashViews;

namespace UGC_App.Order
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
        }
        internal void AttachView(dynamic view)
        {
            switch (view.GetType())
            {
                case Type needle when needle == typeof(SystemList):
                    Controls.Add(view);
                    Height = view.dataGridView_SystemList.Height + view.menuStrip1.Height + 5;
                    break;
            }
        }
    }
}
