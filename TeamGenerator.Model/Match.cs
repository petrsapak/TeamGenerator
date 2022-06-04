using System;
using System.Collections.Generic;

namespace TeamGenerator.Model
{
    public class Match
    {
        public string CreationDate { get; set; }

        [Obsolete("Use teams")]
        public Team Team1 { get; set; }
        [Obsolete("Use teams")]
        public Team Team2 { get; set; }

        [Obsolete("Use properties inside the team")]
        public int Team1Score { get; set; }

        [Obsolete("Use properties inside the team")]
        public int Team2Score { get; set; }

        [Obsolete("Use properties inside the team")]
        public int Team1Probability { get; set; }
        [Obsolete("Use properties inside the team")]
        public int Team2Probability { get; set; }

        public IEnumerable<Team> Teams { get; set; }

        public Match()
        {
            Teams = new List<Team>();
        }
    }
}
