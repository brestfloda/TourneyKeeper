using System;
using Android.App;
using Firebase.Iid;
using Android.Util;
using TourneyKeeper.Common;
using TourneyKeeper.DTO.App;
using Newtonsoft.Json;
using Android.Content;

namespace TourneyKeeper.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class FirebaseIIDService : FirebaseInstanceIdService
    {
        public override void OnTokenRefresh()
        {
            App.FirebaseToken = FirebaseInstanceId.Instance.Token;

            try
            {
                var io = new SaveAndLoad();
                var tmp = io.LoadText("player.txt");
                if (tmp != null)
                {
                    var service = new TKREST();
                    var playerInfo = JsonConvert.DeserializeObject<LoginResponseDTO>(tmp);

                    Common.Firebase.SendRegistrationToServer(playerInfo.Token);
                }
            }
            catch (Exception e)
            {
                //Den er nok bare ikke klar endeby
                Log.Debug($"OnTokenRefresh", $"{e.Message}");
            }
        }
    }
}