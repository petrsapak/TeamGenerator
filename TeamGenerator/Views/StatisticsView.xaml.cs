using System.Windows.Controls;
using TeamGenerator.ViewModels;

namespace TeamGenerator.Views
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
