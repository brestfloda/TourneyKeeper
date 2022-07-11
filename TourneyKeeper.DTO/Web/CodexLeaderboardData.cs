namespace TourneyKeeper.DTO.Web
{
    public class CodexLeaderboardData
    {
        public int Placement { get; set; }
        public int Count { get; set; }
        public string PrimaryCodex { get; set; }
        public float Points { get; set; }
        public double StdDev { get; set; }
    }
}
