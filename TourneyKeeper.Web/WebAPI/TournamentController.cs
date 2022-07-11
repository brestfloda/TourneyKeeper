using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using TourneyKeeper.Common;
using TourneyKeeper.Common.Managers;
using TourneyKeeper.Common.SharedCode;
using TourneyKeeper.DTO.Web;
using TourneyKeeper.Web.WebAPI.DTO;

namespace TourneyKeeper.Web.WebAPI
{
    public class TournamentController : BaseController<TKTournament, TournamentManager>
    {
        [HttpPost]
        public HttpResponseMessage AddPlayer(AddPlayerDTO dto)
        {
            try
            {
                var playerManager = new TournamentPlayerManager();
                var tournamentPlayer = new TKTournamentPlayer { PlayerId = dto.PlayerId, PlayerName = dto.PlayerName, TournamentId = dto.TournamentId };
                var num = playerManager.AddTournamentPlayer(tournamentPlayer);

                return new JsonMessage(true, "Ok");
            }
            catch (Exception e)
            {
                LogManager.LogError(e.Message);
                return new JsonMessage(false, e.Message);
            }
        }

        [HttpPost]
        public HttpResponseMessage AddOrganizer(AddPlayerDTO dto)
        {
            try
            {
                var playerManager = new TournamentPlayerManager();
                var organizer = new TKOrganizer { PlayerId = dto.PlayerId, TournamentId = dto.TournamentId };
                manager.AddOrganizer(organizer);

                return new JsonMessage(true, "Ok");
            }
            catch (Exception e)
            {
                LogManager.LogError(e.Message);
                return new JsonMessage(false, e.Message);
            }
        }

        [HttpPost]
        public HttpResponseMessage GetActivePlayers(IntDTO dto)
        {
            try
            {
                var playerManager = new TournamentPlayerManager();
                var num = playerManager.NumberOfActivePlayers(dto.Id);

                return new JsonMessage(true, num.ToString());
            }
            catch (Exception e)
            {
                LogManager.LogError(e.Message);
                return new JsonMessage(false, e.Message);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetTournaments(DateTime from, DateTime to)
        {
            try
            {
                var tournaments = manager.GetTournaments(from, to);
                var response = new GetTournamentsResponseDto
                {
                    Tournaments = tournaments.Select(t => new TournamentDto
                    {
                        End = t.TournamentEndDate,
                        GameSystem = t.TKGameSystem.Name,
                        Id = t.Id,
                        Name = t.Name,
                        IsTeamTournament = t.TournamentType == TournamentType.Team,
                        PlayersPrTeam = t.TeamSize ?? 0,
                        Start = t.TournamentDate
                    }).ToList()
                };
                return new JsonMessage(true, JsonConvert.SerializeObject(response));
            }
            catch (Exception e)
            {
                LogManager.LogError(e.Message);
                return new JsonMessage(false, e.Message);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetCurrentRound(int tournamentId)
        {
            try
            {
                var round = manager.GetCurrentRound(tournamentId);

                return new JsonMessage(true, JsonConvert.SerializeObject(round));
            }
            catch (Exception e)
            {
                LogManager.LogError(e.Message);
                return new JsonMessage(false, e.Message);
            }
        }
    }
}