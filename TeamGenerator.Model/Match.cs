using System;

namespace TeamGenerator.Model
{
    public class Match
    {
        public string CreationDate { get; set; }

        public Team Team1 { get; set; }
        public Team Team2 { get; set; }

        [Obsolete("Use score inside the team")]
        public int Team1Score { get; set; }

        [Obsolete("Use score inside the team")]
        public int Team2Score { get; set; }

        public int Team1Probability { get; set; }
        public int Team2Probability { get; set; }
    }
}
