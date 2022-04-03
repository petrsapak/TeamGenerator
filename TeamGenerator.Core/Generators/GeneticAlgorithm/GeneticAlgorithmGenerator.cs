using System;
using System.Collections.Generic;
using System.Linq;
using TeamGenerator.Core.Interfaces;
using TeamGenerator.Model;

namespace TeamGenerator.Core.Generators
{
    public class GeneticAlgorithmGenerator : IGenerate
    {
        private readonly IEvaluate evaluator;
        private List<Player> playerPool;
        private List<Player> playerPoolBackup;
        private List<Team> population;
        private Team team1Buffer;
        private Team team2Buffer;
        private readonly Random random;

        public GeneticAlgorithmGenerator(IEvaluate evaluator)
        {
            this.evaluator = evaluator;
            this.random = new Random();
        }

        public (Team, Team) GenerateTeams(IGeneratorSettings generatorSettings)
        {
            playerPool = generatorSettings.AvailablePlayerPool.ToList();
            playerPoolBackup = playerPool.ToList();
            team1Buffer = new Team("1");
            team2Buffer = new Team("2");

            if (playerPool.Count == 0)
            {
                return (team1Buffer, team2Buffer);
            }

            (Team, Team) initializedRandomTeams = InitializeRandomTeams();
            team1Buffer = initializedRandomTeams.Item1;
            team2Buffer = initializedRandomTeams.Item2;





            Team team1 = (Team)team1Buffer.Clone();
            Team team2 = (Team)team2Buffer.Clone();

            CleanBuffers();
            RefreshAvailablePlayerPool();

            return (team1, team2);
        }

        private (Team, Team) InitializeRandomTeams()
        {
            Team firstRandomTeam = new Team("1");
            Team secondRandomTeam = new Team("2");

            return (firstRandomTeam, secondRandomTeam);
        }

        private void CreateNextGeneration()
        {
            population = new List<Team>();

            for (int i = 0; i < ; i++)
            { 
                var firstParentIndex = random.Next(0, matingPool.Count);
                var secondParentIndex = random.Next(0, matingPool.Count);

                while (secondParentIndex != firstParentIndex)
                {
                    secondParentIndex = random.Next(0, matingPool.Count);
                }

                var firstParent = matingPool[firstParentIndex];
                var secondParent = matingPool[secondParentIndex];

                var child = Crossover(firstParent, secondParent);

                if (childWithBestFitness.Fitness > 5 * target.Length / 7)
                {
                    child.Mutate(0);
                }
                else
                {
                    child.Mutate(mutationRate);
                }

                agents.Add(child);
            }

            NumberOfGenerations++;
        }


        private void RefreshAvailablePlayerPool()
        {
            foreach (Player player in playerPoolBackup)
            {
                playerPool.Add(player);
            }
        }

        private void CleanBuffers()
        {
            team1Buffer = new Team("1");
            team2Buffer = new Team("2");
        }

    }
}
