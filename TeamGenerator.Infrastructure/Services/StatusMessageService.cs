using Prism.Events;
using TeamGenerator.Infrastructure.Events;

namespace TeamGenerator.Infrastructure.Services
{
    public class StatusMessageService : IStatusMessageService
    {
        private readonly IEventAggregator eventAggregator;
        public StatusMessageService(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
        }

        public void UpdateStatusMessage(string newMessage)
        {
            eventAggregator.GetEvent<UpdateStatusMessageEvent>().Publish(newMessage);
        }
    }
}
