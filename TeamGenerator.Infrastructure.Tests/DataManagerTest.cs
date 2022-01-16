using NUnit.Framework;
using System.Collections.Generic;
using TeamGenerator.Infrastructure.Services;
using TeamGenerator.Model;

namespace TeamGenerator.Infrastructure.Tests
{
    public class DataManagerTest
    {
        readonly string validTestPlayerPoolSerialization = 
            "[{\"Nick\":\"John\",\"Rank\":{\"Name\":\"Master\",\"Value\":3},\"Bot\":false}," +
            "{\"Nick\":\"Doe\",\"Rank\":{\"Name\":\"Blaster\",\"Value\":5},\"Bot\":false}," +
            "{\"Nick\":\"Johnny\",\"Rank\":{\"Name\":\",.l;op[\",\"Value\":6},\"Bot\":false}," +
            "{\"Nick\":\"Long\",\"Rank\":{\"Name\":\"1234567890\",\"Value\":9},\"Bot\":false}," +
            "{\"Nick\":\"Silver\",\"Rank\":{\"Name\":\"1234asdf,.l;\",\"Value\":13},\"Bot\":false}]";

        readonly List<Player> validTestPlayerPool = new List<Player>
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
            IDataService<List<Player>> playerDataManager = new DataService<List<Player>>();
            string actualPlayerPoolSerialization = playerDataManager.SerializeData(validTestPlayerPool);

            Assert.That(actualPlayerPoolSerialization == validTestPlayerPoolSerialization);
        }
    }
}
