using TeamGenerator.Model.Interfaces;

namespace TeamGenerator.Model
{
    public class Player
    {
        public string Nick { get; private set; }
        public IRank Rank { get; private set; }

        public Player(string nick, IRank rank)
        {
            Nick = nick;
            Rank = rank;
        }
    }
}
