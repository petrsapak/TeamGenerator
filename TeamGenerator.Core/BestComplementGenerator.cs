using System;
using System.Collections.Generic;
using System.Linq;
using TeamGenerator.Core.Interfaces;
using TeamGenerator.Model;

namespace TeamGenerator.Core
{
    public class BestComplementGenerator : IGenerate
    {
        private readonly List<Player> availablePlayerPool;
        private readonly List<Player> availablePlayerPoolBackup;
        private readonly IEvaluate evaluator;
        private readonly Random random;

        private Team teamCounterTerroristBuffer;
        private Team teamTerroristBuffer;

        public BestComplementGenerator(IEvaluate evaluator, IEnumerable<Player> availablePlayers, Random random)
        {
            this.availablePlayerPool = availablePlayers.ToList();
            this.availablePlayerPoolBackup = availablePlayers.ToList();
            this.evaluator = evaluator;
            this.random = random;

            teamTerroristBuffer = new Team("T");
            teamCounterTerroristBuffer = new Team("CT");
        }

        public (Team, Team) GenerateTeams()
        {
            try
            {
                Player initialRandomCoutnerTerroristPlayer = GetRandomPlayerFromPool();
                MovePlayerFromPoolToBuffer(initialRandomCoutnerTerroristPlayer, teamCounterTerroristBuffer);

                Player initialRandomTerroristPlayer = GetRandomPlayerFromPool();
                MovePlayerFromPoolToBuffer(initialRandomTerroristPlayer, teamTerroristBuffer);

                while(availablePlayerPool.Count > 0)
                {
                    AddNextPlayer();
                }
            }
            catch (ArgumentOutOfRangeException exception)
            {
                //TODO log
            }

            Team teamCounterTerrorist = (Team)teamCounterTerroristBuffer.Clone();
            Team teamTerrorist = (Team)teamTerroristBuffer.Clone();

            CleanBuffers();
            RefreshAvailablePlayerPool();

            return (teamCounterTerrorist, teamTerrorist);
        }

        private void RefreshAvailablePlayerPool()
        {
            foreach (Player player in availablePlayerPoolBackup)
            {
                availablePlayerPool.Add(player);
            }
        }

        private void CleanBuffers()
        {
            teamTerroristBuffer = new Team("T");
            teamCounterTerroristBuffer = new Team("CT");
        }

        private void MovePlayerFromPoolToBuffer(Player player, Team buffer)
        {
            availablePlayerPool.Remove(player);
            buffer.AddPlayer(player);
        }

        private void AddNextPlayer()
        {
            double teamCounterTerroristEvaluation = evaluator.EvaluateTeam(teamCounterTerroristBuffer);
            double teamTerroristEvaluation = evaluator.EvaluateTeam(teamTerroristBuffer);
            double evaluationDifference = teamCounterTerroristEvaluation - teamTerroristEvaluation;

            if (evaluationDifference == 0)
            {
                Player randomPlayer = GetRandomPlayerFromPool();
                MovePlayerFromPoolToBuffer(randomPlayer, teamTerroristBuffer);
            }
            else if (evaluationDifference > 0)
            {
                Player bestComplementPlayer = GetBestComplementPlayerFromPool(evaluationDifference);
                MovePlayerFromPoolToBuffer(bestComplementPlayer, teamTerroristBuffer);
            }
            else
            {
                Player bestComplementPlayer = GetBestComplementPlayerFromPool(evaluationDifference);
                MovePlayerFromPoolToBuffer(bestComplementPlayer, teamCounterTerroristBuffer);
            }
        }

        private Player GetRandomPlayerFromPool()
        {
            Player randomPlayer = availablePlayerPool[random.Next(availablePlayerPool.Count)];
            return randomPlayer;
        }

        private Player GetBestComplementPlayerFromPool(double evaluationDifference)
        {
            Player bestComplementPlayer = null;

            foreach (Player player in availablePlayerPool)
            {
                if (bestComplementPlayer == null)
                {
                    bestComplementPlayer = player;
                }
                else
                {
                    double bestComplementPlayerDifferenceAbs =  Math.Abs(evaluator.EvaluatePlayer(bestComplementPlayer) - evaluationDifference);
                    double currentPlayerDifferenceAbs = Math.Abs(evaluator.EvaluatePlayer(player) - evaluationDifference);
                    if (currentPlayerDifferenceAbs < bestComplementPlayerDifferenceAbs)
                    {
                        bestComplementPlayer = player;
                    }
                }
            }

            return bestComplementPlayer;
        }
    }
}
