using System;

namespace TourneyKeeper.Web.WebAPI.DTO
{
    public class TournamentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsTeamTournament { get; set; }
        public int PlayersPrTeam { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string GameSystem { get; set; }
    }
}