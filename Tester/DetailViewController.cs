using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using EvolveQuest.Shared.Extensions;
using MonoTouch.CoreLocation;
using MonoTouch.CoreBluetooth;
using MonoTouch.CoreFoundation;

namespace Tester
{
  public partial class DetailViewController : UIViewController
  {
    Beacon detailItem;

    public DetailViewController(IntPtr handle)
      : base(handle)
    {
    }

    public void SetDetailItem(Beacon newDetailItem)
    {
      if (detailItem != newDetailItem)
      {
        detailItem = newDetailItem;

        // Update the view
        ConfigureView();
      }
    }

    void ConfigureView()
    {
      // Update the user interface for the detail item
      if (IsViewLoaded && detailItem != null)
      {
        detailDescriptionLabel.Text = detailItem.ToString();
      }

      this.Title = detailItem.Code;

      if(peripheralMgr != null)
      {
        peripheralMgr.StopAdvertising();
      }
      else
      {
        peripheralDelegate = new BTPeripheralDelegate();
        peripheralMgr = new CBPeripheralManager(peripheralDelegate, DispatchQueue.DefaultGlobalQueue);
      }

      beaconUUID = new NSUuid(detailItem.UUID);
      beaconRegion = new CLBeaconRegion(beaconUUID, (ushort)detailItem.Major, (ushort)detailItem.Minor, beaconId);



      //power - the received signal strength indicator (RSSI) value (measured in decibels) of the beacon from one meter away
      var power = new NSNumber(-59);
      peripheralData = beaconRegion.GetPeripheralData(power);
      peripheralMgr.StartAdvertising(peripheralData);

      QRCode.LoadUrl(GenerateQRCodeUrl(detailItem.ToString(), QRCodeSize.Medium, QRErrorCorrection.H));
    }

    public enum QRCodeSize
    {
      Small = 120,
      Medium = 230,
      Large = 350
    }

    public enum QRErrorCorrection
    {
      L,
      M,
      Q,
      H
    }

    private string GenerateQRCodeUrl(string content, QRCodeSize size, QRErrorCorrection errorCorrection)
    {
      var baseUrl = "http://chart.apis.google.com/chart?cht=qr&chs={0}x{0}&chld={1}&choe=UTF-8&chl={2}";

      return string.Format(baseUrl, (int)size, errorCorrection.ToString(), Uri.EscapeDataString(content));
    }

    public override void ViewWillDisappear(bool animated)
    {
      base.ViewWillDisappear(animated);
      if (peripheralMgr != null)
      {
        peripheralMgr.StopAdvertising();
      }
    }

    NSUuid beaconUUID;
    CLBeaconRegion beaconRegion;
    const string beaconId = "com.refractored";

    CBPeripheralManager peripheralMgr;

    NSMutableDictionary peripheralData;
    BTPeripheralDelegate peripheralDelegate;

    class BTPeripheralDelegate : CBPeripheralManagerDelegate
    {
      public override void StateUpdated(CBPeripheralManager peripheral)
      {
        if (peripheral.State == CBPeripheralManagerState.PoweredOn)
        {
          Console.WriteLine("powered on");
        }
      }
    }

    public override void ViewDidLoad()
    {
      base.ViewDidLoad();

      // Perform any additional setup after loading the view, typically from a nib.
      ConfigureView();
    }
  }
}
