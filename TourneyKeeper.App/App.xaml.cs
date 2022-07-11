using Newtonsoft.Json;
using System;
using System.IO;
using TourneyKeeper.DTO.App;
using TourneyKeeper.Common;
using Xamarin.Forms;

namespace TourneyKeeper
{
    public partial class App : Application
    {
        private static DateTime LastLogin = DateTime.MinValue;
        public static string PlayerToken = null;
        public static string FirebaseToken = null;

        public App()
        {
            InitializeComponent();
        }

        protected override void OnStart()
        {
        }

        public void Navigate(string token)
        {
            if (token != null)
            {
                MainPage = new NavigationPage(new Games(token));
            }
            else
            {
                MainPage = new NavigationPage(new Login());
            }
        }

        public string VerifyLogin()
        {
            var io = DependencyService.Get<ISaveAndLoad>();
            try
            {
                var tmp = io.LoadText("player.txt");
                if (tmp != null)
                {
                    var service = new TKREST();
                    var playerInfo = JsonConvert.DeserializeObject<LoginResponseDTO>(tmp);
                    if (DateTime.Now.AddHours(-1) > LastLogin)
                    {
                        var response = service.Post<VerifyLoginDTO, LoginResponseDTO>(TKREST.VerifyLogin, new VerifyLoginDTO { Token = playerInfo.Token });
                        if (response != null)
                        {
                            io.SaveText("player.txt", JsonConvert.SerializeObject(response));

                            return response.Token;
                        }
                        else
                        {
                            return null;
                        }
                    }

                    return playerInfo.Token;
                }
                else
                {
                    return null;
                }
            }
            catch (FileNotFoundException)
            {
                return null;
            }
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
