using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using UIKit;

namespace TourneyKeeper.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public const string MAINCHANNEL = "net.tourneykeeper.mainchannel";
        //public static NotificationManager NotificationManager;
        public static AppDelegate Main;

        public static bool CheckPlayServicesAvailable()
        {
            //int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(Main);
            //if (resultCode != ConnectionResult.Success)
            //{
            //    if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
            //    {
            //        Log.Debug("IsPlayServicesAvailable - Error", $"{GoogleApiAvailability.Instance.GetErrorString(resultCode)}");
            //    }
            //    else
            //    {
            //        Log.Debug("IsPlayServicesAvailable - Error", $"This device is not supported");
            //        Main.Finish();
            //    }
            //    return false;
            //}
            //else
            //{
            //    Log.Debug("IsPlayServicesAvailable - Info", $"Google Play Services is available");
                return true;
            //}
        }

        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication uiApp, NSDictionary options)
        {
            Main = this;
            AppCenter.Start("c5ddafcf-18b7-45ec-b97e-5d3ec74432c5", typeof(Analytics), typeof(Crashes));

            global::Xamarin.Forms.Forms.Init();

            CheckPlayServicesAvailable();

            //NotificationManager = (NotificationManager)GetSystemService(Context.NotificationService);
            //try
            //{
            //    var chan = new NotificationChannel(MAINCHANNEL, "New game", NotificationImportance.Default)
            //    {
            //        LockscreenVisibility = NotificationVisibility.Private
            //    };
            //    NotificationManager.CreateNotificationChannel(chan);
            //}
            //catch
            //{
            //    //Det er sgu sikkert ikke supportet...
            //}

            var app = new App();
            var token = app.VerifyLogin();
            //App.FirebaseToken = FirebaseInstanceId.Instance.Token;
            if (token != null)
            {
                Common.Firebase.SendRegistrationToServer(token);
            }
            app.Navigate(token);
            LoadApplication(app);

            return base.FinishedLaunching(uiApp, options);
        }
    }
}
