using System.Windows.Controls;
using TeamGenerator.Shell.ViewModels;

namespace TeamGenerator.Shell.Views
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
