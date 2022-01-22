using Microsoft.Win32;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using TeamGenerator.Infrastructure.Events;
using TeamGenerator.Infrastructure.Services;
using TeamGenerator.Model;

namespace TeamGenerator.ViewModels
{
    internal class StatisticsViewModel : ViewModelBase
    {
        private readonly IStatusMessageService statusMessageService;
        private readonly IDataService<List<Match>> matchDataService;
        private readonly IEventAggregator eventAggregator;

        public StatisticsViewModel(IContainerProvider container, IEventAggregator eventAggregator)
        {
            InitializeCommands();
            Matches = new ObservableCollection<Match>();

            this.eventAggregator = eventAggregator;
            statusMessageService = container.Resolve<IStatusMessageService>();
            matchDataService = container.Resolve<IDataService<List<Match>>>();

            eventAggregator.GetEvent<SaveMatchStatisticsEvent>().Subscribe(AddMatch);
        }

        private void AddMatch(Match match)
        {
            Matches.Add(match);
        }

        #region Properties

        private Match selectedMatch;

        public Match SelectedMatch
        {
            get => selectedMatch;
            set => SetProperty(ref selectedMatch, value);
        }

        private ObservableCollection<Match> matches;

        public ObservableCollection<Match> Matches 
        {
            get => matches;
            set => SetProperty(ref matches, value);
        }

        #endregion

        #region Commands

        private void InitializeCommands()
        {
            RemoveMatchCommand = new DelegateCommand(RemoveMatch, CanExecuteRemoveMatch);
            LoadMatchesCommand = new DelegateCommand(LoadMatches);
            SaveMatchesCommand = new DelegateCommand(SaveMatches);
        }

        public DelegateCommand RemoveMatchCommand { get; private set; }
        public DelegateCommand LoadMatchesCommand { get; private set; }
        public DelegateCommand SaveMatchesCommand { get; private set; }


        private void RemoveMatch()
        {
            Guid matchId = SelectedMatch.Id;
            Matches.Remove(SelectedMatch);

            statusMessageService.UpdateStatusMessage($"Match {matchId} removed.");
        }

        private bool CanExecuteRemoveMatch()
        {
            return SelectedMatch != null;
        }

        private void LoadMatches()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = ".tgst";
            openFileDialog.Title = "Select your saved match statistics";
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
                Matches = new ObservableCollection<Match>(matchDataService.DeserializeData(selectedFileContent));
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

            statusMessageService.UpdateStatusMessage($"Matches loaded.");
        }

        private void SaveMatches()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = ".tgst";
            saveFileDialog.Title = "Save you player pool";
            string serializedPlayerPool = string.Empty;

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    serializedPlayerPool = matchDataService.SerializeData(Matches.ToList<Match>());
                }
                catch (JsonException exception)
                {
                    MessageBox.Show($"Current player pool could not be saved. \nException message: \n{exception.Message}", "Saving error");
                    return;
                }

                File.AppendAllText(saveFileDialog.FileName, serializedPlayerPool);

                statusMessageService.UpdateStatusMessage($"Matches saved at {saveFileDialog.FileName}.");
            }
        }

        #endregion

    }
}
