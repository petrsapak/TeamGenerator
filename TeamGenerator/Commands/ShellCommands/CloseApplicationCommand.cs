using System.Windows;

namespace TeamGenerator.Commands
{
    internal class CloseApplicationCommand : CommandBase
    {
        Application application;

        public CloseApplicationCommand(Application application)
        {
            this.application = application;
        }

        public override void Execute(object parameter) => application.Shutdown();
    }
}
