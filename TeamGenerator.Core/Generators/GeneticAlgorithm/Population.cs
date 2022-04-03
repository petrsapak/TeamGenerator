using System;
using System.Collections.Generic;
using TeamGenerator.Model;

namespace TeamGenerator.Core.Generators.GeneticAlgorithm
{
    public class Population
    {
        private List<Team> agents;
        private List<Team> matingPool;
        private Team childWithBestFitness;
        private int mutationRate;
        private int populationSize;
        private char[] target;
        public bool Finished;
        public int NumberOfGenerations;
        public char[] BestPhrase;
        public int BestFitness = 0;

        public Population(char[] target, int mutationRate, int populationSize)
        {
            this.target = target;
            this.mutationRate = mutationRate;
            this.populationSize = populationSize;
            Finished = false;
            NumberOfGenerations = 0;
            agents = new List<Team>();
            CreateFirstGeneration();
        }

        private void CreateFirstGeneration()
        {
            for (int i = 0; i < populationSize; i++)
            {
                agents.Add(new Agent(target.Length));
            }
        }

        public void CreateNextGeneration()
        {
            var random = new Random();
            agents = new List<Agent>();

            for (int i = 0; i < populationSize; i++)
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

        public void Evaluate()
        {
            BestFitness = childWithBestFitness.Fitness;
            BestPhrase = childWithBestFitness.Genes;

            if (BestFitness == target.Length)
            {
                Finished = true;
            }
        }

        public void CreateMatingPool()
        {
            matingPool = new List<Agent>();
            childWithBestFitness = new Agent();
            var sumOfFitness = 0;

            for (int i = 0; i < populationSize; i++)
            {
                sumOfFitness += agents[i].CalculateFitness(target);

                if (agents[i].Fitness > childWithBestFitness.Fitness)
                {
                    childWithBestFitness = agents[i];
                }
            }

            for (int i = 0; i < populationSize; i++)
            {
                var numberOfCopiesForMating = CalculateNumberOfCopies(agents[i]);

                for (int j = 0; j < numberOfCopiesForMating; j++)
                {
                    matingPool.Add(agents[i]);
                }
            }
        }

        private int CalculateNumberOfCopies(Agent agent)
        {
            if (agent.Fitness >= childWithBestFitness.Fitness)
            {
                return (agent.Fitness * 2) + 2;
            }
            else if (agent.Fitness < childWithBestFitness.Fitness && agent.Fitness >= childWithBestFitness.Fitness / 2)
            {
                return Convert.ToInt32(Math.Floor((double)agent.Fitness / 2 )) + 1;
            }

            return 1;
        }

        public Agent Crossover(Agent firstParent, Agent secondParent)
        {
            Random random = new Random();
            var child = new Agent(target.Length);

            var midpointIndex = random.Next(0, target.Length);
            //var midpointIndex = Convert.ToInt32(Math.Floor((double)Target.Length / 2));

            for (int i = 0; i < midpointIndex; i++)
            {
                child.Genes[i] = firstParent.Genes[i];
            }

            for (int i = midpointIndex; i < child.Genes.Length; i++)
            {
                child.Genes[i] = secondParent.Genes[i];
            }

            //for (int i = 0; i < Target.Length; i++)
            //{
            //    if (i % 2 == 0)
            //    {
            //        child.Genes[i] = firstParent.Genes[i];
            //    }
            //    else
            //    {
            //        child.Genes[i] = secondParent.Genes[i];
            //    }

            //}

            return child;
        }
    }
}
