using Newtonsoft.Json;
using TourneyKeeper.DTO.App;
using System;
using System.Net.Http;
using System.Web.Http;
using TourneyKeeper.Common.Managers;
using TourneyKeeper.DTO.Web;

namespace TourneyKeeper.Web.WebAPI
{
    public class LoginController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage Login(LoginDTO dto)
        {
            try
            {
                var manager = new PlayerManager();
                var player = manager.Logon(dto.Login, dto.Password);
                if(player == null)
                {
                    return new JsonMessage(false, "Failed to login");
                }
                var loginInfo = new LoginResponseDTO { Email = player.Email, Name = player.Name, Token = player.Token };

                return new JsonMessage(true, JsonConvert.SerializeObject(loginInfo));
            }
            catch (Exception e)
            {
                LogManager.LogError(e.Message);
                return new JsonMessage(false, e.Message);
            }
        }

        [HttpPost]
        public HttpResponseMessage VerifyLogin(VerifyLoginDTO dto)
        {
            try
            {
                var manager = new PlayerManager();
                var player = manager.Logon(dto.Token);
                var loginInfo = new LoginResponseDTO { Email = player.Email, Name = player.Name, Token = player.Token };

                return new JsonMessage(true, JsonConvert.SerializeObject(loginInfo));
            }
            catch (Exception e)
            {
                LogManager.LogError(e.Message);
                return new JsonMessage(false, e.Message);
            }
        }

        [HttpPost]
        public HttpResponseMessage ResetPassword(ResetPasswordDTO dto)
        {
            try
            {
                var manager = new PlayerManager();
                manager.ResetPassword(dto.Email);

                return new JsonMessage(true, "Password reset");
            }
            catch (Exception e)
            {
                LogManager.LogError(e.Message);
                return new JsonMessage(false, e.Message);
            }
        }
    }
}