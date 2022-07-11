using System.Collections.Generic;

namespace TourneyKeeper.Web.WebAPI.DTO
{
    public class GetTournamentsResponseDto
    {
        public List<TournamentDto> Tournaments { get; set; }
    }
}