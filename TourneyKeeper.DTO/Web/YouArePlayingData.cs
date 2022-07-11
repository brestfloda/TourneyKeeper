using System;

namespace TourneyKeeper.DTO.Web
{
    public class YouArePlayingData
    {
        public string TournamentName { get; set; }
        public int TournamentId { get; set; }
        public string Round { get; set; }
        public string CurrentOpponentName { get; set; }
        public string CurrentOpponentTeamName { get; set; }
        public int? CurrentOpponentTournamentPlayerId { get; set; }
        public int? CurrentOpponentTeamId { get; set; }
        public bool IsTeamTournament { get; set; }
        public int TournamentPlayerId { get; set; }
        public string LeaderboardLink { get; set; }
        public string PairingsLink { get; set; }
        public string TeamTable { get; set; }
        public string Table { get; set; }
        public string ResultsLink { get; set; }
        public int? TeamId { get; set; }
        public string SetupPairingsLink { get; set; }
        public string Army { get; set; }
        public bool IsCurrentlyMatched { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsTeamCurrentlyMatched { get; set; }
    }
}