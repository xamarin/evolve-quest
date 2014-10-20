using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreLocation;
using EvolveQuest.Shared.ViewModels;
using EvolveQuest.Shared.Extensions;
using System.Collections.Generic;
using System.Linq;
using EvolveQuest.iOS;
using EvolveQuest.Shared.Helpers;
using EvolveQuest.iOS.Helpers;

namespace EvolveQuest.iOS
{
    partial class QuestViewController2 : UIViewController
    {
        public QuestViewController2(IntPtr handle)
            : base(handle)
        {
        }

        CLBeaconRegion beaconRegion, secretRegion;
        const string BeaconId = "com.refractored";
        CLLocationManager manager, managerSecret;
        QuestViewModel viewModel = new QuestViewModel();
        UIImage undiscoveredBeacon;
        UIImage foundBeacon;
        UIBarButtonItem scanButton, loadingButton;
        List<UIImageView> beacons;
        ZXing.Mobile.MobileBarcodeScanner scanner;
        private bool ranging;
        UIActivityIndicatorView ProgressBar;
        ZXing.Mobile.MobileBarcodeScanningOptions options = new ZXing.Mobile.MobileBarcodeScanningOptions();

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            ButtonContinueQuest.Layer.CornerRadius = 5;

            beacons = new List<UIImageView> { Beacon1, Beacon2, Beacon3 };
            Beacon1.Hidden = Beacon2.Hidden = Beacon3.Hidden = true;

            ButtonContinueQuest.Hidden = true;

            ProgressBar = new UIActivityIndicatorView(new System.Drawing.RectangleF(0, 0, 20, 20))
            {
                HidesWhenStopped = true,
                ActivityIndicatorViewStyle = UIActivityIndicatorViewStyle.White
            };
            loadingButton = new UIBarButtonItem(ProgressBar);





            //Setup scanner
            scanner = new ZXing.Mobile.MobileBarcodeScanner();
            options.PossibleFormats = new List<ZXing.BarcodeFormat>()
            { 
                ZXing.BarcodeFormat.QR_CODE
            };
            scanButton = new UIBarButtonItem("Scan", UIBarButtonItemStyle.Plain, async (sender, args) =>
                {

                    var result = await scanner.Scan(options, true);

                    if (result == null)
                        return;

                    Console.WriteLine("Scanned Barcode: " + result.Text);
                    viewModel.CheckBeacon(result.Text);
                });



            NavigationItem.RightBarButtonItem = scanButton;


            undiscoveredBeacon = UIImage.FromBundle("ic_no_banana");
            foundBeacon = UIImage.FromBundle("ic_banana");

            viewModel.PropertyChanged += HandlePropertyChanged;

            manager = new CLLocationManager();
            if (Utils.IsiOS8)
                manager.RequestWhenInUseAuthorization();

            managerSecret = new CLLocationManager();
            if (Utils.IsiOS8)
                managerSecret.RequestWhenInUseAuthorization();
				

            manager.DidRangeBeacons += ManagerDidRangeBeacons;
            manager.RegionLeft += (sender, args) => SetBeaconText(false);
            manager.RegionEntered += (sender, args) => SetBeaconText(true);
            managerSecret.DidRangeBeacons += ManagerDidRangeBeacons;

            viewModel.LoadQuestCommand.Execute(null);
		
        }

        private void SetBeaconText(bool beaconsClose)
        {
            if (viewModel.ExtraTaskVisible)
            {
                LabelBeaconsNearby.Text = "You found all of the Beacons!";
                LabelBeaconsNearby.TextColor = Color.Blue.ToUIColor();      
                return;
            }

            if (beaconsClose)
            {
                LabelBeaconsNearby.Text = "A Beacon is near you.";
                LabelBeaconsNearby.TextColor = Color.Blue.ToUIColor();
            }
            else
            {
                LabelBeaconsNearby.Text = "No Beacons nearby.";
                LabelBeaconsNearby.TextColor = Color.LightGray.ToUIColor();
            }
        }

        void ManagerDidRangeBeacons(object sender, CLRegionBeaconsRangedEventArgs e)
        {
            if (e.Beacons == null)
            {
                SetBeaconText(false);
                return;
            }


            bool close = false;
            foreach (var beacon in e.Beacons)
            {
                if (beacon.Proximity != CLProximity.Unknown)
                    close = true;

                if (beacon.Proximity != CLProximity.Immediate)
                    continue;

                if (beacon.Accuracy > .1)//close, but not close enough.
					continue;

                viewModel.CheckBeacon(beacon.Major.Int32Value, beacon.Minor.Int32Value);
            }

            if (e.Region.Major.StringValue != secretRegion.Major.StringValue)
                SetBeaconText(close);
        }


        private void StopRanging()
        {
            if (!ranging)
                return;

            if (beaconRegion != null)
                manager.StopRangingBeacons(beaconRegion);

            if (secretRegion != null)
                managerSecret.StopRangingBeacons(secretRegion);
				
            ranging = false;
        }

        private void StartRanging()
        {

            if (viewModel == null || viewModel.Quest == null)
                return;

            if (ranging)
                return;


            if (!CLLocationManager.IsMonitoringAvailable(typeof(CLBeaconRegion)))
                return;


            ranging = true;

            if (secretRegion == null)
            {
                secretRegion = new CLBeaconRegion(new NSUuid(viewModel.UUID), 9999, BeaconId);
            }


            if (beaconRegion != null)
                manager.StartRangingBeacons(beaconRegion);

            managerSecret.StartRangingBeacons(secretRegion); 
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            StartRanging();
            SetBeaconText(false);
            NavigationController.NavigationBarHidden = false;
            if (viewModel.QuestComplete && Settings.QuestDone)
                viewModel.LoadNextLevel();
        }


        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            StopRanging();
        }

        void InitializeBeacons()
        {
            StopRanging();

            if (viewModel.Quest.Major >= 0)
            {
                beaconRegion = new CLBeaconRegion(new NSUuid(viewModel.UUID), (ushort)viewModel.Quest.Major, BeaconId);
                beaconRegion.NotifyOnExit = true;
                beaconRegion.NotifyOnEntry = true;
            }

            InvokeOnMainThread(() =>
                {
                    Beacon1.Image = Beacon2.Image = Beacon3.Image = undiscoveredBeacon;
                    Beacon1.Hidden = Beacon2.Hidden = Beacon3.Hidden = true;
                    SetBeaconText(false);
                    for (int i = 0; i < viewModel.Quest.Beacons.Count; i++)
                    {
                        beacons[i].Hidden = false;
                    }

                    UpdateBeacons();
                    MainImage.LoadUrl(viewModel.Quest.Clue.Image);
                    MainText.Text = viewModel.Quest.Clue.Message;
                });
            StartRanging();
        }

        void UpdateBeacons()
        {
            InvokeOnMainThread(() =>
                {
                    for (int i = 0; i < viewModel.Quest.Beacons.Count; i++)
                    {
                        var beacon = viewModel.Quest.Beacons.FirstOrDefault(b => b.Id == i);
                        if (beacon == null)
                            continue;
                        beacons[i].Image = beacon.Found ? foundBeacon : undiscoveredBeacon;
                    }

                    ButtonContinueQuest.Hidden = !viewModel.ExtraTaskVisible;
                });
        }

        void HandlePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            InvokeOnMainThread(() =>
                {
                    switch (e.PropertyName)
                    {
                        case QuestViewModel.ExtraTaskVisiblePropertyName:
                            ButtonContinueQuest.Hidden = !viewModel.ExtraTaskVisible;
                            break;
                        case BaseViewModel.IsBusyPropertyName:
                            if (viewModel.IsBusy)
                            {
                                NavigationItem.RightBarButtonItem = loadingButton;
                                ProgressBar.StartAnimating();
                            }
                            else
                            {
                                ProgressBar.StopAnimating();
                                NavigationItem.RightBarButtonItem = scanButton;
                            }
                            break;
                        case QuestViewModel.QuestPropertyName:
                        case QuestViewModel.NewQuestPropertyName:
                            InitializeBeacons();
                            break;
                        case QuestViewModel.QuestsPropertyName:
                            UpdateBeacons();
                            break;
                        case QuestViewModel.GameCompletePropertyName:
						//GoToGameCompleted();
                            break;
                        case QuestViewModel.CompletionDisplayPropertyName:
                            this.Title = viewModel.CompletionDisplay;
                            break;  
                    }
                });
        }

        private void GoToGameCompleted()
        {
            var storyboard = UIStoryboard.FromName("MainStoryboard", null);
            var vc = storyboard.InstantiateViewController("GameCompleteViewController") as UIViewController;

            NavigationController.PushViewController(vc, true);
        }

        partial void ButtonContinueQuest_TouchUpInside(UIButton sender)
        {
            if (viewModel.GameComplete)
            {
                GoToGameCompleted();
                return;
            }

            var storyboard = UIStoryboard.FromName("MainStoryboard", null);
            UIViewController vc = null;
            if (viewModel.CodeRequired)
            {

                var code = storyboard.InstantiateViewController("QuestCodeViewController") as QuestCodeViewController;
                code.ViewModel = viewModel;
                vc = code;
            }
            else if (viewModel.QuestionRequired)
            {
                var question = storyboard.InstantiateViewController("QuestAskQuestionViewController") as QuestAskQuestionViewController;
                question.ViewModel = viewModel;
                vc = question;
            }
            else if (viewModel.QuestComplete)
            {
                var completed = storyboard.InstantiateViewController("QuestCompletedViewController") as QuestCompletedViewController;
                completed.ViewModel = viewModel;
                vc = completed;
            }

            if (vc == null)
                return;

            NavigationController.PresentViewControllerAsync(vc, true);


        }

    }
}
