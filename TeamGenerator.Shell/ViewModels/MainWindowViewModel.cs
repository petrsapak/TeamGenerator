using System.Windows.Input;
using System;
using System.Windows;
using System.Windows.Controls;
using TeamGenerator.Shell.Views;

namespace TeamGenerator.Shell.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
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
            CloseApplicationCommand = new Command(CloseApplication, CanCloseApplication);
            MinimizeApplicationCommand = new Command(MinimizeApplication, CanMinimizeApplication);
            SwitchViewCommand = new Command(SwitchView, CanSwitchView);
        }

        private void CloseApplication(object parameters)
        {
            Application.Current.Shutdown();
        }

        private void MinimizeApplication(object parameters)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void SwitchView(object parameters)
        {
            switch (parameters as string)
            {
                case "Dashboard":
                    CurrentView = new DashboardView();
                    break;
                case "Settings":
                    CurrentView = new SettingsView();
                    break;
                case "Statistics":
                    CurrentView = new StatisticsView();
                    break;
                case "About":
                    CurrentView = new AboutView();
                    break;
                default:
                    throw new ArgumentException($"The view name {parameters as string} is incorrect", "parameters");
            }
        }

        private bool CanMinimizeApplication(object parameters)
        {
            return true;
        }

        private bool CanCloseApplication(object parameters)
        {
            return true;
        }

        private bool CanSwitchView(object parameters)
        {
            return true;
        }

        #endregion

        #region Properties

        private string applicationTitle;
        public string ApplicationTitle
        {
            get => applicationTitle;
            set
            {
                applicationTitle = value;
                RaisePropertyChanged(nameof(ApplicationTitle));
            }
        }

        private UserControl currentView;

        public UserControl CurrentView
        {
            get => currentView;
            set
            {
                currentView = value;
                RaisePropertyChanged(nameof(CurrentView));
            }
        }

        #endregion
    }
}
