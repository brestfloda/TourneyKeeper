namespace TourneyKeeper.DTO.Web
{
    public class TeamLeaderboardData
    {
        public int Id { get; set; }
        public int TournamentId { get; set; }
        public int Placement { get; set; }
        public string Name { get; set; }
        public int BattlePointPenalty { get; set; }
        public int Penalty { get; set; }
        public int Points { get; set; }
        public int SecondaryPoints { get; set; }
        public int BattlePoints { get; set; }
        public string Players { get; set; }
    }
}