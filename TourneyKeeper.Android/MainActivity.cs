using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Gms.Common;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Android.Util;
using Firebase.Iid;

namespace TourneyKeeper.Droid
{
    [Activity(Label = "TourneyKeeper", Icon = "@drawable/icon", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public const string MAINCHANNEL = "net.tourneykeeper.mainchannel";
        public static NotificationManager NotificationManager;
        public static MainActivity Main;

        public static bool CheckPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(Main);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                {
                    Log.Debug("IsPlayServicesAvailable - Error", $"{GoogleApiAvailability.Instance.GetErrorString(resultCode)}");
                }
                else
                {
                    Log.Debug("IsPlayServicesAvailable - Error", $"This device is not supported");
                    Main.Finish();
                }
                return false;
            }
            else
            {
                Log.Debug("IsPlayServicesAvailable - Info", $"Google Play Services is available");
                return true;
            }
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            //Log.Debug("Debug", "google app id: " + Resource.String.google_app_id);

            Main = this;

            AppCenter.Start("c5ddafcf-18b7-45ec-b97e-5d3ec74432c5", typeof(Analytics), typeof(Crashes));

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            global::Xamarin.Forms.Forms.Init(this, bundle);

            CheckPlayServicesAvailable();

            NotificationManager = (NotificationManager)GetSystemService(NotificationService);
            try
            {
                var chan = new NotificationChannel(MAINCHANNEL, "New game", NotificationImportance.Default)
                {
                    LockscreenVisibility = NotificationVisibility.Private
                };
                NotificationManager.CreateNotificationChannel(chan);
            }
            catch
            {
                //Det er sgu sikkert ikke supportet...
            }

            App.FirebaseToken = FirebaseInstanceId.Instance.Token;
            var app = new App();
            var token = app.VerifyLogin();
            App.PlayerToken = token;
            if (token != null)
            {
               Common.Firebase.SendRegistrationToServer(token);
            }
            app.Navigate(token);
            LoadApplication(app);
        }
    }
}

