using System;
using System.Collections.Generic;

namespace TourneyKeeper.Web.WebAPI.DTO
{
    public class GetGamesResponseDto
    {
        public List<GameDto> Games { get; set; }
    }
}