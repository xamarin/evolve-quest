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
	[Register ("QuestCompletedViewController")]
	partial class QuestCompletedViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton ButtonContinue { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel LabelQuestNumber { get; set; }

		[Action ("ButtonContinue_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void ButtonContinue_TouchUpInside (UIButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (ButtonContinue != null) {
				ButtonContinue.Dispose ();
				ButtonContinue = null;
			}
			if (LabelQuestNumber != null) {
				LabelQuestNumber.Dispose ();
				LabelQuestNumber = null;
			}
		}
	}
}
