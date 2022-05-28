using System;
using System.Collections.Generic;
using TeamGenerator.Model;

namespace TeamGenerator.Services
{
    internal class RankUpdateService
    {
        public void ProposeRankUpdates(List<Match> session)
        {
            GetDictionaryOfPlayers(session);
        }

        private void GetDictionaryOfPlayers(List<Match> session)
        {
            //Player, number of won rounds, number of matches the player was in
            Dictionary<string, (int, int)> players = new Dictionary<string, (int,int)>();
            Dictionary<string, int> playersWithProposedChanges = new Dictionary<string, int>();

            foreach (Match match in session)
            {
                foreach (Player player in match.Team1.Players)
                {
                    int adjustedValue = GetAdjustedWonRoundValue(match.Team1Probability, match.Team1Score, match.Team2Score);
                    if (players.ContainsKey(player.Nick))
                    {
                        players[player.Nick] = (players[player.Nick].Item1 + adjustedValue, players[player.Nick].Item2 + 1);
                    }
                    else
                    {
                        players.Add(player.Nick, (adjustedValue, 1));
                    }
                }

                foreach (Player player in match.Team2.Players)
                {
                    int adjustedValue = GetAdjustedWonRoundValue(match.Team2Probability, match.Team2Score, match.Team1Score);
                    if (players.ContainsKey(player.Nick))
                    {
                        players[player.Nick] = (players[player.Nick].Item1 + adjustedValue, players[player.Nick].Item2 + 1);
                    }
                    else
                    {
                        players.Add(player.Nick, (adjustedValue, 1));
                    }
                }
            }

            foreach (var item in players)
            {
                double coefficient = 0;
                int proposedUpdate = 0;

                if (item.Value.Item2 != 0)
                {
                    coefficient = (double)item.Value.Item1 / (double)item.Value.Item2;
                }

                if (coefficient >= 0 && coefficient <= 5)
                    proposedUpdate = -2;
                if (coefficient > 5 && coefficient <= 7.5)
                    proposedUpdate = -1;
                if (coefficient > 7.5 && coefficient <= 9.5)
                    proposedUpdate = 0;
                if (coefficient > 9.5 && coefficient <= 20)
                    proposedUpdate = 1;

                playersWithProposedChanges.Add(item.Key, proposedUpdate);
            }
        }

        private int GetAdjustedWonRoundValue(int teamWinProbability, int teamScore, int oppositeTeamScore)
        {
            int probabilityDifference = (2 * teamWinProbability) - 100;
            int scoreDifference = teamScore - oppositeTeamScore;
            int adjustedValue = teamScore;

            if (probabilityDifference <= 4 && probabilityDifference >= -4)
            {
                if (scoreDifference >= 4)
                {
                    adjustedValue++;
                }
                else if (scoreDifference <= -4)
                {
                    adjustedValue--;
                }
            }
            else if (probabilityDifference > 4)
            {
                if (scoreDifference == 0)
                {
                    adjustedValue--;
                }
                else if (scoreDifference >= 4)
                {
                    adjustedValue = adjustedValue + 2;
                }
                else if (scoreDifference <= -4)
                {
                    adjustedValue = adjustedValue - 2;
                }
            }
            else if (probabilityDifference < -4)
            {
                if (scoreDifference == 0)
                {
                    adjustedValue++;
                }
                else if (scoreDifference >= 4)
                {
                    adjustedValue = adjustedValue + 2;
                }
                else if (scoreDifference <= -4)
                {
                    adjustedValue = adjustedValue + 2;
                }
            }

            return adjustedValue;
        }
    }
}
