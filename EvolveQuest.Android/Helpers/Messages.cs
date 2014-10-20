using Android.App;
using Android.Widget;
using EvolveQuest.Shared.Interfaces;
using System;
using System.Linq;

namespace EvolveQuest.Droid.Helpers
{
    public class Messages : IMessageDialog
    {
        public void SendToast(string toast)
        {
            App.CurrentActivity.RunOnUiThread(() => Toast.MakeText(App.CurrentActivity, toast, ToastLength.Short).Show());
        }

        public void SendMessage(string title, string message)
        {
            SendMessage(title, message, null);
        }

        public void SendMessage(string title, string message, Action completed)
        {
            App.CurrentActivity.RunOnUiThread(() => new AlertDialog.Builder(App.CurrentActivity)
        .SetMessage(message)
        .SetTitle(title ?? string.Empty)
        .SetPositiveButton("OK", delegate
                    {
                        if (completed != null)
                            completed();
                    })
        .Create()
        .Show());
        }

        public void EnterTextMessage(string title, string message, Action<string> completed)
        {
            App.CurrentActivity.RunOnUiThread(() =>
                {
                    var input = new EditText(App.CurrentActivity);
                    new AlertDialog.Builder(App.CurrentActivity)
          .SetTitle(title)
          .SetMessage(message)
          .SetView(input)
          .SetPositiveButton("OK", delegate
                        {
                            completed(input.Text);
                        })
          .SetNegativeButton("Cancel", delegate
                        {
                        })
          .Create()
          .Show();

                });
        }

        public void AskQuestions(string title, Shared.Models.Question question, Action<int> completed)
        {
            App.CurrentActivity.RunOnUiThread(() => new AlertDialog.Builder(App.CurrentActivity)
        .SetTitle(question.Text)
        .SetItems(question.Answers.Select(a => a.Text).ToArray(), (sender, args) =>
                    {
                        if (completed != null)
                            completed(args.Which);
                    })
        .SetNegativeButton("Cancel", delegate
                    {
                    })
        .Create()
        .Show());
        }
    }
}
