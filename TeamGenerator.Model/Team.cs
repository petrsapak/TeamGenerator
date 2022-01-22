using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using TeamGenerator.Model.Validators;

namespace TeamGenerator.Model
{
    public class Team : ICloneable
    {
        [JsonPropertyName("Name")]
        public string Name { get; private set; }

        [JsonPropertyName("Players")]
        public List<Player> Players { get; private set; }

        public Team(string name)
        {
            Validator.ValidateString(name);
            Name = name;
            Players = new List<Player>();
        }

        public void AddPlayer(Player player)
        {
            if (!Players.Contains(player))
            {
                Players.Add(player);
            }
        }

        public void RemovePlayer(Player player)
        {
            Players.Remove(player);
        }

        public object Clone()
        {
            Team clonedTeam = new Team(Name);

            foreach (Player player in Players)
            {
                clonedTeam.Players.Add(player);
            }

            return clonedTeam;
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
