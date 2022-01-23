using Prism.Events;
using TeamGenerator.Infrastructure.Events;
using TeamGenerator.Model;

namespace TeamGenerator.Infrastructure.Services
{
    public class StatisticsDataService : IStatisticsDataService
    {
        private readonly IEventAggregator eventAggregator;
        public StatisticsDataService(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
        }

        public void SaveMatchStatistics(Match match)
        {
            eventAggregator.GetEvent<SaveMatchStatisticsEvent>().Publish(match);
        }
    }
}
