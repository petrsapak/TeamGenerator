using System;
using System.Collections.Generic;
using TeamGenerator.Model;

namespace TeamGenerator.Core.Tests
{
    public class EvaluatorTestHelper
    {
        public static Team GenerateRandomTeam()
        {
            Random random = new Random();
            Team team = new Team("Test");
            List<Rank> ranks = new List<Rank>
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

            int teamSize = random.Next(3, 9);

            for (int i = 0; i < teamSize; i++)
            {
                string name = $"Player{i}";
                Rank rank = ranks[random.Next(ranks.Count)];
                Player player = new Player(name, rank);

                team.AddPlayer(player);
            }

            return team;
        }

        public static double GetSumOfIndividualEvaluations(Team team)
        {
            double rankCounter = 0;

            foreach (Player player in team.Players.Values)
            {
                rankCounter += player.Rank.Value;
            }

            return rankCounter;
        }
    }
}
