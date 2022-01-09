﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using TeamGenerator.Core;
using TeamGenerator.Core.Interfaces;
using TeamGenerator.Infrastructure;
using TeamGenerator.Model;

namespace TeamGenerator.Shell.ViewModels
{
    internal class DashboardViewModel : ViewModelBase
    {
        private readonly PlayerDataManager playerDataManager = new PlayerDataManager();
        private readonly IGenerate bestComplementTeamGenerator;
        private readonly IEvaluate evaluator;

        public DashboardViewModel()
        {
            AvailablePlayers = new ObservableCollection<Player>();
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
            InitializeCommands();
            evaluator = new BasicEvaluator();
            bestComplementTeamGenerator = new BestComplementGenerator(evaluator);
        }

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

        private string maxPlayerCount;

        public string MaxPlayerCount 
        {
            get => maxPlayerCount;
            set
            {
                maxPlayerCount = value;
                RaisePropertyChanged(nameof(MaxPlayerCount));
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

        private int team2Probability;

        public int Team2Probability
        {
            get => team2Probability;
            set
            {
                team2Probability = value;
                RaisePropertyChanged(nameof(Team2Probability));
            }
        }

        private int team1Probability;

        public int Team1Probability
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

        #region Commands

        private void InitializeCommands()
        {
            AddAvailablePlayerCommand = new Command(AddAvailablePlayer, CanAddNewPlayer);
            DeleteAvailablePlayerCommand = new Command(DeleteAvailablePlayer, CanDeletePlayer);
            GenerateTeamsCommand = new Command(GenerateTeams, CanGenerateTeams);
            LoadPlayerPoolCommand = new Command(LoadPlayerPool, CanLoadPlayers);
            SavePlayerPoolCommand = new Command(SavePlayerPool, CanSavePlayers);
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
            int maxPlayerCountInt = int.Parse(MaxPlayerCount);

            (Team, Team) teams = bestComplementTeamGenerator.GenerateTeams(AvailablePlayers, FillWithBots, maxPlayerCountInt);

            Team1 = new ObservableCollection<Player>(teams.Item1.Players.Values);
            Team2 = new ObservableCollection<Player>(teams.Item2.Players.Values);

            double counterTerroristTeamEvaluation = evaluator.EvaluateTeam(teams.Item1);
            double terroristTeamEvaluation = evaluator.EvaluateTeam(teams.Item2);
            double sumOfEvaluations = counterTerroristTeamEvaluation + terroristTeamEvaluation;
            double evaluationPointToPercent = (double)100 / (double)sumOfEvaluations;
            double counterTerroristsChanceOfWinning = (int)Math.Round(counterTerroristTeamEvaluation * evaluationPointToPercent);
            double terroristsChanceOfWinning = (int)Math.Round(terroristTeamEvaluation * evaluationPointToPercent);

            Team1Probability = (int)counterTerroristsChanceOfWinning;
            Team2Probability = (int)terroristsChanceOfWinning;
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
            else
            {
                return;
            }

            try
            {
                AvailablePlayers = new ObservableCollection<Player>(playerDataManager.DeserializePlayerPool(selectedFileContent));
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
                    MessageBox.Show($"Current player pool could not be saved. \nException message: \n{exception.Message}", "Saving error");
                    return;
                }

                File.WriteAllText(saveFileDialog.FileName, serializedPlayerPool);
            }
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
            return AvailablePlayers.Count > 1;
        }


        #endregion
    }
}