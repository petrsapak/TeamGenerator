using System.Windows.Controls;
using TeamGenerator.ViewModels;

namespace TeamGenerator.Views
{
    public partial class DashboardView : UserControl
    {
        public DashboardView()
        {
            DataContext = new DashboardViewModel();
            InitializeComponent();
        }
    }
}
