using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.CodeDom.Compiler;

namespace Tester
{
	partial class StartViewController : UIViewController
	{
		public StartViewController (IntPtr handle) : base (handle)
		{
		}

    public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
    {
      if (segue.Identifier == "Game1")
      {
        ((MasterViewController)segue.DestinationViewController).Game = 0;
      }
      else if (segue.Identifier == "Game2")
      {
        ((MasterViewController)segue.DestinationViewController).Game = 1;
      }
      else if (segue.Identifier == "Game3")
      {
        ((MasterViewController)segue.DestinationViewController).Game = 2;
      }
    }
	}
}
