using System;
using System.Collections.Generic;
using System.Linq;
using TeamGenerator.Core.Interfaces;
using TeamGenerator.Model;

namespace TeamGenerator.Core
{
    public class BestComplementGenerator : IGenerate
    {
        private List<Player> availablePlayerPool;
        private List<Player> availablePlayerPoolBackup;
        private readonly IEvaluate evaluator;
        private readonly Random random;
        private Team team1Buffer;
        private Team team2Buffer;

        public BestComplementGenerator(IEvaluate evaluator)
        {
            this.evaluator = evaluator;
            this.random = new Random();
        }

        public (Team, Team) GenerateTeams(IEnumerable<Player> availablePlayers)
        {
            availablePlayerPool = availablePlayers.ToList();
            availablePlayerPoolBackup = availablePlayerPool.ToList();
            team1Buffer = new Team("1");
            team2Buffer = new Team("2");

            if (availablePlayerPool.Count != 0)
            {
                Player initialRandomCoutnerTerroristPlayer = GetRandomPlayerFromPool();
                MovePlayerFromPoolToBuffer(initialRandomCoutnerTerroristPlayer, team1Buffer);

                Player initialRandomTerroristPlayer = GetRandomPlayerFromPool();
                MovePlayerFromPoolToBuffer(initialRandomTerroristPlayer, team2Buffer);

                while (availablePlayerPool.Count > 0)
                {
                    AddNextPlayer();
                }
            }

            Team team1 = (Team)team1Buffer.Clone();
            Team team2 = (Team)team2Buffer.Clone();

            CleanBuffers();
            RefreshAvailablePlayerPool();

            return (team1, team2);
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
            team1Buffer = new Team("1");
            team2Buffer = new Team("2");
        }

        private void MovePlayerFromPoolToBuffer(Player player, Team buffer)
        {
            availablePlayerPool.Remove(player);
            buffer.AddPlayer(player);
        }

        private void AddNextPlayer()
        {
            double teamCounterTerroristEvaluation = evaluator.EvaluateTeam(team1Buffer);
            double teamTerroristEvaluation = evaluator.EvaluateTeam(team2Buffer);
            double evaluationDifference = teamCounterTerroristEvaluation - teamTerroristEvaluation;

            if (evaluationDifference == 0)
            {
                Player randomPlayer = GetRandomPlayerFromPool();
                MovePlayerFromPoolToBuffer(randomPlayer, team2Buffer);
            }
            else if (evaluationDifference > 0)
            {
                Player bestComplementPlayer = GetBestComplementPlayerFromPool(evaluationDifference);
                MovePlayerFromPoolToBuffer(bestComplementPlayer, team2Buffer);
            }
            else
            {
                Player bestComplementPlayer = GetBestComplementPlayerFromPool(evaluationDifference);
                MovePlayerFromPoolToBuffer(bestComplementPlayer, team1Buffer);
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
