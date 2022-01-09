using TeamGenerator.Shell.ViewModels;

namespace TeamGenerator.Shell.Commands
{
    internal class DeleteAvailablePlayerCommand : CommandBase
    {
        private readonly DashboardViewModel dashboardViewModel;

        public DeleteAvailablePlayerCommand(DashboardViewModel dashboardViewModel)
        {
            this.dashboardViewModel = dashboardViewModel;
        }

        public override void Execute(object parameter)
        {
            dashboardViewModel.AvailablePlayers.Remove(dashboardViewModel.SelectedAvailablePlayer);
        }

        public override bool CanExecute(object parameter)
        {
            return dashboardViewModel.SelectedAvailablePlayer != null;
        }
    }
}
