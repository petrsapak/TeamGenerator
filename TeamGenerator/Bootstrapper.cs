using Prism.Ioc;
using Prism.Unity;
using System.Collections.Generic;
using System.Windows;
using TeamGenerator.Core;
using TeamGenerator.Core.Interfaces;
using TeamGenerator.Infrastructure.Services;
using TeamGenerator.Model;
using TeamGenerator.ViewModels;
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
            containerRegistry.Register<IStatusMessageService, StatusMessageService>();
            containerRegistry.Register<IDataService<List<Player>>, DataService<List<Player>>>();
            containerRegistry.Register<IDataService<List<Rank>>, DataService<List<Rank>>>();
            containerRegistry.Register<IDataService<List<Match>>, DataService<List<Match>>>();
            containerRegistry.Register<IStatisticsDataService, StatisticsDataService>();
            containerRegistry.RegisterSingleton<DashboardViewModel>();
            containerRegistry.RegisterSingleton<StatisticsViewModel>();
            containerRegistry.RegisterSingleton<SettingsViewModel>();
            containerRegistry.RegisterSingleton<AboutViewModel>();
        }
    }
}
