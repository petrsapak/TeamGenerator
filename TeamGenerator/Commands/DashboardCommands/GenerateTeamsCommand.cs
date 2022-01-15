using System;
using System.Collections.ObjectModel;
using TeamGenerator.Core.Interfaces;
using TeamGenerator.Model;
using TeamGenerator.ViewModels;

namespace TeamGenerator.Commands
{
    internal class GenerateTeamsCommand : CommandBase
    {
        private readonly DashboardViewModel dashboardViewModel;
        private readonly IGenerate generator;
        private readonly IEvaluate evaluator;

        public GenerateTeamsCommand(DashboardViewModel dashboardViewModel, IGenerate generator, IEvaluate evaluator)
        {
            this.dashboardViewModel = dashboardViewModel;
            this.generator = generator;
            this.evaluator = evaluator;
        }

        public override void Execute(object parameter)
        {
            int maxPlayerCountInt = int.Parse(dashboardViewModel.MaxPlayerCount);

            (Team, Team) teams = generator.GenerateTeams(dashboardViewModel.AvailablePlayers, dashboardViewModel.FillWithBots, maxPlayerCountInt);

            dashboardViewModel.Team1 = new ObservableCollection<Player>(teams.Item1.Players.Values);
            dashboardViewModel.Team2 = new ObservableCollection<Player>(teams.Item2.Players.Values);

            double counterTerroristTeamEvaluation = evaluator.EvaluateTeam(teams.Item1);
            double terroristTeamEvaluation = evaluator.EvaluateTeam(teams.Item2);
            double sumOfEvaluations = counterTerroristTeamEvaluation + terroristTeamEvaluation;
            double evaluationPointToPercent = (double)100 / (double)sumOfEvaluations;
            double counterTerroristsChanceOfWinning = (int)Math.Round(counterTerroristTeamEvaluation * evaluationPointToPercent);
            double terroristsChanceOfWinning = (int)Math.Round(terroristTeamEvaluation * evaluationPointToPercent);

            dashboardViewModel.Team1Probability = (int)counterTerroristsChanceOfWinning;
            dashboardViewModel.Team2Probability = (int)terroristsChanceOfWinning;
        }

        public override bool CanExecute(object parameter)
        {
            return dashboardViewModel.AvailablePlayers.Count > 1;
        }
    }
}
