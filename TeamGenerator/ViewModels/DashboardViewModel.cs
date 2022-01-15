using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TeamGenerator.Core;
using TeamGenerator.Infrastructure;
using TeamGenerator.Model;
using TeamGenerator.Commands;

namespace TeamGenerator.ViewModels
{
    internal class DashboardViewModel : ViewModelBase
    {
        public DashboardViewModel()
        {
            AvailablePlayers = new ObservableCollection<Player>();
            Ranks = new List<Rank>
            //{
            //    new Rank("Silver 1", 1),
            //    new Rank("Silver 2", 2),
            //    new Rank("Silver 3", 3),
            //    new Rank("Silver 4", 4),
            //    new Rank("Silver Master", 5),
            //    new Rank("Silver Master Elite", 6),
            //    new Rank("Golden Nova 1", 7),
            //    new Rank("Golden Nova 2", 8),
            //    new Rank("Golden Nova 3", 9),
            //    new Rank("Golden Nova Master", 10),
            //    new Rank("Master Guardian 1", 11),
            //    new Rank("Master Guardian 2", 12),
            //    new Rank("Master Guardian Elite", 13),
            //    new Rank("Distinguished Master Guardian", 14),
            //    new Rank("Legendary Eagle", 15),
            //    new Rank("Legendary Eagle Master", 16),
            //    new Rank("Supreme Master First Class", 17),
            //    new Rank("Global Elite", 18),
            //};
            {
                new Rank("", 1),
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
            InitializeCommands();
        }

        #region Properties

        private string newPlayerName;

        public string NewPlayerName
        {
            get => newPlayerName;
            set => SetProperty(ref newPlayerName, value);
        }

        private Rank newPlayerRank;

        public Rank NewPlayerRank
        {
            get => newPlayerRank;
            set => SetProperty(ref newPlayerRank, value);
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

        private ObservableCollection<Player> availablePlayers;

        public ObservableCollection<Player> AvailablePlayers
        {
            get => availablePlayers;
            set => SetProperty(ref availablePlayers, value);
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
            AddAvailablePlayerCommand = new AddAvailablePlayerCommand(this);
            DeleteAvailablePlayerCommand = new DeleteAvailablePlayerCommand(this);
            GenerateTeamsCommand = new GenerateTeamsCommand(this, new BestComplementGenerator(new BasicEvaluator()), new BasicEvaluator());
            LoadPlayerPoolCommand = new LoadPlayerPoolCommand(this, new PlayerDataManager());
            SavePlayerPoolCommand = new SavePlayerPoolCommand(this, new PlayerDataManager());
        }

        public ICommand AddAvailablePlayerCommand { get; set; }
        public ICommand GenerateTeamsCommand { get; set; }
        public ICommand DeleteAvailablePlayerCommand { get; set; }
        public ICommand LoadPlayerPoolCommand { get; set; }
        public ICommand SavePlayerPoolCommand { get; set; }

        #endregion
    }
}
