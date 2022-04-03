using System;
using TeamGenerator.Core.Interfaces;
using TeamGenerator.Model;

namespace TeamGenerator.Core.Generators.GeneticAlgorithm
{
    /// <summary>
    /// Agent = Team
    /// </summary>

    internal class Agent
    {
        public Team Genes { get; set; }
        public int Fitness { get; set; }

        private IEvaluate evaluator;

        public Agent(Team team, IEvaluate evaluator)
        {
            Genes = team;
            this.evaluator = evaluator;
        }

        public void Mutate(int mutationRate)
        {
            Random random = new Random();
            int playerCount = Genes.Players.Count; 

            for (int i = 0; i < playerCount; i++)
            {
                if (random.Next(0, playerCount) <= mutationRate)
                {
                    Genes = Constants.alphabet[random.Next(0, 27)];
                }
            }
        }

        public void CalculateFitness()
        {
            Fitness = (int)evaluator.EvaluateTeam(Genes);
        }
    }
}
