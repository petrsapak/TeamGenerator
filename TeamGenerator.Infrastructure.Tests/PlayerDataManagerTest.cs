using NUnit.Framework;
using System.Collections.Generic;
using TeamGenerator.Model;

namespace TeamGenerator.Infrastructure.Tests
{
    public class PlayerDataManagerTest
    {
        string validTestPlayerPoolSerialization = 
            "[{\"Nick\":\"John\",\"Rank\":{\"Name\":\"Master\",\"Value\":3}}," +
            "{\"Nick\":\"Doe\",\"Rank\":{\"Name\":\"Blaster\",\"Value\":5}}," +
            "{\"Nick\":\"Johnny\",\"Rank\":{\"Name\":\",.l;op[\",\"Value\":6}}," +
            "{\"Nick\":\"Long\",\"Rank\":{\"Name\":\"1234567890\",\"Value\":9}}," +
            "{\"Nick\":\"Silver\",\"Rank\":{\"Name\":\"1234asdf,.l;\",\"Value\":13}}]";

        List<Player> validTestPlayerPool = new List<Player>
        {
            new Player("John", new Rank("Master", 3)),
            new Player("Doe", new Rank("Blaster", 5)),
            new Player("Johnny", new Rank(",.l;op[", 6)),
            new Player("Long", new Rank("1234567890", 9)),
            new Player("Silver", new Rank("1234asdf,.l;", 13)),
        };

        [Test]
        public void PlayerPoolIsSerializedCorrectly()
        {
            var playerDataManager = new PlayerDataManager();
            string actualPlayerPoolSerialization = playerDataManager.SerializePlayerPool(validTestPlayerPool);

            Assert.That(actualPlayerPoolSerialization == validTestPlayerPoolSerialization);
        }
    }
}
