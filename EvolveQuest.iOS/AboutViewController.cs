using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;

namespace EvolveQuest.iOS
{
    public class AboutViewController : DialogViewController
    {
        public AboutViewController()
            : base(UITableViewStyle.Grouped, null, true)
        {

        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            NavigationController.NavigationBarHidden = false;
            Root = new RootElement("About")
            {
                new Section("Evolve Quest")
                {
					
                    new HtmlElement("C# and Xamarin", NSUrl.FromString("http://www.xamarin.com")),
                    new HtmlElement("Privacy Policy", NSUrl.FromString("http://www.xamarin.com/privacy")),
                },
                new Section("Technology Use")
                {
                    new HtmlElement("ZXing.NET Barcode Scanning", NSUrl.FromString("https://components.xamarin.com/view/zxing.net.mobile")),
                    new HtmlElement("Xam.PCL Settings", NSUrl.FromString("https://www.nuget.org/packages/Xam.Plugins.Settings/")),
                    new HtmlElement("Discreet Notifications", NSUrl.FromString("https://components.xamarin.com/view/gcdiscreetnotification")),
                    new HtmlElement("Json.NET", NSUrl.FromString("https://components.xamarin.com/view/json.net")),
                },
            };
        }
    }
}