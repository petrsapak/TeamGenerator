using System.Windows;
using System.Windows.Controls;
using Prism.Regions;
using Prism.Ioc;
using Prism.Commands;

namespace TeamGenerator.ViewModels
{
    internal class ShellViewModel : ViewModelBase
    {
        private readonly IRegionManager regionManager;
        private readonly IContainerExtension container;

        public ShellViewModel(IRegionManager regionManager, IContainerExtension container)
        {
            this.regionManager = regionManager;
            this.container = container;
            InitializeCommands();
            ApplicationTitle = "Team Generator";
        }

        #region Commands

        public DelegateCommand CloseApplicationCommand { get; private set; }
        public DelegateCommand MinimizeApplicationCommand { get; private set; }

        private void InitializeCommands()
        {
            CloseApplicationCommand = new DelegateCommand(CloseApplication);
            MinimizeApplicationCommand = new DelegateCommand(MinimizeApplication);
        }

        private void CloseApplication()
        {
            Application.Current.Shutdown();
        }

        private void MinimizeApplication()
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        #endregion

        #region Properties

        private string applicationTitle;
        public string ApplicationTitle
        {
            get => applicationTitle;
            set => SetProperty(ref applicationTitle, value);
        }

        private UserControl currentView;
        public UserControl CurrentView
        {
            get => currentView;
            set => SetProperty(ref currentView, value);
        }

        #endregion
    }
}
