namespace TeamGenerator.Shell.ViewModels
{
    internal class StatisticsViewModel : ViewModelBase
    {
        public StatisticsViewModel()
        {
            Placeholder = "STATISTICS";
        }

        private string placeholder;

        public string Placeholder
        {
            get => placeholder;
            set
            {
                placeholder = value;
                RaisePropertyChanged(nameof(Placeholder));
            }
        }
    }
}
