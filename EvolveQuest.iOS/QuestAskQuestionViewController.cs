using System;
using UIKit;
using EvolveQuest.Shared.ViewModels;
using EvolveQuest.Shared.Interfaces;
using EvolveQuest.Shared.Helpers;

namespace EvolveQuest.iOS
{
    partial class QuestAskQuestionViewController : UIViewController
    {
        public QuestViewModel ViewModel { get; set; }

        public QuestAskQuestionViewController(IntPtr handle)
            : base(handle)
        {
        }

        partial void ButtonCancel_TouchUpInside(UIButton sender)
        {
            DismissViewControllerAsync(true);
        }

        partial void ButtonAnswer_TouchUpInside(UIButton sender)
        {
            if (ViewModel.QuestComplete)
            {
                DismissViewControllerAsync(true);
                return;
            }

            messages.AskQuestions("Question:", ViewModel.Quest.Question, (answer) =>
                {
                    ViewModel.CheckAnswer(answer);
                    if (ViewModel.QuestComplete)
                    {
                        ButtonCancel.Hidden = true;
                        ButtonAnswer.SetTitle("Continue", UIControlState.Normal);
                        LabelHint.Text = "Are you ready to continue with the next quest, thrill-seeker?";
                        LabelAwesome.Text = "That's it!";
                        LabelCongrats.Text = "You answered the question correctly, noble one.";
                        Settings.QuestDone = true;
                    }
                });
      
        }

        private IMessageDialog messages;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            messages = ServiceContainer.Resolve<IMessageDialog>();
            ButtonAnswer.Layer.CornerRadius = ButtonCancel.Layer.CornerRadius = 5;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            LabelQuestNumber.Text = ViewModel.CompletionDisplayShort;
            LabelHint.Text = ViewModel.Quest.Question.Text;
        }
    }
}
