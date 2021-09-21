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
        private List<Rank> csgoRanks = new List<Rank>
            {
                new Rank("Silver 1", 1),
                new Rank("Silver 2", 2),
                new Rank("Silver 3", 3),
                new Rank("Silver 4", 4),
                new Rank("Silver Master", 5),
                new Rank("Silver Master Elite", 6),
                new Rank("Golden Nova 1", 7),
                new Rank("Golden Nova 2", 8),
                new Rank("Golden Nova 3", 9),
                new Rank("Golden Nova Master", 10),
                new Rank("Master Guardian 1", 11),
                new Rank("Master Guardian 2", 12),
                new Rank("Master Guardian Elite", 13),
                new Rank("Distinguished Master Guardian", 14),
                new Rank("Legendary Eagle", 15),
                new Rank("Legendary Eagle Master", 16),
                new Rank("Supreme Master First Class", 17),
                new Rank("Global Elite", 18),
            };

        [Test]
        public void GenerateTeams_ReturnsCorrectlyNamedEmptyTeams_WhenNoPlayersAreProvided()
        {
            IGenerate basicGenerator = new BestComplementGenerator(new BasicEvaluator());
            (Team, Team) teams = basicGenerator.GenerateTeams(new List<Player>());

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
            List<Player> availablePlayers = new List<Player>
            {
                new Player("1", csgoRanks.First(rank => rank.Name == "Silver 4")),
                new Player("2", csgoRanks.First(rank => rank.Name == "Silver 2")),
                new Player("3", csgoRanks.First(rank => rank.Name == "Silver 2")),
                new Player("4", csgoRanks.First(rank => rank.Name == "Silver 1"))
            };
            IGenerate basicGenerator = new BestComplementGenerator(new BasicEvaluator());
            (Team, Team) teams = basicGenerator.GenerateTeams(availablePlayers);

            Assert.Multiple(() =>
            {
                Assert.That(teams.Item1.Players.Count, Is.GreaterThan(0));
                Assert.That(teams.Item2.Players.Count, Is.GreaterThan(0));
            });
        }
    }
}
