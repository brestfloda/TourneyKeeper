using System;
using Android.App;
using Android.Content;
using Android.Media;
using Android.Util;
using Firebase.Messaging;

namespace TourneyKeeper.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class TKFirebaseMessagingService : FirebaseMessagingService
    {
        public override void OnMessageReceived(RemoteMessage message)
        {
            Log.Debug("OnMessageReceived", "From: " + message.From);
            Log.Debug("OnMessageReceived", "Notification Message Body: " + message.GetNotification().Body);

            new Notifier().Notify(message.GetNotification().Title, message.GetNotification().Body);
        }
    }
}
