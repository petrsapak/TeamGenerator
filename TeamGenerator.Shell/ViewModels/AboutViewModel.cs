namespace TeamGenerator.Shell.ViewModels
{
    internal class AboutViewModel : ViewModelBase
    {
        public AboutViewModel()
        {
            AboutMessage = "https://github.com/petrsapak/TeamGenerator";
        }

        private string aboutMessage;

        public string AboutMessage 
        {
            get => aboutMessage;
            set
            {
                aboutMessage = value;
                RaisePropertyChanged(nameof(AboutMessage));
            }
        }
    }
}
