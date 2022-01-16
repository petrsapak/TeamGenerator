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
    internal class SettingsViewModel : ViewModelBase
    {
        private readonly IDataService<List<Rank>> rankDataService;
        private readonly IEventAggregator eventAggregator;
        private readonly IContainerProvider container;
        private readonly IStatusMessageService statusMessageService;

        public SettingsViewModel(IContainerProvider container, IEventAggregator eventAggregator, IStatusMessageService statusMessageService)
        {
            this.container = container;
            this.eventAggregator = eventAggregator;
            this.statusMessageService = statusMessageService;

            Ranks = new ObservableCollection<Rank>();
            InitializeCommands();

            rankDataService = container.Resolve<IDataService<List<Rank>>>();
        }

        #region Properties

        private ObservableCollection<Rank> ranks;

        public ObservableCollection<Rank> Ranks
        {
            get => ranks;
            set => SetProperty(ref ranks, value);
        }

        private Rank selectedRank;

        public Rank SelectedRank
        {
            get => selectedRank;
            set => SetProperty(ref selectedRank, value);
        }

        private string newRankName;

        public string NewRankName
        {
            get => newRankName;
            set
            {
                SetProperty(ref newRankName, value);
                AddRankCommand.RaiseCanExecuteChanged();
            }
        }

        private double newRankValue;

        public double NewRankValue
        {
            get => newRankValue;
            set
            {
                SetProperty(ref newRankValue, value);
                AddRankCommand.RaiseCanExecuteChanged();
            }
        }

        private bool useRanksAsDefault;

        public bool UseRanksAsDefault
        {
            get => useRanksAsDefault;
            set => SetProperty(ref useRanksAsDefault, value);
        }


        #endregion

        #region Commands

        public DelegateCommand AddRankCommand { get; private set; }
        public DelegateCommand RemoveRankCommand { get; private set; }
        public DelegateCommand LoadRanksCommand { get; private set; }
        public DelegateCommand SaveRanksCommand { get; private set; }
        public DelegateCommand UseRanksCommand { get; private set; }

        private void InitializeCommands()
        {
            AddRankCommand = new DelegateCommand(AddRank, CanExecuteAddRank);
            RemoveRankCommand = new DelegateCommand(RemoveRank, CanExecuteRemoveRank);
            LoadRanksCommand = new DelegateCommand(LoadRanks);
            SaveRanksCommand = new DelegateCommand(SaveRanks);
            UseRanksCommand = new DelegateCommand(UseRanks);
        }

        private void AddRank()
        {
            Ranks.Add(new Rank(NewRankName, NewRankValue));
            statusMessageService.UpdateStatusMessage($"Rank {NewRankName} added.");
        }

        private bool CanExecuteAddRank()
        {
            return Ranks.All(rank => rank.Name != NewRankName) &&
                                       !string.IsNullOrEmpty(NewRankName) &&
                                       NewRankName.Any(char.IsLetterOrDigit) &&
                                       NewRankValue > 0;
        }

        private void RemoveRank()
        {
            var name = SelectedRank.Name;
            Ranks.Remove(SelectedRank);

            AddRankCommand.RaiseCanExecuteChanged();
            statusMessageService.UpdateStatusMessage($"Rank {name} removed.");
        }

        private bool CanExecuteRemoveRank()
        {
            return SelectedRank != null;
        }

        private void LoadRanks()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = ".tgce";
            openFileDialog.Title = "Select your custom evaluation";
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
                Ranks = new ObservableCollection<Rank>(rankDataService.DeserializeData(selectedFileContent));
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

            statusMessageService.UpdateStatusMessage($"Ranks loaded.");
        }

        private void SaveRanks()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = ".tgce";
            saveFileDialog.Title = "Save your custom evaluation";
            string serializedPlayerPool = string.Empty;

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    serializedPlayerPool = rankDataService.SerializeData(Ranks.ToList<Rank>());
                }
                catch (JsonException exception)
                {
                    MessageBox.Show($"Current player pool could not be saved. \nException message: \n{exception.Message}", "Saving error");
                    return;
                }

                File.WriteAllText(saveFileDialog.FileName, serializedPlayerPool);
            }


            statusMessageService.UpdateStatusMessage($"Ranks saved at {saveFileDialog.FileName}.");
        }

        private void UseRanks()
        {
            eventAggregator.GetEvent<UpdateRanksEvent>().Publish(Ranks.ToList());
            eventAggregator.GetEvent<UpdateStatusMessageEvent>().Publish("Ranks updated.");
        }

        #endregion
    }
} 