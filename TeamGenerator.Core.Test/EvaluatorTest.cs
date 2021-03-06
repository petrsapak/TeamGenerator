using NUnit.Framework;
using TeamGenerator.Core.Interfaces;
using TeamGenerator.Model;

namespace TeamGenerator.Core.Tests
{
    [TestFixture]
    public class EvaluatorTest
    {
        private readonly IEvaluate evaluator = new BasicEvaluator();

        [Test]
        public void EvaluateTeam_ReturnsSumOfIndividualTeamMembersEvaluations()
        {
            Team team = EvaluatorTestHelper.GenerateRandomTeam();
            double sumOfIndividualEvaluations = EvaluatorTestHelper.GetSumOfIndividualEvaluations(team);

            Assert.That(evaluator.EvaluateTeam(team), Is.EqualTo(sumOfIndividualEvaluations));
        }
    }
}
