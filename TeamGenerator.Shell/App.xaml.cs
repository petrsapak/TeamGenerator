using System.Windows;
using TeamGenerator.Shell.ViewModels;
using TeamGenerator.Shell.Views;

namespace TeamGenerator.Shell
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow = new MainWindow()
            {
                DataContext = new MainWindowViewModel()
            };
            MainWindow.Show();

            base.OnStartup(e);
        }
    }
}
