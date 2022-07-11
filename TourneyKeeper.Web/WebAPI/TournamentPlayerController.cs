using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using TourneyKeeper.Common;
using TourneyKeeper.Common.Managers;
using TourneyKeeper.DTO.App;
using TourneyKeeper.DTO.Web;

namespace TourneyKeeper.Web.WebAPI
{
    public class TournamentPlayerController : BaseController<TKTournamentPlayer, TournamentPlayerManager>
    {
        [HttpPost]
        public HttpResponseMessage SelectOpponent(SelectOpponentDTO dto)
        {
            try
            {
                var manager = new GameManager();
                manager.SelectOpponent(dto.GameId, dto.TournamentPlayerId, dto.OpponentId, dto.Table, dto.PlayerTeam1);

                return new JsonMessage(true, JsonConvert.SerializeObject(new ResponseDTO { Success = true }));
            }
            catch (Exception e)
            {
                LogManager.LogError(e.Message);
                return new JsonMessage(false, JsonConvert.SerializeObject(new ResponseDTO { Success = false, Message = e.Message }));
            }
        }

        [HttpGet]
        public HttpResponseMessage GetPlayerArmyDetails(int tournamentPlayerId)
        {
            try
            {
                var tournamentPlayer = manager.GetPlayerArmyDetails(tournamentPlayerId);
                var playerArmyDetails = new PlayerArmyDetailsDTO
                {
                    Army = tournamentPlayer.ArmyList,
                    PrimaryId = tournamentPlayer.PrimaryCodexId,
                    SecondaryId = tournamentPlayer.SecondaryCodexId,
                    TertiaryId = tournamentPlayer.TertiaryCodexId,
                    QuaternaryId = tournamentPlayer.QuaternaryCodexId,
                    PlayerId = tournamentPlayer.PlayerId,
                    PlayerName = tournamentPlayer.PlayerName,
                    TournamentPlayerId = tournamentPlayer.Id,
                    TeamId = tournamentPlayer.TournamentTeamId,
                    TeamName = tournamentPlayer.TeamName,
                    Active = tournamentPlayer.Active,
                };

                return new JsonMessage(true, JsonConvert.SerializeObject(playerArmyDetails));
            }
            catch (Exception e)
            {
                LogManager.LogError(e.Message);
                return new JsonMessage(false, JsonConvert.SerializeObject(new PlayerArmyDetailsDTO { }));
            }
        }

        [HttpPost]
        public HttpResponseMessage SetPlayerArmyDetails(PlayerArmyDetailsDTO dto)
        {
            try
            {
                manager.SetPlayerArmyDetails(dto.TournamentPlayerId, dto.Army, dto.PrimaryId, dto.SecondaryId, dto.TertiaryId, dto.QuaternaryId);

                return new JsonMessage(true, JsonConvert.SerializeObject(new ResponseDTO { Success = true }));
            }
            catch (Exception e)
            {
                LogManager.LogError(e.Message);
                return new JsonMessage(false, JsonConvert.SerializeObject(new ResponseDTO { Success = false, Message = e.Message }));
            }
        }

        [HttpPost]
        public HttpResponseMessage GetOpponents(GetOpponentsDTO dto)
        {
            try
            {
                var manager = new TournamentTeamManager();
                var opponents = manager.GetOpponents(dto.PlayerToken);

                return new JsonMessage(true, JsonConvert.SerializeObject(opponents));
            }
            catch (Exception e)
            {
                LogManager.LogError(e.Message);
                return new JsonMessage(false, JsonConvert.SerializeObject(new GetOpponentsResponseDTO { GameId = null }));
            }
        }

        [HttpPost]
        public HttpResponseMessage SignOut(SignOutDTO dto)
        {
            try
            {
                var success = manager.DeletePlayer(dto.TournamentId, dto.PlayerId);

                return new JsonMessage(success, "Signed out");
            }
            catch (Exception e)
            {
                LogManager.LogError(e.Message);
                return new JsonMessage(false, e.Message);
            }
        }
    }
}