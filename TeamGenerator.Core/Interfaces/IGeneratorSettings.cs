using System.Collections.Generic;
using TeamGenerator.Model;

namespace TeamGenerator.Core.Interfaces
{
    public interface IGeneratorSettings
    {
        bool UseBots { get; set; }
        double BotQuotient { get; set; }
        int MaxBotCount { get; set; }
        bool TeamsHaveToBeEqual { get; set; }
        IEnumerable<Player> AvailablePlayerPool { get; set; }
        bool DisallowTeamRepetition { get; set; }
    }
}
