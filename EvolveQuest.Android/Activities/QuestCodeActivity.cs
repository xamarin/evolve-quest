using System;
using Android.Content.PM;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using EvolveQuest.Shared.Helpers;
using Android.Views.InputMethods;

namespace EvolveQuest.Droid.Activities
{
    [Activity(Label = "Evolve Quest",
        ScreenOrientation = ScreenOrientation.Portrait,
        Theme = "@android:style/Theme.Holo.Light.NoActionBar")]
    public class QuestCodeActivity : Activity
    {
        Button cancelButton, codeButton;
        TextView labelHint, labelAwesome, labelCongrats;
        EditText code;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle); 
            App.CurrentActivity = this;
            SetContentView(Resource.Layout.quest_code);
            // Create your application here

            cancelButton = FindViewById<Button>(Resource.Id.button_cancel);
            cancelButton.Click += (sender, args) =>
            {
                Finish();
                OverridePendingTransition(Resource.Animation.slide_in_down, Resource.Animation.slide_out_down);
            };

            codeButton = FindViewById<Button>(Resource.Id.button_enter);
            labelHint = FindViewById<TextView>(Resource.Id.hint);
            labelAwesome = FindViewById<TextView>(Resource.Id.awesome);
            labelCongrats = FindViewById<TextView>(Resource.Id.congrats);
            code = FindViewById<EditText>(Resource.Id.code);

            code.EditorAction += (sender, args) =>
            {
                var imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
                imm.HideSoftInputFromWindow(code.WindowToken, 0);
                if (args.ActionId == Android.Views.InputMethods.ImeAction.Done)
                {
                    CodeButtonClick(null, null);
                }
            };
            codeButton.Click += CodeButtonClick;

            var questNumber = FindViewById<TextView>(Resource.Id.text_quest_number);
            questNumber.Text = QuestActivity.ViewModel.CompletionDisplayShort;

            if (!string.IsNullOrWhiteSpace(QuestActivity.ViewModel.Quest.CodeHint))
                labelHint.Text = QuestActivity.ViewModel.Quest.CodeHint;
        }

        void CodeButtonClick(object sender, EventArgs e)
        {
            if (QuestActivity.ViewModel.QuestComplete)
            {
                Finish();
                OverridePendingTransition(Resource.Animation.slide_in_down, Resource.Animation.slide_out_down);
                return;
            }

            QuestActivity.ViewModel.ExtraTaskText = code.Text.Trim();
            QuestActivity.ViewModel.CheckCode(QuestActivity.ViewModel.ExtraTaskText);
            if (QuestActivity.ViewModel.QuestComplete)
            {
                cancelButton.Visibility = code.Visibility = ViewStates.Invisible;
                codeButton.SetText(Resource.String.continue_game);
                labelHint.SetText(Resource.String.continue_quest);
                labelAwesome.SetText(Resource.String.thats_it);
                labelCongrats.SetText(Resource.String.correct_code);

                Settings.QuestDone = true;
            }
        }


        public override void OnBackPressed()
        {
            base.OnBackPressed();
            OverridePendingTransition(Resource.Animation.slide_in_down, Resource.Animation.slide_out_down);
        }
    }
}