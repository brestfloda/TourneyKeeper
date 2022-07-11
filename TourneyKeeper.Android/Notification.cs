using Xamarin.Forms;
using TourneyKeeper.Droid;
using TourneyKeeper.Common;
using Android.Content;
using Android.Support.V4.App;
using Android.App;
using Android.Media;

[assembly: Dependency(typeof(Notifier))]
namespace TourneyKeeper.Droid
{
    public class Notifier : INotifier
    {
        private static readonly int NotificationId = 1001;

        public void Notify(string title, string message)
        {
            var intent = Android.App.Application.Context.PackageManager.GetLaunchIntentForPackage(Android.App.Application.Context.PackageName);
            intent.AddFlags(ActivityFlags.ClearTop);

            var pendingIntent = PendingIntent.GetActivity(Android.App.Application.Context, 0, intent, PendingIntentFlags.UpdateCurrent);

            NotificationCompat.Builder builder = new NotificationCompat.Builder(Android.App.Application.Context, MainActivity.MAINCHANNEL)
                .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification))
                .SetContentTitle(title)
                .SetContentText(message)
                .SetSmallIcon(Resource.Drawable.icon)
                .SetContentIntent(pendingIntent);

            Notification notification = builder.Build();

            MainActivity.NotificationManager.Notify(NotificationId, notification);
        }
    }
}