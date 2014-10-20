
using System.Collections.Generic;
using System.Threading.Tasks;



#if __ANDROID__
using Android.Graphics;
using Android.Widget;
using EvolveQuest.Shared.Helpers;

#elif __IOS__
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;
using System.Drawing;
using EvolveQuest.Shared.Helpers;

using EvolveQuest.Shared.Extensions;
#endif

namespace EvolveQuest.Shared.Extensions
{
    public static class Images
    {
        #if __ANDROID__
        public static float ScreenWidth = 320;
        static Dictionary<string, Bitmap> bmpCache = new Dictionary<string, Bitmap>();

        public static async Task<Bitmap> SetImageFromUrlAsync(this ImageView imageView, string url)
        {
            var bmp = FromUrl(url);
            if (bmp.IsCompleted)
                return bmp.Result;
		
            return await bmp;
        }

        public static async Task<Bitmap> FromUrl(string url)
        {
            Bitmap bmp;
            if (bmpCache.TryGetValue(url, out bmp))
                return bmp;
            var path = await FileCache.Download(url);
            if (string.IsNullOrEmpty(path))
                return null;
            bmp = await BitmapFactory.DecodeFileAsync(path);
            bmpCache[url] = bmp;
            return bmp;
        }
        #elif __IOS__
        

		public static async Task LoadUrl(this UIImageView imageView, string url)
		{	
			if (string.IsNullOrEmpty (url))
				return;
			var progress = new UIActivityIndicatorView (UIActivityIndicatorViewStyle.WhiteLarge)
			{
				Center = new PointF(imageView.Bounds.GetMidX(), imageView.Bounds.GetMidY()),
			};
			imageView.AddSubview (progress);


			var t = FileCache.Download (url);
			if (t.IsCompleted) {
				imageView.Image = UIImage.FromFile(t.Result);
				progress.RemoveFromSuperview ();
				return;
			}
			progress.StartAnimating ();
			var image = UIImage.FromFile(await t);

			UIView.Animate (.3, 
				() => imageView.Image = image,
				() => {
					progress.StopAnimating ();
					progress.RemoveFromSuperview ();
				});
		}
		#endif
    }
}