using System;
using Foundation;
using UIKit;
using EvolveQuest.Shared.Helpers;
using EvolveQuest.Shared.Interfaces;
using Tester.Helpers;

namespace Tester
{
  // The UIApplicationDelegate for the application. This class is responsible for launching the 
  // User Interface of the application, as well as listening (and optionally responding) to 
  // application events from iOS.
  [Register("AppDelegate")]
  public partial class AppDelegate : UIApplicationDelegate
  {
    // class-level declarations

    public override UIWindow Window
    {
      get;
      set;
    }

    public override void FinishedLaunching(UIApplication application)
    {
			Xamarin.Insights.Initialize("6978a61611a7e9f6fd20172582cb56911ee3131c");
			#if DEBUG
			Xamarin.Insights.DisableCollection = true;
			Xamarin.Insights.DisableDataTransmission = true;
			Xamarin.Insights.DisableExceptionCatching = true;
			#endif

      ServiceContainer.Register<IMessageDialog>(new Messages());
			
      FileCache.SaveLocation = System.IO.Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.Personal)).ToString() + "/tmp";
		
    }
  }
}