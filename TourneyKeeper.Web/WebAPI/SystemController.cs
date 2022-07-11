using Newtonsoft.Json;
using TourneyKeeper.DTO.App;
using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using TourneyKeeper.Common.Managers;
using TourneyKeeper.DTO.Web;
using TourneyKeeper.Common;

namespace TourneyKeeper.Web.WebAPI
{
    public class SystemController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage GetGameSystems()
        {
            try
            {
                var manager = new TournamentManager();
                var gameSystems = manager.GetGameSystems().Select(g => new BasicIdent { Id = g.Id, Name = g.Name });

                return new JsonMessage(true, JsonConvert.SerializeObject(gameSystems));
            }
            catch (Exception e)
            {
                LogManager.LogError(e.Message);
                return new JsonMessage(false, e.Message);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetCountries()
        {
            try
            {
                return new JsonMessage(true, JsonConvert.SerializeObject(Country.Countries));
            }
            catch (Exception e)
            {
                LogManager.LogError(e.Message);
                return new JsonMessage(false, e.Message);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetTeamScoringSystems()
        {
            try
            {
                var manager = new TournamentManager();
                var gameSystems = manager.GetTeamScoringSystems().Select(g => new BasicIdent { Id = g.Id, Name = g.Name });

                return new JsonMessage(true, JsonConvert.SerializeObject(gameSystems));
            }
            catch (Exception e)
            {
                LogManager.LogError(e.Message);
                return new JsonMessage(false, e.Message);
            }
        }
    }
}