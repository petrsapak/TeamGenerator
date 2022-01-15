using Prism.Mvvm;
using Prism.Regions;

namespace TeamGenerator.ViewModels
{
    public class ViewModelBase : BindableBase, INavigationAware
    {
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext) { }

        public void OnNavigatedTo(NavigationContext navigationContext) { }
    }
}
