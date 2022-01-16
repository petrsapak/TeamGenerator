using System.Windows;
using System.Windows.Controls;
using Prism.Regions;
using Prism.Ioc;
using Prism.Commands;
using Prism.Events;
using TeamGenerator.Infrastructure.Events;

namespace TeamGenerator.ViewModels
{
    internal class ShellViewModel : ViewModelBase
    {
        private readonly IRegionManager regionManager;
        private readonly IContainerExtension container;
        private readonly IEventAggregator eventAggregator;

        public ShellViewModel(IRegionManager regionManager, IContainerExtension container, IEventAggregator eventAggregator)
        {
            this.regionManager = regionManager;
            this.container = container;
            this.eventAggregator = eventAggregator;

            InitializeCommands();

            eventAggregator.GetEvent<UpdateStatusMessageEvent>().Subscribe(UpdateStatusMessage);

            ApplicationTitle = "Team Generator";
            StatusMessage = "The application started.";
        }

        private void UpdateStatusMessage(string newMessage)
        {
            StatusMessage = newMessage;
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

        private string statusMessage;

        public string StatusMessage
        {
            get => statusMessage;
            set
            {
                //The message doesn't show up if it is the same as the previous one. There's some issue in the animation binding.
                //Setting it to strin.empty is a workaround.
                statusMessage = string.Empty;
                SetProperty(ref statusMessage, value);
            }
        }

        #endregion
    }
}
