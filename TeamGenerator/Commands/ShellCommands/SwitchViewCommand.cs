using Prism.Commands;
using Prism.Regions;
using System;
using System.Windows.Controls;
using TeamGenerator.ViewModels;
using TeamGenerator.Views;

namespace TeamGenerator.Commands
{
    internal class SwitchViewCommand : CommandBase 
    {
        private readonly IRegionManager regionManager;

        public SwitchViewCommand(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
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

        }
    }
}
