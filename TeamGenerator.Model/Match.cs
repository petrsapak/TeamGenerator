using System.Collections.Generic;

namespace TeamGenerator.Model
{
    public class Match
    {
        public string CreationDate { get; set; }
        public IEnumerable<Team> Teams { get; set; }

        public Match()
        {
            Teams = new List<Team>();
        }
    }
}
