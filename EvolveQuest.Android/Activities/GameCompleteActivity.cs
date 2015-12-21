using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Content.PM;
using EstimoteSdk;
using EvolveQuest.Shared.ViewModels;
using Android.Content;

namespace EvolveQuest.Droid.Activities
{
    [Activity(Label = "Evolve Quest", ScreenOrientation = ScreenOrientation.Portrait, ParentActivity = typeof(QuestActivity))]
    public class GameCompleteActivity : Activity, BeaconManager.IServiceReadyCallback
    {
        BeaconManager beaconManager;
        Region secretRegion;

        ImageView mainImage;
        ProgressBar progressBar;
        const string BeaconId = "com.refractored";
        bool beaconsEnabled = true;
        ZXing.Mobile.MobileBarcodeScanner scanner;
        GameCompleteViewModel viewModel;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            App.CurrentActivity = this;
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);
            viewModel = new GameCompleteViewModel();
            SetContentView(Resource.Layout.game_complete);
            viewModel.PropertyChanged += HandlePropertyChanged;

            mainImage = FindViewById<ImageView>(Resource.Id.main_image);
            progressBar = FindViewById<ProgressBar>(Resource.Id.progressBar);
            beaconManager = new BeaconManager(this);
            scanner = new ZXing.Mobile.MobileBarcodeScanner();

            var shareButton = FindViewById<Button>(Resource.Id.share_success);
            shareButton.Click += (sender, e) =>
            {
                var intent = new Intent(Intent.ActionSend);
                intent.SetType("text/plain");
                intent.PutExtra(Intent.ExtraText, Resources.GetString(Resource.String.success_tweet));
                StartActivity(Intent.CreateChooser(intent, Resources.GetString(Resource.String.share_success)));
            };
      
            beaconManager.Ranging += (sender, e) =>
            {
                if (e.Beacons == null)
                    return;

                foreach (var beacon in e.Beacons)
                {
                    var proximity = Utils.ComputeProximity(beacon);

                    if (proximity != Utils.Proximity.Immediate)
                        continue;

                    var accuracy = Utils.ComputeAccuracy(beacon);
                    if (accuracy > .06)
                        continue;

                    viewModel.CheckBanana(beacon.Major, beacon.Minor);
                }
            };


            viewModel.LoadGameCommand.Execute(null);

        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();
            OverridePendingTransition(Resource.Animation.slide_in_right, Resource.Animation.slide_out_right);
        }

        private void CheckBluetooth()
        {
            //Validation checks
            if (!beaconManager.HasBluetooth ||
            !PackageManager.HasSystemFeature(PackageManager.FeatureBluetoothLe) ||
            !beaconManager.IsBluetoothEnabled ||
            !beaconManager.CheckPermissionsAndService())
            {
                beaconsEnabled = false;
            }
            else
            {
                beaconsEnabled = true;
            }
        }


        void HandlePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            RunOnUiThread(() =>
                {
                    switch (e.PropertyName)
                    {
          
                        case BaseViewModel.IsBusyPropertyName:
                            progressBar.Visibility = viewModel.IsBusy ? ViewStates.Visible : ViewStates.Invisible;
                            break;
                        case GameCompleteViewModel.PrizePropertyName:
                            {
                                if (viewModel.SecretBananaFound)
                                    mainImage.SetImageResource(Resource.Drawable.ic_secret_prize);

                                InitBeacon();
                            }
                            break;
                        case GameCompleteViewModel.SecretBananaFoundPropertyName:
                            if (viewModel.SecretBananaFound)
                                mainImage.SetImageResource(Resource.Drawable.ic_secret_prize);
                            break;
                    }

                });
        }


        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_home, menu);
            return base.OnCreateOptionsMenu(menu);
        }


        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_scan:
                    {

                        scanner.Scan().ContinueWith(result =>
                            {
                                if (result.Result == null)
                                    return;

                                Console.WriteLine("Scanned Barcode: " + result.Result.Text);
                                viewModel.CheckBanana(result.Result.Text);
                            });
                    }
                    break;
                case Android.Resource.Id.Home:
                    Finish();
                    OverridePendingTransition(Resource.Animation.slide_in_right, Resource.Animation.slide_out_right);
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }


        private void InitBeacon()
        {
            if (!beaconsEnabled)
                return;

            secretRegion = new Region(BeaconId, viewModel.UUID, new Java.Lang.Integer(9999), new Java.Lang.Integer(9999));
            beaconManager.Connect(this);
        }

        protected override void OnResume()
        {
            base.OnResume();
            App.CurrentActivity = this;
            CheckBluetooth();

            if (secretRegion != null && beaconsEnabled)
                beaconManager.Connect(this);
        }

        protected override void OnPause()
        {
            base.OnPause();
            if (secretRegion != null && beaconsEnabled)
                beaconManager.Disconnect();
        }

        public void OnServiceReady()
        { 
            beaconManager.StartRanging(secretRegion);
        }

        protected override void OnDestroy()
        {
            // Make sure we disconnect from the Estimote.
            if (beaconsEnabled)
                beaconManager.Disconnect();
            base.OnDestroy();
        }

    }
}