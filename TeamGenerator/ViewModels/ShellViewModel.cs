using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using TeamGenerator.Commands;
using Prism.Regions;
using Prism.Ioc;

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

        public ICommand CloseApplicationCommand { get; set; }
        public ICommand MinimizeApplicationCommand { get; set; }

        private void InitializeCommands()
        {
            CloseApplicationCommand = new CloseApplicationCommand(Application.Current);
            MinimizeApplicationCommand = new MinimizeApplicationCommand(Application.Current);
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
