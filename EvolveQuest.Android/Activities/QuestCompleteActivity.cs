using Android.App;
using Android.OS;
using Android.Widget;
using Android.Content.PM;
using EvolveQuest.Shared.Helpers;

namespace EvolveQuest.Droid.Activities
{
    [Activity(Label = "Evolve Quest",
        ScreenOrientation = ScreenOrientation.Portrait,
        Theme = "@android:style/Theme.Holo.Light.NoActionBar")]
    public class QuestCompleteActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            App.CurrentActivity = this;
            SetContentView(Resource.Layout.quest_complete);
            // Create your application here
            Settings.QuestDone = true;

            var continueButton = FindViewById<Button>(Resource.Id.button_continue);
            continueButton.Click += (sender, args) =>
            {
                Finish(); 
                OverridePendingTransition(Resource.Animation.slide_in_down, Resource.Animation.slide_out_down);
            };

            var questNumber = FindViewById<TextView>(Resource.Id.text_quest_number);
            questNumber.Text = QuestActivity.ViewModel.CompletionDisplayShort;
        }


        public override void OnBackPressed()
        {
            base.OnBackPressed();
            OverridePendingTransition(Resource.Animation.slide_in_down, Resource.Animation.slide_out_down);
        }
    }
}