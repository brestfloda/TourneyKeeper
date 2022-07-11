using Newtonsoft.Json;
using TourneyKeeper.DTO.App;
using System;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using TourneyKeeper.Common.Managers;
using TourneyKeeper.Web.WebAPI.DTO;

namespace TourneyKeeper.Web.WebAPI
{
    public class NotificationController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage RegisterClient(RegisterClientDTO clientToken)
        {
            try
            {
                var manager = new PlayerManager();
                manager.SetNotificationToken(clientToken.PlayerToken, clientToken.ClientToken);

                return new JsonMessage(true, JsonConvert.SerializeObject(new ResponseDTO { Success = true }));
            }
            catch (Exception e)
            {
                LogManager.LogError(e.Message);
                return new JsonMessage(false, JsonConvert.SerializeObject(new ResponseDTO { Success = false, Message = e.Message }));
            }
        }
    }
}