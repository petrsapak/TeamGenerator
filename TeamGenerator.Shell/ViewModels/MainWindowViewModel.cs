using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using TeamGenerator.Core;
using TeamGenerator.Core.Interfaces;
using TeamGenerator.Model;
using System;
using System.Linq;
using TeamGenerator.Model.Interfaces;
using System.Collections.Generic;

namespace TeamGenerator.Shell.ViewModels
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        public MainWindowViewModel()
        {
            availablePlayers = new ObservableCollection<Player>();
            ranks = new CSGORanks().Ranks;
            AddAvailablePlayerCommand = new Command(AddAvailablePlayer, CanAddNewPlayer);
            DeleteAvailablePlayerCommand = new Command(DeleteAvailablePlayer, CanDeletePlayer);
            GenerateTeamsCommand = new Command(GenerateTeams, CanGenerateTeams);
        }

        #region Commands

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

        private IRank newPlayerRank;

        public IRank NewPlayerRank
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

        private List<IRank> ranks;

        public List<IRank> Ranks
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
