using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Windows;
using TeamGenerator.Infrastructure;
using TeamGenerator.Model;
using TeamGenerator.Shell.ViewModels;

namespace TeamGenerator.Shell.Commands
{
    internal class LoadPlayerPoolCommand : CommandBase
    {
        private readonly DashboardViewModel dashboardViewModel;
        private readonly PlayerDataManager playerDataManager;

        public LoadPlayerPoolCommand(DashboardViewModel dashboardViewModel, PlayerDataManager playerDataManager)
        {
            this.dashboardViewModel = dashboardViewModel;
            this.playerDataManager = playerDataManager;
        }

        public override void Execute(object parameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = ".tgpp";
            openFileDialog.Title = "Select your saved player pool";
            string selectedFileContent = string.Empty;

            if (openFileDialog.ShowDialog() == true)
            {
                selectedFileContent = File.ReadAllText(openFileDialog.FileName);
            }
            else
            {
                return;
            }

            try
            {
                dashboardViewModel.AvailablePlayers = new ObservableCollection<Player>(playerDataManager.DeserializePlayerPool(selectedFileContent));
            }
            catch (JsonException exception)
            {
                MessageBox.Show($"The selected file could not be loaded.\nException message: \n{exception.Message}", "Loading error");
            }
            catch (ArgumentNullException argumentNullException)
            {
                MessageBox.Show($"An exception occured while trying to load the player pool. Your player pool file contains invalid data.\nException message: \n{argumentNullException.Message}", "Loading error");
            }
            catch (ArgumentException argumentException)
            {
                MessageBox.Show($"An Exception occured while trying to load the player pool. Your player pool file contains invalid data.\nException message: \n{argumentException.Message}", "Loading error");
            }
        }
    }
}
