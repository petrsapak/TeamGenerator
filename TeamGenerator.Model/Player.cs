namespace TeamGenerator.Model
{
    public class Player
    {
        public string Nick { get; private set; }
        public Rank Rank { get; private set; }
        public bool Bot { get; private set; }

        public Player(string nick, Rank rank, bool bot = false)
        {
            Nick = nick;
            Rank = rank;
            Bot = bot;
        }
    }
}
