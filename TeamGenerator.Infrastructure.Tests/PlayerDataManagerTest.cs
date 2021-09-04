using NUnit.Framework;
using System.Collections.Generic;
using TeamGenerator.Model;

namespace TeamGenerator.Infrastructure.Tests
{
    public class PlayerDataManagerTest
    {
        List<Player> validTestPlayerPool = new List<Player>
        {
            new Player("John", new Rank("Master", 3)),
            new Player("Doe", new Rank("Blaster", 5)),
            new Player("Johnny", new Rank(",./l;'op[", 6)),
            new Player("Long", new Rank("1234567890", 9)),
            new Player("Silver", new Rank("1234asdf,.l;", 13)),
        };

        [Test]
        public void PlayerPoolIsSerializedCorrectly()
        {

        }
    }
}
