using System.Windows;

namespace TeamGenerator.Shell.Commands
{
    internal class MinimizeApplicationCommand : CommandBase
    {
        Application application;

        public MinimizeApplicationCommand(Application application)
        {
            this.application = application;
        }

        public override void Execute(object parameter) => application.MainWindow.WindowState = WindowState.Minimized;
    }
}
