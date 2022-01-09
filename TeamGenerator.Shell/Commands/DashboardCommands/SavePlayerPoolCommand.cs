using Microsoft.Win32;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using TeamGenerator.Infrastructure;
using TeamGenerator.Model;
using TeamGenerator.Shell.ViewModels;

namespace TeamGenerator.Shell.Commands
{
    internal class SavePlayerPoolCommand : CommandBase
    {
        private readonly DashboardViewModel dashboardViewModel;
        private readonly PlayerDataManager playerDataManager;

        public SavePlayerPoolCommand(DashboardViewModel dashboardViewModel, PlayerDataManager playerDataManager)
        {
            this.dashboardViewModel = dashboardViewModel;
            this.playerDataManager = playerDataManager;
        }

        public override void Execute(object parameter)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = ".tgpp";
            saveFileDialog.Title = "Save you player pool";
            string serializedPlayerPool = string.Empty;

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    serializedPlayerPool = playerDataManager.SerializePlayerPool(dashboardViewModel.AvailablePlayers.ToList<Player>());
                }
                catch (JsonException exception)
                {
                    MessageBox.Show($"Current player pool could not be saved. \nException message: \n{exception.Message}", "Saving error");
                    return;
                }

                File.WriteAllText(saveFileDialog.FileName, serializedPlayerPool);
            }
        }
    }
}
