using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using TeamGenerator.Core;
using TeamGenerator.Core.Interfaces;
using TeamGenerator.Model;
using System;
using System.Linq;
using System.Collections.Generic;
using TeamGenerator.Infrastructure;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Text.Json;

namespace TeamGenerator.Shell.ViewModels
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly PlayerDataManager playerDataManager = new PlayerDataManager();
        private readonly IGenerate bestComplementTeamGenerator;
        private readonly IEvaluate evaluator;

        public MainWindowViewModel()
        {
            availablePlayers = new ObservableCollection<Player>();
            ranks = new List<Rank>
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

            InitializeCommands();

            evaluator = new BasicEvaluator();
            bestComplementTeamGenerator = new BestComplementGenerator(evaluator, 10);
        }

        #region Commands

        private void InitializeCommands()
        {
            AddAvailablePlayerCommand = new Command(AddAvailablePlayer, CanAddNewPlayer);
            DeleteAvailablePlayerCommand = new Command(DeleteAvailablePlayer, CanDeletePlayer);
            GenerateTeamsCommand = new Command(GenerateTeams, CanGenerateTeams);
            LoadPlayerPoolCommand = new Command(LoadPlayerPool, CanLoadPlayers);
            SavePlayerPoolCommand = new Command(SavePlayerPool, CanSavePlayers);
        }

        private bool CanLoadPlayers(object parameters)
        {
            return true;
        }

        private bool CanSavePlayers(object parameters)
        {
            return true;
        }

        private bool CanAddNewPlayer(object parameters)
        {
            return AvailablePlayers.All(player => player.Nick != NewPlayerName) && !string.IsNullOrEmpty(NewPlayerName) && NewPlayerName.Any(char.IsLetterOrDigit);
        }

        private bool CanDeletePlayer(object parameters)
        {
            return SelectedAvailablePlayer != null;
        }

        private bool CanGenerateTeams(object parameters)
        {
            return AvailablePlayers.Count > 0;
        }

        public ICommand AddAvailablePlayerCommand { get; set; }
        public ICommand GenerateTeamsCommand { get; set; }
        public ICommand DeleteAvailablePlayerCommand { get; set; }
        public ICommand LoadPlayerPoolCommand { get; set; }
        public ICommand SavePlayerPoolCommand { get; set; }

        private void AddAvailablePlayer(object parameters)
        {
            Player availablePlayer = new Player(nick: NewPlayerName, rank: newPlayerRank);
            AvailablePlayers.Add(availablePlayer);
        }

        private void DeleteAvailablePlayer(object parameters)
        {
            AvailablePlayers.Remove(SelectedAvailablePlayer);
        }

        private void GenerateTeams(object parameters)
        {
            (Team, Team) teams = bestComplementTeamGenerator.GenerateTeams(AvailablePlayers, FillWithBots);

            Team1 = new ObservableCollection<Player>(teams.Item1.Players.Values);
            Team2 = new ObservableCollection<Player>(teams.Item2.Players.Values);

            double counterTerroristTeamEvaluation = evaluator.EvaluateTeam(teams.Item1);
            double terroristTeamEvaluation = evaluator.EvaluateTeam(teams.Item2);
            double sumOfEvaluations = counterTerroristTeamEvaluation + terroristTeamEvaluation;
            double evaluationPointToPercent = (double)100 / (double)sumOfEvaluations;
            double counterTerroristsChanceOfWinning = (int)Math.Round(counterTerroristTeamEvaluation * evaluationPointToPercent);
            double terroristsChanceOfWinning = (int)Math.Round(terroristTeamEvaluation * evaluationPointToPercent);

            Team1Probability = $"Estimated chance to win {counterTerroristsChanceOfWinning}%";
            Team2Probability = $"Estimated chance to win {terroristsChanceOfWinning}%";
        }

        private void LoadPlayerPool(object parameters)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = ".tgpp";
            openFileDialog.Title = "Select your saved player pool";
            string selectedFileContent = string.Empty;

            if (openFileDialog.ShowDialog() == true)
            {
                selectedFileContent = File.ReadAllText(openFileDialog.FileName);
            }

            try
            {
                AvailablePlayers = new ObservableCollection<Player>(playerDataManager.DeserializePlayerPool(selectedFileContent));
            }
            catch (JsonException exception)
            {
                MessageBox.Show($"The selected file could not be loaded.", "Loading error");
            }
        }

        private void SavePlayerPool(object parameters)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = ".tgpp";
            saveFileDialog.Title = "Save you player pool";
            string serializedPlayerPool = string.Empty;

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    serializedPlayerPool = playerDataManager.SerializePlayerPool(AvailablePlayers.ToList<Player>());
                }
                catch (JsonException exception)
                {
                    MessageBox.Show($"Current player pool could not be saved.", "Saving error");
                    return;
                }

                File.WriteAllText(saveFileDialog.FileName, serializedPlayerPool);
            }

        }

        #endregion

        #region Properties

        private string newPlayerName;

        public string NewPlayerName
        {
            get => newPlayerName;
            set
            {
                newPlayerName = value;
                RaisePropertyChanged(nameof(NewPlayerName));
            }
        }

        private Rank newPlayerRank;

        public Rank NewPlayerRank
        {
            get => newPlayerRank;
            set
            {
                newPlayerRank = value;
                RaisePropertyChanged(nameof(NewPlayerRank));
            }
        }

        private Player selectedAvailablePlayer;

        public Player SelectedAvailablePlayer
        {
            get => selectedAvailablePlayer;
            set
            {
                selectedAvailablePlayer = value;
                RaisePropertyChanged(nameof(SelectedAvailablePlayer));
            }
        }

        private bool fillWithBots;

        public bool FillWithBots
        {
            get => fillWithBots;
            set
            {
                fillWithBots = value;
                RaisePropertyChanged(nameof(fillWithBots));
            }
        }

        private ObservableCollection<Player> availablePlayers;

        public ObservableCollection<Player> AvailablePlayers
        {
            get => availablePlayers;
            set
            {
                availablePlayers = value;
                RaisePropertyChanged(nameof(AvailablePlayers));
            }
        }

        private ObservableCollection<Player> team2;

        public ObservableCollection<Player> Team2
        {
            get => team2;
            set
            {
                team2 = value;
                RaisePropertyChanged(nameof(Team2));
            }
        }

        private ObservableCollection<Player> team1;

        public ObservableCollection<Player> Team1
        {
            get => team1;
            set
            {
                team1 = value;
                RaisePropertyChanged(nameof(Team1));
            }
        }

        private string team2Probability;

        public string Team2Probability
        {
            get => team2Probability;
            set
            {
                team2Probability = value;
                RaisePropertyChanged(nameof(Team2Probability));
            }
        }

        private string team1Probability;

        public string Team1Probability
        {
            get => team1Probability;
            set
            {
                team1Probability = value;
                RaisePropertyChanged(nameof(Team1Probability));
            }
        }

        private List<Rank> ranks;

        public List<Rank> Ranks
        {
            get => ranks;
            set
            {
                ranks = value;
                RaisePropertyChanged(nameof(Ranks));
            }
        }
        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
