// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.CodeDom.Compiler;

namespace EvolveQuest.iOS
{
	[Register ("WelcomeViewController")]
	partial class WelcomeViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton ButtonAbout { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton ButtonLoadGame { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIImageView MapImage { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIActivityIndicatorView ProgressBar { get; set; }

		[Action ("ButtonAbout_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void ButtonAbout_TouchUpInside (UIButton sender);

		[Action ("ButtonLoadGame_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void ButtonLoadGame_TouchUpInside (UIButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (ButtonAbout != null) {
				ButtonAbout.Dispose ();
				ButtonAbout = null;
			}
			if (ButtonLoadGame != null) {
				ButtonLoadGame.Dispose ();
				ButtonLoadGame = null;
			}
			if (MapImage != null) {
				MapImage.Dispose ();
				MapImage = null;
			}
			if (ProgressBar != null) {
				ProgressBar.Dispose ();
				ProgressBar = null;
			}
		}
	}
}
