using System.Windows.Controls;
using TeamGenerator.Shell.ViewModels;

namespace TeamGenerator.Shell.Views
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
