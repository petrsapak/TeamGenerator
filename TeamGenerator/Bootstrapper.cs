using Prism.Ioc;
using Prism.Unity;
using System.Windows;
using TeamGenerator.Core;
using TeamGenerator.Core.Interfaces;
using TeamGenerator.Infrastructure;
using TeamGenerator.Views;
using Unity;

namespace TeamGenerator
{
    internal class Bootstrapper : PrismBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<Shell>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IGenerate, BestComplementGenerator>();
            containerRegistry.Register<IEvaluate, BasicEvaluator>();
            containerRegistry.Register<IPlayerDataService, PlayerDataService>();
        }
    }
}
