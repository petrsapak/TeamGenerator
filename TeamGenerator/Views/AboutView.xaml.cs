using System.Windows.Controls;
using TeamGenerator.ViewModels;

namespace TeamGenerator.Views
{
    public partial class AboutView : UserControl
    {
        public AboutView()
        {
            DataContext = new AboutViewModel();
            InitializeComponent();
        }
    }
}
