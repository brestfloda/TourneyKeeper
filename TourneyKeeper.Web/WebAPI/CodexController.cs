using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using TourneyKeeper.Common;
using TourneyKeeper.Common.Managers;
using TourneyKeeper.DTO.App;
using TourneyKeeper.DTO.Web;

namespace TourneyKeeper.Web.WebAPI
{
    public class CodexController : BaseController<TKCodex, CodexManager>
    {
        [HttpGet]
        public HttpResponseMessage GetCodices(int gamesystemId)
        {
            try
            {
                var codices = manager.GetAllCodices(gamesystemId).Select(g => new CodexDTO { Id = g.Id, Name = g.Name }).OrderBy(c => c.Name);

                return new JsonMessage(true, JsonConvert.SerializeObject(codices));
            }
            catch (Exception e)
            {
                LogManager.LogError(e.Message);
                return new JsonMessage(false, e.Message);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetCodexCount(int tournamentId)
        {
            try
            {
                var codices = manager.GetCodexCount(tournamentId).Select(g => new CodexCountDTO { Name = g.Item1, Count = g.Item2 });

                return new JsonMessage(true, JsonConvert.SerializeObject(codices));
            }
            catch (Exception e)
            {
                LogManager.LogError(e.Message);
                return new JsonMessage(false, e.Message);
            }
        }
    }
}