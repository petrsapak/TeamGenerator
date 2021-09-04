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

namespace TeamGenerator.Shell.ViewModels
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        private PlayerDataManager playerDataManager = new PlayerDataManager();

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
            AddAvailablePlayerCommand = new Command(AddAvailablePlayer, CanAddNewPlayer);
            DeleteAvailablePlayerCommand = new Command(DeleteAvailablePlayer, CanDeletePlayer);
            GenerateTeamsCommand = new Command(GenerateTeams, CanGenerateTeams);
            LoadPlayerPoolCommand = new Command(LoadPlayerPool, CanLoadPlayers);
            SavePlayerPoolCommand = new Command(SavePlayerPool, CanSavePlayers);
        }

        #region Commands

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
            return AvailablePlayers.All(player => player.Nick != NewPlayerName) && !string.IsNullOrEmpty(NewPlayerName);
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
            IEvaluate evaluator = new BasicEvaluator();
            IGenerate generator = new BestComplementGenerator(evaluator, AvailablePlayers, new Random());
            (Team, Team) teams = generator.GenerateTeams();

            CounterTerrorists = new ObservableCollection<Player>(teams.Item1.Players.Values);
            Terrorists = new ObservableCollection<Player>(teams.Item2.Players.Values);

            double counterTerroristTeamEvaluation = evaluator.EvaluateTeam(teams.Item1);
            double terroristTeamEvaluation = evaluator.EvaluateTeam(teams.Item2);
            double sumOfEvaluations = counterTerroristTeamEvaluation + terroristTeamEvaluation;
            double evaluationPointToPercent = (double)100 / (double)sumOfEvaluations;
            double counterTerroristsChanceOfWinning = (int)Math.Round(counterTerroristTeamEvaluation * evaluationPointToPercent);
            double terroristsChanceOfWinning = (int)Math.Round(terroristTeamEvaluation * evaluationPointToPercent);

            CounterTerroristsProbability = $"Estimated chance to win {counterTerroristsChanceOfWinning}%";
            TerroristsProbability = $"Estimated chance to win {terroristsChanceOfWinning}%";
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

            AvailablePlayers = new ObservableCollection<Player>(playerDataManager.DeserializePlayerPool(selectedFileContent));
        }

        private void SavePlayerPool(object parameters)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = ".tgpp";
            saveFileDialog.Title = "Save you player pool";

            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, playerDataManager.SerializePlayerPool(AvailablePlayers.ToList<Player>()));
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

        private ObservableCollection<Player> terrorists;

        public ObservableCollection<Player> Terrorists
        {
            get => terrorists;
            set
            {
                terrorists = value;
                RaisePropertyChanged(nameof(Terrorists));
            }
        }

        private ObservableCollection<Player> counterTerrorists;

        public ObservableCollection<Player> CounterTerrorists
        {
            get => counterTerrorists;
            set
            {
                counterTerrorists = value;
                RaisePropertyChanged(nameof(CounterTerrorists));
            }
        }

        private string terroristsProbability;

        public string TerroristsProbability
        {
            get => terroristsProbability;
            set
            {
                terroristsProbability = value;
                RaisePropertyChanged(nameof(TerroristsProbability));
            }
        }

        private string counterTerroristsProbability;

        public string CounterTerroristsProbability
        {
            get => counterTerroristsProbability;
            set
            {
                counterTerroristsProbability = value;
                RaisePropertyChanged(nameof(CounterTerroristsProbability));
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
