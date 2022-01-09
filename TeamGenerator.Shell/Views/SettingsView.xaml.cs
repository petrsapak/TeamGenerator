using System.Windows.Controls;
using TeamGenerator.Shell.ViewModels;

namespace TeamGenerator.Shell.Views
{
    public partial class SettingsView : UserControl
    {
        public SettingsView()
        {
            DataContext = new SettingsViewModel();
            InitializeComponent();
        }
    }
}
