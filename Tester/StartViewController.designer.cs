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

namespace Tester
{
	[Register ("StartViewController")]
	partial class StartViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton Game1 { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton Game2 { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton Game3 { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (Game1 != null) {
				Game1.Dispose ();
				Game1 = null;
			}
			if (Game2 != null) {
				Game2.Dispose ();
				Game2 = null;
			}
			if (Game3 != null) {
				Game3.Dispose ();
				Game3 = null;
			}
		}
	}
}
