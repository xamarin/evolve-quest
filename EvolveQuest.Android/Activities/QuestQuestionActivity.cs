using Android.Content.PM;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using EvolveQuest.Shared.Interfaces;
using EvolveQuest.Shared.Helpers;

namespace EvolveQuest.Droid.Activities
{
    [Activity(Label = "Evolve Quest",
        ScreenOrientation = ScreenOrientation.Portrait,
        Theme = "@android:style/Theme.Holo.Light.NoActionBar")]
    public class QuestQuestionActivity : Activity
    {
        private IMessageDialog messages;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            App.CurrentActivity = this;
            messages = ServiceContainer.Resolve<IMessageDialog>();
            SetContentView(Resource.Layout.quest_question);
            // Create your application here

            var cancelButton = FindViewById<Button>(Resource.Id.button_cancel);
            cancelButton.Click += (sender, args) =>
            {
                Finish();
                OverridePendingTransition(Resource.Animation.slide_in_down, Resource.Animation.slide_out_down);
            };

            var answerButton = FindViewById<Button>(Resource.Id.button_answer);
            var labelHint = FindViewById<TextView>(Resource.Id.hint);
            var labelAwesome = FindViewById<TextView>(Resource.Id.awesome);
            var labelCongrats = FindViewById<TextView>(Resource.Id.congrats);
            labelHint.Text = QuestActivity.ViewModel.Quest.Question.Text;
            answerButton.Click += (sender, args) =>
            {
                if (QuestActivity.ViewModel.QuestComplete)
                {
                    Finish();
                    OverridePendingTransition(Resource.Animation.slide_in_down, Resource.Animation.slide_out_down);
      
                    return;
                }

                messages.AskQuestions("Question:", QuestActivity.ViewModel.Quest.Question, (answer) =>
                    {
                        QuestActivity.ViewModel.CheckAnswer(answer);
                        if (QuestActivity.ViewModel.QuestComplete)
                        {
                            cancelButton.Visibility = ViewStates.Invisible;
                            answerButton.SetText(Resource.String.continue_game);
                            labelHint.SetText(Resource.String.continue_quest);
                            labelAwesome.SetText(Resource.String.thats_it);
                            labelCongrats.SetText(Resource.String.correct_answer);
                            Settings.QuestDone = true;
                        }
                    });

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