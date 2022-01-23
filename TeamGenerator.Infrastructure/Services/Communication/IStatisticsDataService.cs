using TeamGenerator.Model;

namespace TeamGenerator.Infrastructure.Services
{
    public interface IStatisticsDataService
    {
        void SaveMatchStatistics(Match match);
    }
}
