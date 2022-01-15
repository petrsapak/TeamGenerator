using System.Windows.Controls;
using TeamGenerator.ViewModels;

namespace TeamGenerator.Views
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
