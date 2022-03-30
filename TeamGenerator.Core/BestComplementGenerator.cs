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

        public (Team, Team) GenerateTeams(IEnumerable<Player> availablePlayers, bool fillWithBots, int maxPlayerCount)
        {
            availablePlayerPool = availablePlayers.ToList();
            availablePlayerPoolBackup = availablePlayerPool.ToList();
            team1Buffer = new Team("1");
            team2Buffer = new Team("2");

            if (availablePlayerPool.Count == 0)
            {
                return (new Team("1"), new Team("2"));
            }

            Player initialRandomTeam1Player = GetRandomPlayerFromPool();
            MovePlayerFromPoolToBuffer(initialRandomTeam1Player, team1Buffer);

            Player initialRandomTeam2Player = GetRandomPlayerFromPool();
            MovePlayerFromPoolToBuffer(initialRandomTeam2Player, team2Buffer);

            while (availablePlayerPool.Count > 0)
            {
                AddNextPlayer();
            }

            int botCounter = 0;

            while (fillWithBots && team1Buffer.Players.Count + team2Buffer.Players.Count <= maxPlayerCount)
            {
                double botCoefficient = 0.5;

                double team1Evaluation = evaluator.EvaluateTeam(team1Buffer);
                double team1EvaluationWithoutBots = evaluator.EvaluateTeamWithoutBots(team1Buffer);
                double team2Evaluation = evaluator.EvaluateTeam(team2Buffer);
                double team2EvaluationWithoutBots = evaluator.EvaluateTeamWithoutBots(team2Buffer);

                double team1RankAverage = team1EvaluationWithoutBots / (double)team1Buffer.Players.Count;
                double team1BotEvaluation = team1RankAverage * botCoefficient;

                double team2RankAverage = team2EvaluationWithoutBots / (double)team2Buffer.Players.Count;
                double team2BotEvaluation = team2RankAverage * botCoefficient;

                double teamEvaluationDifference = team1Evaluation - team2Evaluation;

                if (teamEvaluationDifference > 2)
                {
                    team2Buffer.AddPlayer(new Player($"Bot {botCounter}", new Rank("Bot", team2BotEvaluation), bot: true));
                }
                else if (teamEvaluationDifference < -3)
                {
                    team1Buffer.AddPlayer(new Player($"Bot {botCounter}", new Rank("Bot", team1BotEvaluation), bot: true));
                }
                else
                {
                    break;
                }

                botCounter++;
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
