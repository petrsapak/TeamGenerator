using System;
using TeamGenerator.Model.Validators;

namespace TeamGenerator.Model
{
    public class Player : ICloneable
    {
        public string Nick { get; }
        public string Name { get; }
        public Rank Rank { get; set; }
        public bool? Bot { get; private set; }
        public bool IsActive { get; set; }

        public Player(string nick, Rank rank, bool? bot = false)
        {
            Validator.ValidateString(nick);
            Nick = nick;
            Name = nick;
            Rank = rank ?? throw new ArgumentNullException(nameof(rank));
            Bot = bot ?? throw new ArgumentNullException(nameof(bot));
            IsActive = true;
        }

        public object Clone()
        {
            Player clone = new Player()
            {
                Nick = Nick,
                Rank = Rank,
                Bot = Bot,
                IsActive = IsActive
            };

            return clone;
        }

        public override string ToString()
        {
            return Nick;
        }
    }
}
