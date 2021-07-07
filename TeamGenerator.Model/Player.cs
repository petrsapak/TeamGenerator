namespace TeamGenerator.Model
{
    public class Player
    {
        public string Nick { get; private set; }
        public Rank Rank { get; private set; }

        public Player(string nick, Rank rank)
        {
            Nick = nick;
            Rank = rank;
        }
    }
}
