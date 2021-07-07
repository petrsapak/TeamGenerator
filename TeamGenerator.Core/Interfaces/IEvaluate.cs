using TeamGenerator.Model;

namespace TeamGenerator.Core.Interfaces
{
    public interface IEvaluate
    {
        int EvaluatePlayer(Player player);
        int EvaluateTeam(Team team);
    }
}
