using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using TeamGenerator.Views;
using TeamGenerator.Commands;

namespace TeamGenerator.ViewModels
{
    internal class ShellViewModel : ViewModelBase
    {
        public ShellViewModel()
        {
            InitializeCommands();
            ApplicationTitle = "Team Generator";
            CurrentView = new DashboardView();
        }

        #region Commands

        public ICommand CloseApplicationCommand { get; set; }
        public ICommand MinimizeApplicationCommand { get; set; }
        public ICommand SwitchViewCommand { get; set; }

        private void InitializeCommands()
        {
            CloseApplicationCommand = new CloseApplicationCommand(Application.Current);
            MinimizeApplicationCommand = new MinimizeApplicationCommand(Application.Current);
            SwitchViewCommand = new SwitchViewCommand(this);
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
