using System;
using System.Collections.Generic;

namespace TeamGenerator.Model
{
    public class Team : ICloneable
    {
        public string Name { get; private set; }
        public Dictionary<string, Player> Players { get; private set; }

        public Team(string name)
        {
            Name = name;
            Players = new Dictionary<string, Player>();
        }

        public void AddPlayer(Player player)
        {
            try
            {
                Players.Add(player.Nick, player);
            }
            catch (ArgumentException argumentException)
            {
                //TODO log and iform
            }
        }

        public void RemovePlayer(Player player)
        {
            Players.Remove(player.Nick);
        }

        public object Clone()
        {
            Team clonedTeam = new Team(Name);

            foreach (Player player in Players.Values)
            {
                clonedTeam.Players.Add(player.Nick, player);
            }

            return clonedTeam;
        }
    }
}
