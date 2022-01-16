using Prism.Events;
using System.Collections.Generic;
using TeamGenerator.Model;

namespace TeamGenerator.Infrastructure.Events
{
    public class UpdateRanksEvent : PubSubEvent<List<Rank>>
    {
    }
}
