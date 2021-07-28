using TeamGenerator.Model;

namespace TeamGenerator.Core.Interfaces
{
    public interface IEvaluate
    {
        double EvaluatePlayer(Player player);
        double EvaluateTeam(Team team);
    }
}
