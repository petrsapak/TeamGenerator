using Prism.Events;
using TeamGenerator.Model;

namespace TeamGenerator.Infrastructure.Events
{
    public class SaveMatchStatisticsEvent : PubSubEvent<Match>
    {
    }
}
