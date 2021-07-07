using NUnit.Framework;

namespace TeamGenerator.Model.Tests
{
    public class TeamTests
    {
        private Team team;

        [SetUp]
        public void SetUp()
        {
            team = new Team("Test");
        }

        [Test]
        public void AddPlayer_PlayerAdded_WhenPlayerIsValid()
        {
            Player player = new Player("Nick", Rank.Silver1);
            team.AddPlayer(player);

            Assert.That(team.Players.ContainsKey("Nick"));
        }

        [Test]
        public void AddPlayer_PlayerNotAdded_WhenPlayerIsAlreadyInTeam()
        {
            Player player = new Player("Nick", Rank.Silver1);
            Player theSamePlayer = new Player("Nick", Rank.Silver1);
            team.AddPlayer(player);
            team.AddPlayer(theSamePlayer);

            Assert.Multiple(() =>
            {
                Assert.That(team.Players.ContainsKey("Nick"));
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
            Player player = new Player("Nick", Rank.Silver1);
            team.RemovePlayer(player);

            Assert.That(team.Players.ContainsKey(player.Nick), Is.False);
        }
    }
}
