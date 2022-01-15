using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;
using System.Windows.Controls;
using TeamGenerator.Views;

namespace TeamGenerator.ViewModels
{
    internal class MainMenuViewModel : ViewModelBase 
    {
        private readonly IRegionManager regionManager;
        private readonly IContainerExtension container;

        private readonly IRegion contentRegion;

        private UserControl dashboardView;
        private UserControl aboutView;
        private UserControl statisticsView;
        private UserControl settingsView;

        public MainMenuViewModel(IRegionManager regionManager, IContainerExtension container)
        {
            this.regionManager = regionManager;
            this.container = container;
            InitializeCommands();
            ResolveViews();

            contentRegion = regionManager.Regions["ContentRegion"];

            contentRegion.Add(dashboardView);
            contentRegion.Add(aboutView);
            contentRegion.Add(statisticsView);
            contentRegion.Add(settingsView);
        }

        private void ResolveViews()
        {
            dashboardView = container.Resolve<DashboardView>();
            aboutView = container.Resolve<AboutView>();
            settingsView = container.Resolve<SettingsView>();
            statisticsView = container.Resolve<StatisticsView>();
        }

        #region Commands

        public DelegateCommand NavigateToDashboardCommand { get; private set; }
        public DelegateCommand NavigateToStatisticsCommand { get; private set; }
        public DelegateCommand NavigateToSettingsCommand { get; private set; }
        public DelegateCommand NavigateToAboutCommand { get; private set; }

        private void InitializeCommands()
        {
            NavigateToDashboardCommand = new DelegateCommand(NavigateToDashboardRegion);
            NavigateToStatisticsCommand = new DelegateCommand(NavigateToStatisticsRegion);
            NavigateToSettingsCommand = new DelegateCommand(NavigateToSettingsRegion);
            NavigateToAboutCommand = new DelegateCommand(NavigateToAboutRegion);
        }

        private void NavigateToDashboardRegion()
        {
            contentRegion.Activate(dashboardView);
        }

        private void NavigateToStatisticsRegion()
        {
            contentRegion.Activate(statisticsView);
        }

        private void NavigateToSettingsRegion()
        {
            contentRegion.Activate(settingsView);
        }

        private void NavigateToAboutRegion()
        {
            contentRegion.Activate(aboutView);
        }

        #endregion
    }
}
