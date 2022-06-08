using System;
using TeamGenerator.Model.Validators;

namespace TeamGenerator.Model
{
    public class Player : ICloneable
    {
        public string Nick { get; }
        public string Name { get; }
        public Rank Rank { get; set; }
        public bool? Bot { get; }

        public Player(string nick, Rank rank, bool? bot = false)
        {
            Validator.ValidateString(nick);
            Nick = nick;
            Name = nick;
            Rank = rank ?? throw new ArgumentNullException(nameof(rank));
            Bot = bot ?? throw new ArgumentNullException(nameof(bot));
        }

        public object Clone()
        {
            Player clonedPlayer = new Player(nick: Nick, rank: Rank, bot: Bot);
            return clonedPlayer;
        }

        public override string ToString()
        {
            return Nick;
        }
    }
}
