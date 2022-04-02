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
    internal class SettingsViewModel : ViewModelBase
    {
        private readonly IDataService<List<Rank>> rankDataService;
        private readonly IEventAggregator eventAggregator;
        private readonly IContainerProvider container;
        private readonly IStatusMessageService statusMessageService;
        private readonly SaveFileDialogService saveFileDialogService;
        private readonly OpenFileDialogService openFileDialogService;

        public SettingsViewModel(IContainerProvider container, IEventAggregator eventAggregator, IStatusMessageService statusMessageService)
        {
            this.container = container;
            this.eventAggregator = eventAggregator;
            this.statusMessageService = statusMessageService;

            Ranks = new ObservableCollection<Rank>();
            InitializeCommands();

            rankDataService = container.Resolve<IDataService<List<Rank>>>();
            saveFileDialogService = container.Resolve<SaveFileDialogService>();
            openFileDialogService = container.Resolve<OpenFileDialogService>();
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
            bool loadSuccessful = false;
            string serializedRanks = openFileDialogService.ShowOpenFileDialog("Load custom ranks", ".tgce");

            if (string.IsNullOrEmpty(serializedRanks)) return;

            try
            {
                Ranks = new ObservableCollection<Rank>(rankDataService.DeserializeData(serializedRanks));
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
                    statusMessageService.UpdateStatusMessage($"An Exception occured while trying to load the custom ranks.");
                }
            }

            if (loadSuccessful)
            {
                statusMessageService.UpdateStatusMessage($"Ranks loaded.");
            }
        }

        private void SaveRanks()
        {
            try
            {
                string serializedData = rankDataService.SerializeData(Ranks.ToList<Rank>());
                saveFileDialogService.ShowSaveFileDialog("Save custom ranks", ".tgce", serializedData);
            }
            catch (JsonException ex)
            {
                statusMessageService.UpdateStatusMessage("Error while serializing custom ranks.");
            }

        }

        private void UseRanks()
        {
            eventAggregator.GetEvent<UpdateRanksEvent>().Publish(Ranks.ToList());
            eventAggregator.GetEvent<UpdateStatusMessageEvent>().Publish("Ranks updated.");
        }

        #endregion
    }
} 