using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace TeamGenerator.Model.Tests
{
    [TestFixture]
    public class TeamTests
    {
        private Team team;
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

        [SetUp]
        public void SetUp()
        {
            team = new Team("Test");
        }

        [Test]
        public void AddPlayer_PlayerAdded_WhenPlayerIsValid()
        {
            Player player = new Player("Nick", csgoRanks.First(rank => rank.Name == "Silver 1"));
            team.AddPlayer(player);

            Assert.That(team.Players.Contains(player));
        }

        [Test]
        public void AddPlayer_PlayerNotAdded_WhenPlayerIsAlreadyInTeam()
        {
            Player player = new Player("Nick", csgoRanks.First(rank => rank.Name == "Silver 1"));
            Player theSamePlayer = (Player)player.Clone();
            team.AddPlayer(player);
            team.AddPlayer(theSamePlayer);

            Assert.Multiple(() =>
            {
                Assert.That(team.Players.Contains(player));
                Assert.That(team.Players.Count, Is.EqualTo(1));
            });
        }

        [Test]
        public void AddPlayer_ThrowsException_WhenPlayerIsNull()
        {
            Assert.That(() => team.AddPlayer(null), Throws.Exception);
        }

        [Test]
        public void RemovePlayer_PlayerRemoved_WhenPlayerIsInTeam()
        {
            Player player = new Player("Nick", csgoRanks.First(rank => rank.Name == "Silver 1"));
            team.RemovePlayer(player);

            Assert.That(team.Players.Contains(player), Is.False);
        }
    }
}
