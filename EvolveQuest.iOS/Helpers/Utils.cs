using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace EvolveQuest.iOS.Helpers
{
    public static class Utils
    {
        public static NSObject Invoker;

        /// <summary>
        /// Ensures the invoked on main thread.
        /// </summary>
        /// <param name="action">Action to run on main thread.</param>
        public static void EnsureInvokedOnMainThread(Action action)
        {
            if (NSThread.Current.IsMainThread)
            {
                action();
                return;
            }
            if (Invoker == null)
                Invoker = new NSObject();

            Invoker.BeginInvokeOnMainThread(() => action());
        }

        public static bool IsiOS8
        {
            get
            {
                return UIDevice.CurrentDevice.CheckSystemVersion(8, 0);
            }

        }

    }
}