// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using System;
using Foundation;
using UIKit;
using System.CodeDom.Compiler;

namespace EvolveQuest.iOS
{
	[Register ("QuestAskQuestionViewController")]
	partial class QuestAskQuestionViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton ButtonAnswer { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton ButtonCancel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel LabelAwesome { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel LabelCongrats { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel LabelHint { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel LabelQuestNumber { get; set; }

		[Action ("ButtonAnswer_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void ButtonAnswer_TouchUpInside (UIButton sender);

		[Action ("ButtonCancel_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void ButtonCancel_TouchUpInside (UIButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (ButtonAnswer != null) {
				ButtonAnswer.Dispose ();
				ButtonAnswer = null;
			}
			if (ButtonCancel != null) {
				ButtonCancel.Dispose ();
				ButtonCancel = null;
			}
			if (LabelAwesome != null) {
				LabelAwesome.Dispose ();
				LabelAwesome = null;
			}
			if (LabelCongrats != null) {
				LabelCongrats.Dispose ();
				LabelCongrats = null;
			}
			if (LabelHint != null) {
				LabelHint.Dispose ();
				LabelHint = null;
			}
			if (LabelQuestNumber != null) {
				LabelQuestNumber.Dispose ();
				LabelQuestNumber = null;
			}
		}
	}
}
