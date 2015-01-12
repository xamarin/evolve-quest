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
	[Register ("QuestViewController2")]
	partial class QuestViewController2
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIImageView Beacon1 { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIImageView Beacon2 { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIImageView Beacon3 { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton ButtonContinueQuest { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel LabelBeaconsNearby { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIImageView MainImage { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel MainText { get; set; }

		[Action ("ButtonContinueQuest_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void ButtonContinueQuest_TouchUpInside (UIButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (Beacon1 != null) {
				Beacon1.Dispose ();
				Beacon1 = null;
			}
			if (Beacon2 != null) {
				Beacon2.Dispose ();
				Beacon2 = null;
			}
			if (Beacon3 != null) {
				Beacon3.Dispose ();
				Beacon3 = null;
			}
			if (ButtonContinueQuest != null) {
				ButtonContinueQuest.Dispose ();
				ButtonContinueQuest = null;
			}
			if (LabelBeaconsNearby != null) {
				LabelBeaconsNearby.Dispose ();
				LabelBeaconsNearby = null;
			}
			if (MainImage != null) {
				MainImage.Dispose ();
				MainImage = null;
			}
			if (MainText != null) {
				MainText.Dispose ();
				MainText = null;
			}
		}
	}
}
