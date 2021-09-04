using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TeamGenerator.Core.Interfaces;
using TeamGenerator.Model;

namespace TeamGenerator.Core.Test
{
    class BasicGeneratorTest
    {
        private CSGORanks csgoRanks = new CSGORanks();

        [Test]
        public void GenerateTeams_ReturnsCorrectlyNamedEmptyTeams_WhenNoPlayersAreProvided()
        {
            IGenerate basicGenerator = new BestComplementGenerator(new BasicEvaluator(), new List<Player>(), new Random());
            (Team, Team) teams = basicGenerator.GenerateTeams();

            Assert.Multiple(() =>
            {
                Assert.That(teams.Item1.Name, Is.EqualTo("CT"));
                Assert.That(teams.Item2.Name, Is.EqualTo("T"));
                Assert.That(teams.Item1.Players.Count, Is.EqualTo(0));
                Assert.That(teams.Item2.Players.Count, Is.EqualTo(0));
            });
        }

        [Test]
        public void GenerateTeams_ReturnsBalancedTeams_WhenListOfBalancablePlayersIsProvided()
        {
            List<Player> availablePlayers = new List<Player>
            {
                new Player("1", csgoRanks.Ranks.First(rank => rank.Name == "Silver 4")),
                new Player("2", csgoRanks.Ranks.First(rank => rank.Name == "Silver 2")),
                new Player("3", csgoRanks.Ranks.First(rank => rank.Name == "Silver 2")),
                new Player("4", csgoRanks.Ranks.First(rank => rank.Name == "Silver 1"))
            };
            IGenerate basicGenerator = new BestComplementGenerator(new BasicEvaluator(), availablePlayers, new Random());
            (Team, Team) teams = basicGenerator.GenerateTeams();

            Assert.Multiple(() =>
            {
                Assert.That(teams.Item1.Players.Count, Is.GreaterThan(0));
                Assert.That(teams.Item2.Players.Count, Is.GreaterThan(0));
            });
        }
    }
}
