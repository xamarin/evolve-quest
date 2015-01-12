using System;
using Foundation;
using UIKit;
using EvolveQuest.Shared.ViewModels;
using EvolveQuest.Shared.Helpers;
using System.Drawing;

namespace EvolveQuest.iOS
{
    partial class QuestCodeViewController : UIViewController
    {
        private UIView activeview;
        // Controller that activated the keyboard
        private float scrollamount;
        // amount to scroll
        private float bottom;
        // bottom point
        private const float Offset = 68.0f;
        // extra offset
        private bool moveViewUp;
        // which direction are we moving
        public QuestViewModel ViewModel { get; set; }

        public QuestCodeViewController(IntPtr handle)
            : base(handle)
        {
        }

        partial void ButtonCancel_TouchUpInside(UIButton sender)
        {
            TextFieldCode.ResignFirstResponder();
            DismissViewControllerAsync(true);
        }

        partial void ButtonEnterCode_TouchUpInside(UIButton sender)
        {
            if (ViewModel.QuestComplete)
            {
                DismissViewControllerAsync(true);
                return;
            }

            TextFieldCode.ResignFirstResponder();
            ViewModel.ExtraTaskText = TextFieldCode.Text.Trim();
            ViewModel.CheckCode(ViewModel.ExtraTaskText);
            if (ViewModel.QuestComplete)
            {
                ButtonCancel.Hidden = TextFieldCode.Hidden = true;
                ButtonEnterCode.SetTitle("Continue", UIControlState.Normal);
                LabelHint.Text = "Are you ready to continue with the next quest, thrill-seeker?";
                LabelAwesome.Text = "That's it!";
                LabelCongrats.Text = "You entered the correct code.";
                Settings.QuestDone = true;
            }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            TextFieldCode.ShouldReturn += (textField) =>
            {
                TextFieldCode.ResignFirstResponder();
                ButtonEnterCode_TouchUpInside(null);
                return true;
            };
            ButtonEnterCode.Layer.CornerRadius = ButtonCancel.Layer.CornerRadius = 5;
            TextFieldCode.Layer.BorderColor = EvolveQuest.Shared.Helpers.Color.Blue.ToCGColor();

            // Keyboard popup
            NSNotificationCenter.DefaultCenter.AddObserver
      (UIKeyboard.DidShowNotification, KeyBoardUpNotification);

            // Keyboard Down
            NSNotificationCenter.DefaultCenter.AddObserver
      (UIKeyboard.WillHideNotification, KeyBoardDownNotification);

        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            LabelQuestNumber.Text = ViewModel.CompletionDisplayShort;
            if (!string.IsNullOrWhiteSpace(ViewModel.Quest.CodeHint))
                LabelHint.Text = ViewModel.Quest.CodeHint;
        }

        private void KeyBoardDownNotification(NSNotification notification)
        {
            if (moveViewUp)
            {
                ScrollTheView(false);
            }
        }

        private void ScrollTheView(bool move)
        {
 
            // scroll the view up or down
            UIView.BeginAnimations(string.Empty, System.IntPtr.Zero);
            UIView.SetAnimationDuration(0.3);
 
            var frame = View.Frame;
 
            if (move)
            {
                frame.Y -= scrollamount;
            }
            else
            {
                frame.Y += scrollamount;
                scrollamount = 0;
            }
 
            View.Frame = frame;
            UIView.CommitAnimations();
        }

        private void KeyBoardUpNotification(NSNotification notification)
        {
            // get the keyboard size
            var r = UIKeyboard.FrameBeginFromNotification(notification);
    
      
            // Find what opened the keyboard
            foreach (UIView view in this.View.Subviews)
            {
                if (view.IsFirstResponder)
                    activeview = view;
            }

            // Bottom of the controller = initial position + height + offset      
            bottom = ((float)activeview.Frame.Y + (float)activeview.Frame.Height + Offset);

            // Calculate how far we need to scroll
            scrollamount = ((float)r.Height - ((float)View.Frame.Size.Height - bottom));

            // Perform the scrolling
            if (scrollamount > 0)
            {
                moveViewUp = true;
                ScrollTheView(moveViewUp);
            }
            else
            {
                moveViewUp = false;
            }

        }

    }
}
