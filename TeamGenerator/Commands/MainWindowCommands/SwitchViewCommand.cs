using System;
using System.Windows.Controls;
using TeamGenerator.ViewModels;
using TeamGenerator.Views;

namespace TeamGenerator.Commands
{
    internal class SwitchViewCommand : CommandBase
    {
        private readonly ShellViewModel mainWindowViewModel;

        public SwitchViewCommand(ShellViewModel mainWindowViewModel)
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
