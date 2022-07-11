using System;

namespace TourneyKeeper.DTO.Web
{
    public class PlayerData
    {
        public bool IsTeamTournament { get; set; }
        public int TournamentId { get; set; }
        public int GameId { get; set; }
        public int OpponentPlayerId { get; set; }
        public string PlayerName { get; set; }
        public string TournamentName { get; set; }
        public DateTime TournamentDate { get; set; }
        public string OpponentName { get; set; }
        public string Ranked { get; set; }
        public string Email { get; set; }
        public int Points { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Draws { get; set; }
        public int Round { get; set; }
        public string Rank { get; set; }
        public double? ELO { get; set; }
        public int? TableNumber { get; set; }
    }
}