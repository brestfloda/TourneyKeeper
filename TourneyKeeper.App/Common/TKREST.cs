using Newtonsoft.Json;
using TourneyKeeper.DTO.App;
using System.Net.Http;
using System.Text;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace TourneyKeeper.Common
{
    public class TKREST
    {
        private static readonly string baseClient = "https://tourneykeeper.net/webapi/";

        public static string Login { get; } = baseClient + "login/login";
        public static string VerifyLogin { get; } = baseClient + "login/verifylogin";
        public static string Registerclient { get; } = baseClient + "Notification/registerclient";
        public static string UpdateGame { get; } = baseClient + "game/updategame";
        public static string GetCurrentGames { get; } = baseClient + "game/getgames";
        public static string SelectOpponent { get; } = baseClient + "tournamentplayer/selectopponent";
        public static string GetOpponents { get; } = baseClient + "tournamentplayer/getopponents";

        private readonly HttpClient httpclient;

        public TKREST()
        {
            httpclient = new HttpClient
            {
                MaxResponseContentBufferSize = 256000
            };
        }

        public RESPONSE Post<REQUEST, RESPONSE>(string client, REQUEST dto) where REQUEST : BaseDTO where RESPONSE : class
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(dto).ToString(), Encoding.UTF8, "application/json");
                var response = httpclient.PostAsync(client, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    var tmp = response.Content.ReadAsStringAsync().Result;
                    var responseContent = JsonConvert.DeserializeObject<ResponseDTO>(tmp);
                    var message = JsonConvert.DeserializeObject<RESPONSE>(responseContent.Message);

                    if (!responseContent.Success)
                    {
                        var dialog = DependencyService.Get<IDialog>();
                        dialog.ShowDialog("Error", $"Internal error in {client}, data was {dto?.Describe()}, message is {responseContent.Message}");
                    }

                    return message;
                }
                else
                {
                    //var dialog = DependencyService.Get<IDialog>();
                    //dialog.ShowDialog("Error", $"HTTP error in {client}, data was {dto?.Describe()}");
                    return null;
                }
            }
            catch (Exception e)
            {
                //var dialog = DependencyService.Get<IDialog>();
                //dialog.ShowDialog("Error", $"Exception when calling {client}, data was {dto?.Describe()}");
                Crashes.TrackError(e, new Dictionary<string, string> { { "Client", client.Replace(baseClient, "") }, { "Description", dto?.Describe() } });
                return null;
            }
        }
    }
}
