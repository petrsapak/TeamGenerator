using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TeamGenerator.Core.Interfaces;
using TeamGenerator.Model;

namespace TeamGenerator.Core.Test
{
    class BasicGeneratorTest
    {
        List<Player> availablePlayers = new List<Player>
        {
            new Player("1", new Rank("1", 1)),
            new Player("2", new Rank("2", 2)),
            new Player("3", new Rank("3", 3)),
            new Player("4", new Rank("4", 4))
        };

        private GeneratorSettings settings;
        private IGenerate basicGenerator = new BestComplementGenerator(new BasicEvaluator());

        [Test]
        public void GenerateTeams_ReturnsCorrectlyNamedEmptyTeams_WhenNoPlayersAreProvided()
        {
            settings = new GeneratorSettings()
            {
                AvailablePlayerPool = new List<Player>(),
            };
 
            (Team, Team) teams = basicGenerator.GenerateTeams(settings);

            Assert.Multiple(() =>
            {
                Assert.That(teams.Item1.Name, Is.EqualTo("1"));
                Assert.That(teams.Item2.Name, Is.EqualTo("2"));
                Assert.That(teams.Item1.Players.Count, Is.EqualTo(0));
                Assert.That(teams.Item2.Players.Count, Is.EqualTo(0));
            });
        }

        [Test]
        public void GenerateTeams_ReturnsBalancedTeams_WhenListOfBalancablePlayersIsProvided()
        {
            settings = new GeneratorSettings()
            {
                AvailablePlayerPool = availablePlayers
            };

            (Team, Team) teams = basicGenerator.GenerateTeams(settings);

            Assert.Multiple(() =>
            {
                Assert.That(teams.Item1.Players.Count, Is.GreaterThan(0));
                Assert.That(teams.Item2.Players.Count, Is.GreaterThan(0));
            });
        }

        [Test]
        public void GenerateTeams_ReturnsTeamsWithoutBots_WhenBotsAreAllowedButMaxCountIsSetToZero()
        {
            settings = new GeneratorSettings()
            {
                AvailablePlayerPool = availablePlayers,
                UseBots = true,
                MaxBotCount = 0
            };

            (Team, Team) teams = basicGenerator.GenerateTeams(settings);

            Assert.That(teams.Item1.BotCount, Is.EqualTo(0));
            Assert.That(teams.Item2.BotCount, Is.EqualTo(0));
        }

        [Test]
        public void GenerateTeams_ReturnsTeamsWithBots_WhenBotsAreAllowed()
        {
            settings = new GeneratorSettings()
            {
                AvailablePlayerPool = availablePlayers,
                UseBots = true,
                MaxBotCount = 2
            };

            (Team, Team) teams = basicGenerator.GenerateTeams(settings);

            Assert.That(teams.Item1.BotCount, Is.GreaterThanOrEqualTo(0));
            Assert.That(teams.Item2.BotCount, Is.GreaterThanOrEqualTo(0));
        }
    }
}
