using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using TourneyKeeper.Common;
using TourneyKeeper.Common.Managers;
using TourneyKeeper.DTO.App;
using TourneyKeeper.DTO.Web;

namespace TourneyKeeper.Web.WebAPI
{
    public class PlayerController : BaseController<TKPlayer, PlayerManager>
    {
        [HttpPost]
        public HttpResponseMessage Search(PlayerSearchDTO dto)
        {
            try
            {
                var result = manager.Search(dto.TournamentId, dto.SearchString);

                var val = result.Select(r => new PlayerSearchResultDTO { PlayerId = r.Id, PlayerName = r.Name }).ToList();

                return new JsonMessage(true, JsonConvert.SerializeObject(val));
            }
            catch (Exception e)
            {
                LogManager.LogError(e.Message);
                return new JsonMessage(false, JsonConvert.SerializeObject(new ResponseDTO { Success = false, Message = e.Message }));
            }
        }

        [HttpPost]
        public HttpResponseMessage SearchOrganizer(PlayerSearchDTO dto)
        {
            try
            {
                var result = manager.SearchOrganizer(dto.TournamentId, dto.SearchString);

                var val = result.Select(r => new PlayerSearchResultDTO { PlayerId = r.Id, PlayerName = r.Name }).ToList();

                return new JsonMessage(true, JsonConvert.SerializeObject(val));
            }
            catch (Exception e)
            {
                LogManager.LogError(e.Message);
                return new JsonMessage(false, JsonConvert.SerializeObject(new ResponseDTO { Success = false, Message = e.Message }));
            }
        }
    }
}