using System.Collections.Generic;
using TeamGenerator.Core.Interfaces;
using TeamGenerator.Model;

namespace TeamGenerator.Core
{
    public class GeneratorSettings : IGeneratorSettings
    {
        public int MaxPlayerCount { get; set; }
        public bool UseBots { get; set; }
        public double BotQuotient { get; set; }
        public int MaxBotCount { get; set; }
        public bool TeamsHaveToBeEqual { get; set; }
        public IEnumerable<Player> AvailablePlayerPool { get; set; }
        public bool DisallowTeamRepetition { get; set; }
    }
}
