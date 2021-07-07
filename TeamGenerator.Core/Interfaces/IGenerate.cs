using TeamGenerator.Model;

namespace TeamGenerator.Core.Interfaces
{
    public interface IGenerate
    {
        (Team, Team) GenerateTeams();
    }
}
