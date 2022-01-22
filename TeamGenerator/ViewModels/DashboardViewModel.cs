using System.Collections.Generic;
using System.Collections.ObjectModel;
using TeamGenerator.Model;
using Prism.Commands;
using System.Linq;
using System;
using TeamGenerator.Core.Interfaces;
using Prism.Ioc;
using Microsoft.Win32;
using System.IO;
using System.Text.Json;
using System.Windows;
using Prism.Events;
using TeamGenerator.Infrastructure.Services;
using TeamGenerator.Infrastructure.Events;

namespace TeamGenerator.ViewModels
{
    internal class DashboardViewModel : ViewModelBase
    {
        private readonly IGenerate generator;
        private readonly IEvaluate evaluator;
        private readonly IStatusMessageService statusMessageService;
        private readonly IDataService<List<Player>> playerDataService;
        private readonly IStatisticsDataService statisticsDataService;
        private readonly IEventAggregator eventAggregator;

        public DashboardViewModel(IContainerProvider container, IEventAggregator eventAggregator)
        {
            InitializeCommands();

            this.eventAggregator = eventAggregator;
            generator = container.Resolve<IGenerate>();
            evaluator = container.Resolve<IEvaluate>();
            statusMessageService = container.Resolve<IStatusMessageService>();
            playerDataService = container.Resolve<IDataService<List<Player>>>();
            statisticsDataService = container.Resolve<IStatisticsDataService>();

            eventAggregator.GetEvent<UpdateRanksEvent>().Subscribe(UpdateRanks);

            PlayerPool = new ObservableCollection<Player>();
            Ranks = new List<Rank>
            {
                new Rank("Silver 1", 1),
                new Rank("Silver 2", 2),
                new Rank("Silver 3", 3),
                new Rank("Silver 4", 4),
                new Rank("Silver Master", 5),
                new Rank("Silver Master Elite", 6),
                new Rank("Golden Nova 1", 7),
                new Rank("Golden Nova 2", 8),
                new Rank("Golden Nova 3", 9),
                new Rank("Golden Nova Master", 10),
                new Rank("Master Guardian 1", 11),
                new Rank("Master Guardian 2", 12),
                new Rank("Master Guardian Elite", 13),
                new Rank("Distinguished Master Guardian", 14),
                new Rank("Legendary Eagle", 15),
                new Rank("Legendary Eagle Master", 16),
                new Rank("Supreme Master First Class", 17),
                new Rank("Global Elite", 18),
            };
            MaxPlayerCount = "10";
            NewPlayerRank = Ranks[0];
        }

        private void UpdateRanks(List<Rank> updatedRanks)
        {
            PlayerPool.Clear();
            NewPlayerName = null;
            NewPlayerRank = null;
            Ranks = updatedRanks;
        }

        #region Properties

        private string newPlayerName;

        public string NewPlayerName
        {
            get => newPlayerName;
            set
            {
                SetProperty(ref newPlayerName, value);
                AddAvailablePlayerCommand.RaiseCanExecuteChanged();
            }
        }

        private Rank newPlayerRank;

        public Rank NewPlayerRank
        {
            get => newPlayerRank;
            set
            {
                SetProperty(ref newPlayerRank, value);
                AddAvailablePlayerCommand.RaiseCanExecuteChanged();
            }
        }

        private Player selectedAvailablePlayer;

        public Player SelectedAvailablePlayer
        {
            get => selectedAvailablePlayer;
            set => SetProperty(ref selectedAvailablePlayer, value);
        }

        private bool fillWithBots;

        public bool FillWithBots
        {
            get => fillWithBots;
            set => SetProperty(ref fillWithBots, value);
        }

        private string maxPlayerCount;

        public string MaxPlayerCount 
        {
            get => maxPlayerCount;
            set => SetProperty(ref maxPlayerCount, value);
        }

        private ObservableCollection<Player> playerPool;

        public ObservableCollection<Player> PlayerPool
        {
            get => playerPool;
            set => SetProperty(ref playerPool, value);
        }

        private ObservableCollection<Player> team2;

        public ObservableCollection<Player> Team2
        {
            get => team2;
            set => SetProperty(ref team2, value);
        }

        private ObservableCollection<Player> team1;

        public ObservableCollection<Player> Team1
        {
            get => team1;
            set => SetProperty(ref team1, value);
        }

        private int team2Probability;

        public int Team2Probability
        {
            get => team2Probability;
            set => SetProperty(ref team2Probability, value);
        }

        private int team1Probability;

        public int Team1Probability
        {
            get => team1Probability;
            set => SetProperty(ref team1Probability, value);
        }

        private List<Rank> ranks;

        public List<Rank> Ranks
        {
            get => ranks;
            set => SetProperty(ref ranks, value);
        }

        #endregion

        #region Commands

        private void InitializeCommands()
        {
            AddAvailablePlayerCommand = new DelegateCommand(AddAvailablePlayer, CanExecuteAddAvailablePlayer);
            DeleteAvailablePlayerCommand = new DelegateCommand(DeleteAvailablePlayer, CanExecuteDeleteAvailablePlayer);
            GenerateTeamsCommand = new DelegateCommand(GenerateTeams, CanExecuteGenerateTeams);
            LoadPlayerPoolCommand = new DelegateCommand(LoadPlayerPool);
            SavePlayerPoolCommand = new DelegateCommand(SavePlayerPool);
        }

        public DelegateCommand AddAvailablePlayerCommand { get; private set; }
        public DelegateCommand GenerateTeamsCommand { get; private set; }
        public DelegateCommand DeleteAvailablePlayerCommand { get; private set; }
        public DelegateCommand LoadPlayerPoolCommand { get; private set; }
        public DelegateCommand SavePlayerPoolCommand { get; private set; }

        private void AddAvailablePlayer()
        {
            Player availablePlayer = new Player(nick: NewPlayerName, rank: NewPlayerRank);
            PlayerPool.Add(availablePlayer);
            GenerateTeamsCommand.RaiseCanExecuteChanged();

            statusMessageService.UpdateStatusMessage($"Player {NewPlayerName} added.");
        }

        private bool CanExecuteAddAvailablePlayer()
        {
            return PlayerPool.All(player => player.Nick != NewPlayerName) &&
                                       !string.IsNullOrEmpty(NewPlayerName) &&
                                       NewPlayerName.Any(char.IsLetterOrDigit) &&
                                       NewPlayerRank != null &&
                                       !string.IsNullOrEmpty(NewPlayerRank.Name);
        }

        private void DeleteAvailablePlayer()
        {
            string name = selectedAvailablePlayer.Nick;
            PlayerPool.Remove(SelectedAvailablePlayer);

            AddAvailablePlayerCommand.RaiseCanExecuteChanged();
            statusMessageService.UpdateStatusMessage($"Player {name} removed.");
        }

        private bool CanExecuteDeleteAvailablePlayer()
        {
            return SelectedAvailablePlayer != null;
        }

        private void GenerateTeams()
        {
            int maxPlayerCountInt = int.Parse(MaxPlayerCount);

            (Team, Team) teams = generator.GenerateTeams(PlayerPool, FillWithBots, maxPlayerCountInt);

            Team1 = new ObservableCollection<Player>(teams.Item1.Players);
            Team2 = new ObservableCollection<Player>(teams.Item2.Players);

            Match match = new Match()
            {
                Team1 = teams.Item1,
                Team2 = teams.Item2
            };

            double counterTerroristTeamEvaluation = evaluator.EvaluateTeam(teams.Item1);
            double terroristTeamEvaluation = evaluator.EvaluateTeam(teams.Item2);
            double sumOfEvaluations = counterTerroristTeamEvaluation + terroristTeamEvaluation;
            double evaluationPointToPercent = (double)100 / (double)sumOfEvaluations;
            double counterTerroristsChanceOfWinning = (int)Math.Round(counterTerroristTeamEvaluation * evaluationPointToPercent);
            double terroristsChanceOfWinning = (int)Math.Round(terroristTeamEvaluation * evaluationPointToPercent);

            Team1Probability = (int)counterTerroristsChanceOfWinning;
            Team2Probability = (int)terroristsChanceOfWinning;

            match.Team1Probability = Team1Probability;
            match.Team2Probability = Team2Probability;

            statisticsDataService.SaveMatchStatistics(match);
            statusMessageService.UpdateStatusMessage($"Teams generated.");
        }

        private bool CanExecuteGenerateTeams()
        {
            return PlayerPool.Count() > 1;
        }

        private void LoadPlayerPool()
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
                PlayerPool = new ObservableCollection<Player>(playerDataService.DeserializeData(selectedFileContent));
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

            GenerateTeamsCommand.RaiseCanExecuteChanged();

            statusMessageService.UpdateStatusMessage($"Players loaded.");
        }

        private void SavePlayerPool()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = ".tgpp";
            saveFileDialog.Title = "Save you player pool";
            string serializedPlayerPool = string.Empty;

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    serializedPlayerPool = playerDataService.SerializeData(PlayerPool.ToList<Player>());
                }
                catch (JsonException exception)
                {
                    MessageBox.Show($"Current player pool could not be saved. \nException message: \n{exception.Message}", "Saving error");
                    return;
                }

                File.WriteAllText(saveFileDialog.FileName, serializedPlayerPool);

                statusMessageService.UpdateStatusMessage($"Players saved at {saveFileDialog.FileName}.");
            }
        }

        #endregion
    }
}
