using Prism.Ioc;
using Prism.Unity;
using System.Windows;
using TeamGenerator.Views;

namespace TeamGenerator
{
    internal class Bootstrapper : PrismBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}
