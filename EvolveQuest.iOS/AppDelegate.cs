using System;
using Foundation;
using UIKit;
using EvolveQuest.Shared.Helpers;
using EvolveQuest.Shared.Interfaces;
using EvolveQuest.iOS.Helpers;

namespace EvolveQuest.iOS
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
            UINavigationBar.Appearance.BackgroundColor = UIColor.FromRGBA(0, 0, 0, 0);
            UINavigationBar.Appearance.TintColor = UIColor.White;
            UINavigationBar.Appearance.SetTitleTextAttributes(new UITextAttributes()
                {
                    TextColor = UIColor.White
                });
            UINavigationBar.Appearance.BarTintColor = EvolveQuest.Shared.Helpers.Color.Blue.ToUIColor();
 
            #if DEBUG || ADHOC
            //Xamarin.Calabash.Start();
            #endif

            ServiceContainer.Register<IMessageDialog>(new Messages());
            #if !__UNIFIED__
            Xamarin.Insights.Initialize("6978a61611a7e9f6fd20172582cb56911ee3131c");
            #if DEBUG
            Xamarin.Insights.DisableCollection = true;
            Xamarin.Insights.DisableDataTransmission = true;
            Xamarin.Insights.DisableExceptionCatching = true;
            #endif
            #endif
            FileCache.SaveLocation = System.IO.Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.Personal)).ToString() + "/tmp";
        }
    }
}

