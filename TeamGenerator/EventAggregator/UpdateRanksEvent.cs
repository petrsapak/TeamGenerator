using Prism.Events;
using System.Collections.Generic;
using TeamGenerator.Model;

namespace TeamGenerator.EventAggregator
{
    internal class UpdateRanksEvent : PubSubEvent<List<Rank>>
    {

    }
}
