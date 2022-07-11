using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using TourneyKeeper.Common;
using TourneyKeeper.Common.Managers;
using TourneyKeeper.DTO.App;
using TourneyKeeper.Web.WebAPI.DTO;

namespace TourneyKeeper.Web.WebAPI
{
    public class GameController : BaseController<TKGame, GameManager>
    {
        [HttpPost]
        public HttpResponseMessage GetGames(GamesDTO dto)
        {
            try
            {
                var playerManager = new PlayerManager();
                var playerId = playerManager.GetPlayerId(dto.Token);
                var games = manager.GetCurrentGames(playerId);

                return new JsonMessage(true, JsonConvert.SerializeObject(games));
            }
            catch (Exception e)
            {
                LogManager.LogError(e.Message);
                return new JsonMessage(false, e.Message);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetGamesForTournament(int tournamentId)
        {
            try
            {
                var games = manager.GetGames(tournamentId);
                var response = new GetGamesResponseDto
                {
                    Games = games.Select(g => new GameDto
                    {
                        Player1Armylist = g.TKTournamentPlayer?.ArmyList,
                        Player1Id = g.Player1Id ?? 0,
                        Player1Result = g.Player1Result,
                        Player1SecondaryResult = g.Player1SecondaryResult,
                        Player1PrimaryCodex = g.TKTournamentPlayer?.TKCodex?.Name,
                        Player1SecondaryCodex = g.TKTournamentPlayer?.TKCodex1?.Name,
                        Player1TertiaryCodex = g.TKTournamentPlayer?.TKCodex2?.Name,
                        Player2Armylist = g.TKTournamentPlayer1?.ArmyList,
                        Player2Id = g.Player2Id ?? 0,
                        Player2Result = g.Player2Result,
                        Player2SecondaryResult = g.Player2SecondaryResult,
                        Player2PrimaryCodex = g.TKTournamentPlayer1?.TKCodex?.Name,
                        Player2SecondaryCodex = g.TKTournamentPlayer1?.TKCodex1?.Name,
                        Player2TertiaryCodex = g.TKTournamentPlayer1?.TKCodex2?.Name,
                        Round = g.Round
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

        [HttpPost]
        public HttpResponseMessage UpdateGame(UpdateGameDTO dto)
        {
            try
            {
                manager.Update(dto.Token, dto.GameId, dto.MyScore, dto.MySecondaryScore, dto.OpponentScore, dto.OpponentSecondaryScore);

                return new JsonMessage(true, JsonConvert.SerializeObject(new UpdateGameResponseDTO { Status = true }));
            }
            catch (Exception e)
            {
                LogManager.LogError(e.Message);
                return new JsonMessage(false, JsonConvert.SerializeObject(new UpdateGameResponseDTO { Status = false, Message = e.Message }));
            }
        }
    }
}