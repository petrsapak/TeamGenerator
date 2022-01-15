namespace TeamGenerator.ViewModels
{
    internal class SettingsViewModel : ViewModelBase
    {
        public SettingsViewModel()
        {
            Placeholder = "SETTINGS";
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
