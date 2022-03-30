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
using TeamGenerator.Services;
using TeamGenerator.Core;

namespace TeamGenerator.ViewModels
{
    internal class DashboardViewModel : ViewModelBase
    {
        private readonly IGenerate generator;
        private readonly IEvaluate evaluator;
        private readonly IStatusMessageService statusMessageService;
        private readonly IDataService<DataHelper> dataService;
        private readonly IStatisticsDataService statisticsDataService;
        private readonly IEventAggregator eventAggregator;
        private Match match;

        public DashboardViewModel(IContainerProvider container, IEventAggregator eventAggregator)
        {
            InitializeCommands();

            this.eventAggregator = eventAggregator;
            generator = container.Resolve<IGenerate>();
            evaluator = container.Resolve<IEvaluate>();
            statusMessageService = container.Resolve<IStatusMessageService>();
            dataService = container.Resolve<IDataService<DataHelper>>();
            statisticsDataService = container.Resolve<IStatisticsDataService>();

            eventAggregator.GetEvent<UpdateRanksEvent>().Subscribe(UpdateRanks);

            PlayerPool = new ObservableCollection<Player>();
            Ranks = new List<Rank>
            {
                new Rank("1", 1),
                new Rank("2", 2),
                new Rank("3", 3),
                new Rank("4", 4),
                new Rank("5", 5),
                new Rank("6", 6),
                new Rank("7", 7),
                new Rank("8", 8),
                new Rank("9", 9),
                new Rank("10", 10),
                new Rank("11", 11),
                new Rank("12", 12),
                new Rank("13", 13),
                new Rank("14", 14),
                new Rank("15", 15),
                new Rank("16", 16),
                new Rank("17", 17),
                new Rank("18", 18),
                new Rank("19", 19),
                new Rank("20", 20),
            };
            MaxPlayerCount = "10";
            Team1Score = 0;
            Team2Score = 0;
            match = new Match();
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

        private int team1Score;

        public int Team1Score
        {
            get => team1Score;
            set => SetProperty(ref team1Score, value);
        }

        private int team2Score;

        public int Team2Score
        {
            get => team2Score;
            set => SetProperty(ref team2Score, value);
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
            SaveResultCommand = new DelegateCommand(SaveResult);
        }

        public DelegateCommand AddAvailablePlayerCommand { get; private set; }
        public DelegateCommand GenerateTeamsCommand { get; private set; }
        public DelegateCommand DeleteAvailablePlayerCommand { get; private set; }
        public DelegateCommand LoadPlayerPoolCommand { get; private set; }
        public DelegateCommand SavePlayerPoolCommand { get; private set; }
        public DelegateCommand SaveResultCommand { get; private set; }

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

            IGeneratorSettings settings = new GeneratorSettings()
            {
                AvailablePlayerPool = PlayerPool,
                UseBots = FillWithBots,
                BotQuotient = 0.5,
                MaxBotCount = maxPlayerCountInt
            };

            (Team, Team) teams = generator.GenerateTeams(settings);

            Team1 = new ObservableCollection<Player>(teams.Item1.Players);
            Team2 = new ObservableCollection<Player>(teams.Item2.Players);

            match = new Match()
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

            statusMessageService.UpdateStatusMessage($"Teams generated.");
        }

        private bool CanExecuteGenerateTeams()
        {
            return PlayerPool.Count() > 1;
        }

        private void LoadPlayerPool()
        {
            bool loadSuccessful = true;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = ".tgpp";
            openFileDialog.Title = "Select your saved player pool";
            string selectedFileContent = string.Empty;
            DataHelper deserializedData = new DataHelper();

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
                deserializedData = dataService.DeserializeData(selectedFileContent);
            }
            catch (JsonException exception)
            {
                loadSuccessful = false;
                MessageBox.Show($"The selected file could not be loaded.\nException message: \n{exception.Message}", "Loading error");
            }
            catch (ArgumentNullException argumentNullException)
            {
                loadSuccessful = false;
                MessageBox.Show($"An exception occured while trying to load the player pool. Your player pool file contains invalid data.\nException message: \n{argumentNullException.Message}", "Loading error");
            }
            catch (ArgumentException argumentException)
            {
                loadSuccessful = false;
                MessageBox.Show($"An Exception occured while trying to load the player pool. Your player pool file contains invalid data.\nException message: \n{argumentException.Message}", "Loading error");
            }

            if (loadSuccessful)
            {
                Ranks = deserializedData.Ranks;
                PlayerPool = deserializedData.PlayerPool;

                GenerateTeamsCommand.RaiseCanExecuteChanged();
                statusMessageService.UpdateStatusMessage($"Players loaded.");
            }
            else
            {
                statusMessageService.UpdateStatusMessage($"Error while loading players.");
            }
        }

        private void SavePlayerPool()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = ".tgpp";
            saveFileDialog.Title = "Save you player pool";
            string serializedData = string.Empty;

            DataHelper dataForSerialization = new DataHelper()
            {
                PlayerPool = PlayerPool,
                Ranks = Ranks
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    serializedData = dataService.SerializeData(dataForSerialization);
                }
                catch (JsonException exception)
                {
                    MessageBox.Show($"Current player pool could not be saved. \nException message: \n{exception.Message}", "Saving error");
                    return;
                }

                File.WriteAllText(saveFileDialog.FileName, serializedData);

                statusMessageService.UpdateStatusMessage($"Players saved at {saveFileDialog.FileName}.");
            }
        }

        private void SaveResult()
        {
            match.Team1Score = team1Score;
            match.Team2Score = team2Score;

            statisticsDataService.SaveMatchStatistics(match);
            statusMessageService.UpdateStatusMessage($"Match {match.Id} saved.");
        }

        #endregion
    }
}
