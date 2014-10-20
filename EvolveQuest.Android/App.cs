using Android.App;
using EvolveQuest.Droid.Helpers;
using EvolveQuest.Shared.Helpers;
using EvolveQuest.Shared.Interfaces;
using System;

namespace EvolveQuest.Droid
{
    #if DEBUG
    [Application(Theme = "@style/Theme.Quest", Debuggable = true)]
    #else
	[Application(Theme = "@style/Theme.Quest", Debuggable=false)]
	#endif
  public class App : Application
    {
    
        public static Activity CurrentActivity { get; set; }

        public App(IntPtr handle, global::Android.Runtime.JniHandleOwnership transer)
            : base(handle, transer)
        {

        }

        public override void OnCreate()
        {
            base.OnCreate();
            FileCache.SaveLocation = CacheDir.AbsolutePath;
            ServiceContainer.Register<IMessageDialog>(new Messages());
            Xamarin.Insights.Initialize("6978a61611a7e9f6fd20172582cb56911ee3131c", this);
            #if DEBUG
            Xamarin.Insights.DisableCollection = true;
            Xamarin.Insights.DisableDataTransmission = true;
            Xamarin.Insights.DisableExceptionCatching = true;
            #endif
        }
    }
}
