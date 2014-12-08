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
	[Register ("GameCompleteViewController")]
	partial class GameCompleteViewController
	{
		[Outlet]
		UIKit.UIButton ButtonShare { get; set; }

		[Outlet]
		UIKit.UIImageView ImageMain { get; set; }

		[Outlet]
		UIKit.UIImageView ImageSecret { get; set; }

		[Outlet]
		UIKit.UILabel LabelMessage { get; set; }

		[Outlet]
		UIKit.UIActivityIndicatorView ProgressBar { get; set; }

		[Action ("ButtonShare_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void ButtonShare_TouchUpInside (UIButton sender);

		void ReleaseDesignerOutlets ()
		{
		}
	}
}
