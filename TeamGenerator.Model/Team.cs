using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeamGenerator.Model.Validators;

namespace TeamGenerator.Model
{
    public class Team : ICloneable
    {
        public string Name { get; private set; }
        //this setter is public because of System.Text.Json bug - it won't deserialize otherwise
        public List<Player> Players { get; set; }
        public int BotCount { get; private set; }
        public int Score { get; set; }

        public Team(string name, int score = 0) : this()
        {
            Validator.ValidateString(name);
            Name = name;
            Score = score;
        }

        private Team()
        {
            Players = new List<Player>();
        }

        public void AddPlayer(Player player)
        {
            if (player == null)
            {
                throw new ArgumentNullException(nameof(player));
            }

            if (!Players.Any(p => p.Nick == player.Nick))
            {
                Players.Add(player);
            }

            if ((bool)player.Bot)
            {
                BotCount++;
            }
        }

        public void RemovePlayer(Player player)
        {
            Players.Remove(player);

            if ((bool)player.Bot)
            {
                BotCount--;
            }
        }

        public object Clone()
        {
            Team clone = new Team()
            {
                Name = Name
            };

            foreach (Player player in Players)
            {
                clone.Players.Add((Player)player.Clone());
            }

            return clone;
        }

        public override string ToString()
        {
            if (Players.Count == 0)
                return string.Empty;

            StringBuilder builder = new StringBuilder();

            foreach (Player player in Players)
            {
                builder.Append(player.Nick);
                builder.Append(", ");
            }

            //Get rid of trailing comma
            builder.Remove(builder.Length - 2, 2);

            return builder.ToString();
        }
    }
}
