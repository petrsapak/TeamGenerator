namespace TeamGenerator.Model
{
    public class Match
    {
        public string CreationDate { get; set; }
        public Team Team1 { get; set; }
        public Team Team2 { get; set; }
        public int Team1Score { get; set; }
        public int Team2Score { get; set; }
        public int Team1Probability { get; set; }
        public int Team2Probability { get; set; }
    }
}
