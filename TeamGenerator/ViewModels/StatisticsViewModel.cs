using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using TeamGenerator.Infrastructure.Events;
using TeamGenerator.Infrastructure.Services;
using TeamGenerator.Model;
using TeamGenerator.Services;

namespace TeamGenerator.ViewModels
{
    internal class StatisticsViewModel : ViewModelBase
    {
        private readonly IStatusMessageService statusMessageService;
        private readonly IDataService<List<Match>> matchDataService;
        private readonly IEventAggregator eventAggregator;
        private readonly SaveFileDialogService saveFileDialogService;
        private readonly OpenFileDialogService openFileDialogService;

        public StatisticsViewModel(IContainerProvider container, IEventAggregator eventAggregator)
        {
            InitializeCommands();
            Matches = new ObservableCollection<Match>();

            this.eventAggregator = eventAggregator;
            statusMessageService = container.Resolve<IStatusMessageService>();
            matchDataService = container.Resolve<IDataService<List<Match>>>();
            saveFileDialogService = container.Resolve<SaveFileDialogService>();
            openFileDialogService = container.Resolve<OpenFileDialogService>();

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
            UpdatePoolCommand = new DelegateCommand(UpdatePool);
            ClearMatchesCommand = new DelegateCommand(ClearMatches);
        }

        public DelegateCommand RemoveMatchCommand { get; private set; }
        public DelegateCommand LoadMatchesCommand { get; private set; }
        public DelegateCommand SaveMatchesCommand { get; private set; }
        public DelegateCommand UpdatePoolCommand { get; private set; }
        public DelegateCommand ClearMatchesCommand { get; private set; }

        private void UpdatePool()
        {
            RankUpdateService rankUpdateService = new RankUpdateService();
            rankUpdateService.ProposeRankUpdates(Matches.ToList());
        }

        private void ClearMatches()
        {
            Matches.Clear();
            statusMessageService.UpdateStatusMessage($"All matches removed.");
        }

        private void RemoveMatch()
        {
            Matches.Remove(SelectedMatch);
            statusMessageService.UpdateStatusMessage($"Match removed.");
        }

        private bool CanExecuteRemoveMatch()
        {
            return SelectedMatch != null;
        }

        private void LoadMatches()
        {
            bool loadSuccessful = false;
            string serializedMatches = openFileDialogService.ShowOpenFileDialog("Load statistics", ".tgst");

            if (string.IsNullOrEmpty(serializedMatches)) return;

            try
            {
                Matches = new ObservableCollection<Match>(matchDataService.DeserializeData(serializedMatches));
                loadSuccessful = true;
            }
            catch (Exception exception)
            {
                if (exception is JsonException)
                {
                    statusMessageService.UpdateStatusMessage($"The selected file could not be loaded.");
                }
                else if (exception is FileNotFoundException)
                {
                    statusMessageService.UpdateStatusMessage($"The selected file could not be found.");
                }
                else if (exception is ArgumentException)
                {
                    statusMessageService.UpdateStatusMessage($"An Exception occured while trying to load the player pool.");
                }
            }

            if (loadSuccessful)
                statusMessageService.UpdateStatusMessage($"Matches loaded.");
            else
                statusMessageService.UpdateStatusMessage($"Error while loading matches.");
        }

        private void SaveMatches()
        {
            try
            {
                string serializedMatches = matchDataService.SerializeData(Matches.ToList<Match>());
                saveFileDialogService.ShowSaveFileDialog("Save the matches", ".tgst", serializedMatches);
            }
            catch (JsonException exception)
            {
                statusMessageService.UpdateStatusMessage("Error while serializing matches.");
            }
        }

        #endregion

    }
}
