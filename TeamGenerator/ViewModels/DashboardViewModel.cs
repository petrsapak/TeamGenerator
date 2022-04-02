using System.Collections.Generic;
using System.Collections.ObjectModel;
using TeamGenerator.Model;
using Prism.Commands;
using System.Linq;
using System;
using TeamGenerator.Core.Interfaces;
using Prism.Ioc;
using System.IO;
using System.Text.Json;
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
        private readonly SaveFileDialogService saveFileDialogService;
        private readonly OpenFileDialogService openFileDialogService;

        public DashboardViewModel(IContainerProvider container, IEventAggregator eventAggregator)
        {
            InitializeCommands();

            this.eventAggregator = eventAggregator;
            generator = container.Resolve<IGenerate>();
            evaluator = container.Resolve<IEvaluate>();
            statusMessageService = container.Resolve<IStatusMessageService>();
            dataService = container.Resolve<IDataService<DataHelper>>();
            statisticsDataService = container.Resolve<IStatisticsDataService>();
            saveFileDialogService = container.Resolve<SaveFileDialogService>();
            openFileDialogService = container.Resolve<OpenFileDialogService>();

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

            MaxBotCount = 1;
            BotQuotient = 0.5;
            Team1Score = 0;
            Team2Score = 0;
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

        private bool enableBots;

        public bool EnableBots
        {
            get => enableBots;
            set => SetProperty(ref enableBots, value);
        }

        private int maxBotCount;

        public int MaxBotCount
        {
            get => maxBotCount;
            set => SetProperty(ref maxBotCount, value);
        }

        public string MaxBotCountString
        {
            get => maxBotCount.ToString();
            set
            {
                if (int.TryParse(value, out int validInt) && validInt >= 0)
                {
                    SetProperty(ref maxBotCount, validInt);
                }
            }
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

        public string Team1ScoreString
        {
            get => team1Score.ToString();
            set
            {
                if (int.TryParse(value, out int validTeam1Score) && validTeam1Score >= 0)
                {
                    SetProperty(ref team1Score, validTeam1Score);
                }
            }
        }

        private int team2Score;

        public int Team2Score
        {
            get => team2Score;
            set => SetProperty(ref team2Score, value);
        }

        public string Team2ScoreString
        {
            get => team2Score.ToString();
            set
            {
                if (int.TryParse(value, out int validTeam2Score) && validTeam2Score >= 0)
                {
                    SetProperty(ref team2Score, validTeam2Score);
                }
            }
        }

        private double botQuotient;

        public double BotQuotient
        {
            get => botQuotient;
            set => SetProperty(ref botQuotient, value);
        }

        public string BotQuotientString
        {
            get => botQuotient.ToString();
            set
            {
                if (double.TryParse(value, out double validDouble) && validDouble >= 0)
                {
                    SetProperty(ref botQuotient, validDouble);
                }
            }
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
            SaveResultCommand = new DelegateCommand(SaveMatchResult, CanSaveMatchResults);
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
            IGeneratorSettings settings = new GeneratorSettings()
            {
                AvailablePlayerPool = PlayerPool,
                UseBots = EnableBots,
                BotQuotient = BotQuotient,
                MaxBotCount = MaxBotCount
            };

            (Team, Team) teams = generator.GenerateTeams(settings);

            Team1 = new ObservableCollection<Player>(teams.Item1.Players);
            Team2 = new ObservableCollection<Player>(teams.Item2.Players);

            double counterTerroristTeamEvaluation = evaluator.EvaluateTeam(teams.Item1);
            double terroristTeamEvaluation = evaluator.EvaluateTeam(teams.Item2);
            double sumOfEvaluations = counterTerroristTeamEvaluation + terroristTeamEvaluation;
            double evaluationPointToPercent = (double)100 / (double)sumOfEvaluations;
            double counterTerroristsChanceOfWinning = (int)Math.Round(counterTerroristTeamEvaluation * evaluationPointToPercent);
            double terroristsChanceOfWinning = (int)Math.Round(terroristTeamEvaluation * evaluationPointToPercent);

            Team1Probability = (int)counterTerroristsChanceOfWinning;
            Team2Probability = (int)terroristsChanceOfWinning;

            SaveResultCommand.RaiseCanExecuteChanged();
            statusMessageService.UpdateStatusMessage($"Teams generated.");
        }

        private bool CanExecuteGenerateTeams()
        {
            return PlayerPool.Count() > 1;
        }

        private void LoadPlayerPool()
        {
            bool loadSuccessful = false;
            DataHelper deserializedData = new DataHelper();
            string serializedPlayerPool = openFileDialogService.ShowOpenFileDialog("Load player pool", ".tgpp");

            if (string.IsNullOrEmpty(serializedPlayerPool)) return;

            try
            {
                deserializedData = dataService.DeserializeData(serializedPlayerPool);
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
            {
                Ranks = deserializedData.Ranks;
                PlayerPool = deserializedData.PlayerPool;

                GenerateTeamsCommand.RaiseCanExecuteChanged();
                statusMessageService.UpdateStatusMessage($"Players loaded.");
            }
        }

        private void SavePlayerPool()
        {
            DataHelper dataForSerialization = new DataHelper()
            {
                PlayerPool = PlayerPool,
                Ranks = Ranks
            };

            try
            {
                string serializedData = dataService.SerializeData(dataForSerialization);
                saveFileDialogService.ShowSaveFileDialog("Save player pool", ".tgpp", serializedData);
            }
            catch (JsonException ex)
            {
                statusMessageService.UpdateStatusMessage("Error while serializing player pool.");
            }
        }

        private void SaveMatchResult()
        {
            Team team1 = new Team("Team 1")
            {
                Players = Team1.ToList()
            };

            Team team2 = new Team("Team 2")
            {
                Players = Team2.ToList()
            };

            Match match = new Match()
            {
                CreationDate = DateTime.Now.ToString("dd.MM.yy HH:mm:ss"),
                Team1 = team1,
                Team2 = team2
            };

            match.Team1Score = team1Score;
            match.Team2Score = team2Score;

            match.Team1Probability = Team1Probability;
            match.Team2Probability = Team2Probability;

            statisticsDataService.SaveMatchStatistics(match);
            statusMessageService.UpdateStatusMessage($"Match saved.");
        }

        private bool CanSaveMatchResults()
        {
            if (Team1 == null || Team2 == null)
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}
