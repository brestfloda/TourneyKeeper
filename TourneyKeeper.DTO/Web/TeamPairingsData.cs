namespace TourneyKeeper.DTO.Web
{
    public class TeamPairingsData
    {
        public string FormattedPoints { get; set; }
        public int TournamentId { get; set; }
        public int MatchId { get; set; }
        public int? TableNumber { get; set; }
        public string Team1Name { get; set; }
        public string Team2Name { get; set; }
        public int Team1Id { get; set; }
        public int Team2Id { get; set; }
        public int Team1Points { get; set; }
        public int Team2Points { get; set; }
        public int Team1BattlePoints { get; set; }
        public int Team2BattlePoints { get; set; }
        public int? Team1Penalty { get; set; }
        public int? Team2Penalty { get; set; }
        public bool AllowSetup { get; set; }
    }
}