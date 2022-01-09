using System;
using System.Windows.Controls;
using TeamGenerator.Shell.ViewModels;
using TeamGenerator.Shell.Views;

namespace TeamGenerator.Shell.Commands
{
    internal class SwitchViewCommand : CommandBase
    {
        private readonly MainWindowViewModel mainWindowViewModel;

        public SwitchViewCommand(MainWindowViewModel mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
        }

        public override void Execute(object parameter)
        {
            UserControl currentView;

            switch (parameter as string)
            {
                case "Dashboard":
                    currentView = new DashboardView();
                    break;
                case "Settings":
                    currentView = new SettingsView();
                    break;
                case "Statistics":
                    currentView = new StatisticsView();
                    break;
                case "About":
                    currentView = new AboutView();
                    break;
                default:
                    throw new ArgumentException($"The view name {parameter as string} is incorrect", "parameters");
            }

            mainWindowViewModel.CurrentView = currentView;
        }
    }
}
