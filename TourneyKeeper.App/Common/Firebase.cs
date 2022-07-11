using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using TourneyKeeper.DTO.App;

namespace TourneyKeeper.Common
{
    public class Firebase
    {
        public static void SendRegistrationToServer(string playerToken)
        {
            try
            {
                var rest = new TKREST();
                var dto = new RegisterClientDTO { ClientToken = App.FirebaseToken, PlayerToken = playerToken };
                rest.Post<RegisterClientDTO, ResponseDTO>(TKREST.Registerclient, dto);
            }
            catch (Exception e)
            {
                Crashes.TrackError(e, new Dictionary<string, string> { { "Token", playerToken } });
                throw;
            }
        }
    }
}