using System.Linq;
using TeamGenerator.Model;
using TeamGenerator.Shell.ViewModels;

namespace TeamGenerator.Shell.Commands
{
    internal class AddAvailablePlayerCommand : CommandBase
    {
        private readonly DashboardViewModel dashboardViewModel;

        public AddAvailablePlayerCommand(DashboardViewModel dashboardViewModel)
        {
            this.dashboardViewModel = dashboardViewModel;
        }

        public override void Execute(object parameter)
        {
            Player availablePlayer = new Player(nick: dashboardViewModel.NewPlayerName, rank: dashboardViewModel.NewPlayerRank);
            dashboardViewModel.AvailablePlayers.Add(availablePlayer);
        }

        public override bool CanExecute(object parameter)
        {
            return dashboardViewModel.AvailablePlayers.All(player => player.Nick != dashboardViewModel.NewPlayerName) &&
                                                              !string.IsNullOrEmpty(dashboardViewModel.NewPlayerName) &&
                                                              dashboardViewModel.NewPlayerName.Any(char.IsLetterOrDigit);
        }
    }
}
