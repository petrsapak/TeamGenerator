using System;

namespace TeamGenerator.Model
{
    public class Match
    {
        public Guid Id { get; }
        public DateTime CreationDate { get; }
        public Team Team1 { get; set; }
        public Team Team2 { get; set; }
        public int Team1Score { get; set; }
        public int Team2Score { get; set; }
        public int Team1Probability { get; set; }
        public int Team2Probability { get; set; }

        public Match()
        {
            CreationDate = DateTime.Now;
            Id = Guid.NewGuid();
        }
    }
}
