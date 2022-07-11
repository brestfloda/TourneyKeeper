using System;
using Xamarin.Forms;
using System.IO;
using System.Threading.Tasks;
using TourneyKeeper.Droid;
using TourneyKeeper.Common;
using Android.Widget;
using Android.App;

[assembly: Dependency(typeof(TourneyKeeper.Droid.Dialog))]
namespace TourneyKeeper.Droid
{
    public class Dialog : IDialog
    {
        Activity mcontext;
        public Dialog() : base()
        {
            this.mcontext = MainActivity.Main;
        }

        public Task<MessageResult> ShowDialog(string Title, string Message, bool SetCancelable = false, bool SetInverseBackgroundForced = false, MessageResult PositiveButton = MessageResult.OK, MessageResult NegativeButton = MessageResult.NONE, MessageResult NeutralButton = MessageResult.NONE)
        {
            var tcs = new TaskCompletionSource<MessageResult>();

            var builder = new AlertDialog.Builder(mcontext);
            builder.SetIconAttribute(Android.Resource.Attribute.AlertDialogIcon);
            builder.SetTitle(Title);
            builder.SetMessage(Message);
            builder.SetCancelable(SetCancelable);

            builder.SetPositiveButton((PositiveButton != MessageResult.NONE) ? PositiveButton.ToString() : string.Empty, (senderAlert, args) =>
            {
                tcs.SetResult(PositiveButton);
            });
            builder.SetNegativeButton((NegativeButton != MessageResult.NONE) ? NegativeButton.ToString() : string.Empty, delegate
            {
                tcs.SetResult(NegativeButton);
            });
            builder.SetNeutralButton((NeutralButton != MessageResult.NONE) ? NeutralButton.ToString() : string.Empty, delegate
            {
                tcs.SetResult(NeutralButton);
            });

            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
            {
                builder.Show();
            });

            // builder.Show();
            return tcs.Task;
        }
    }
}
