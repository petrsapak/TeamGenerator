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

        private UserControl dashboardView;
        private UserControl aboutView;

        public MainMenuViewModel(IRegionManager regionManager, IContainerExtension container)
        {
            this.regionManager = regionManager;
            this.container = container;
            InitializeCommands();
            dashboardView = container.Resolve<DashboardView>();
            aboutView = container.Resolve<AboutView>();
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
            IRegion contentRegion = regionManager.Regions["ContentRegion"];
            contentRegion.Add(dashboardView);
            contentRegion.Activate(dashboardView);
        }

        private void NavigateToStatisticsRegion()
        {
            UserControl view = container.Resolve<StatisticsView>();
            IRegion contentRegion = regionManager.Regions["ContentRegion"];
            contentRegion.Add(view);
            contentRegion.Activate(view);
        }

        private void NavigateToSettingsRegion()
        {
            UserControl view = container.Resolve<SettingsView>();
            IRegion contentRegion = regionManager.Regions["ContentRegion"];
            contentRegion.Add(view);
            contentRegion.Activate(view);
        }

        private void NavigateToAboutRegion()
        {
            IRegion contentRegion = regionManager.Regions["ContentRegion"];
            contentRegion.Add(aboutView);
            contentRegion.Activate(aboutView);
        }

        #endregion
    }
}
