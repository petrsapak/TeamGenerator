using System.Windows.Controls;
using TeamGenerator.Shell.ViewModels;

namespace TeamGenerator.Shell.Views
{
    public partial class StatisticsView : UserControl
    {
        public StatisticsView()
        {
            DataContext = new StatisticsViewModel();
            InitializeComponent();
        }
    }
}
