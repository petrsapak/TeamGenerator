using System;
using TeamGenerator.Model.Validators;

namespace TeamGenerator.Model
{
    public class Player : ICloneable
    {
        public string Nick { get; private set; }
        public string Name { get; private set; }
        public Rank Rank { get; private set; }
        public bool? Bot { get; private set; }

        public Player(string nick, Rank rank, bool? bot = false)
        {
            Validator.ValidateString(nick);
            Nick = nick;
            Name = nick;
            Rank = rank ?? throw new ArgumentNullException(nameof(rank));
            Bot = bot ?? throw new ArgumentNullException(nameof(bot));
        }

        private Player() { }

        public object Clone()
        {
            Player clone = new Player()
            {
                Nick = Nick,
                Rank = Rank,
                Bot = Bot
            };

            return clone;
        }

        public override string ToString()
        {
            return Nick;
        }
    }
}
