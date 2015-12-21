using System;

using Android.App;
using Android.Views;
using Android.Widget;
using Android.OS;
using EstimoteSdk;
using EvolveQuest.Shared.ViewModels;
using Android.Content.PM;
using EvolveQuest.Shared.Helpers;
using EvolveQuest.Shared.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using com.refractored.monodroidtoolkit.imageloader;

namespace EvolveQuest.Droid.Activities
{
    [Activity(Label = "Evolve Quest", ScreenOrientation = ScreenOrientation.Portrait, ParentActivity = typeof(WelcomeActivity))]
    public class QuestActivity : Activity, BeaconManager.IServiceReadyCallback
    {

        public static QuestViewModel ViewModel{ get; set; }

        BeaconManager beaconManager;
        EstimoteSdk.Region region, secretRegion;
        ImageView beacon1, beacon2, beacon3, mainImage;
        TextView mainText, beaconNearby;
        ProgressBar progressBar;
        const string BeaconId = "com.refractored";
        bool beaconsEnabled = false;
        Button buttonContinue;
        ImageLoader imageLoader;
	
        List<ImageView> beacons;
        private bool connected;
        ZXing.Mobile.MobileBarcodeScanner scanner;
        ZXing.Mobile.MobileBarcodeScanningOptions options = new ZXing.Mobile.MobileBarcodeScanningOptions();



        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);

            App.CurrentActivity = this;
            ViewModel = new QuestViewModel();
            SetContentView(Resource.Layout.quest);
            ViewModel.PropertyChanged += HandlePropertyChanged;
            imageLoader = new ImageLoader(this, 256, 5);
            beacon1 = FindViewById<ImageView>(Resource.Id.beacon1);
            beacon2 = FindViewById<ImageView>(Resource.Id.beacon2);
            beacon3 = FindViewById<ImageView>(Resource.Id.beacon3);
            beacons = new List<ImageView> { beacon1, beacon2, beacon3 };
            mainImage = FindViewById<ImageView>(Resource.Id.main_image);
            mainText = FindViewById<TextView>(Resource.Id.main_text);
            beaconNearby = FindViewById<TextView>(Resource.Id.text_beacons);
            progressBar = FindViewById<ProgressBar>(Resource.Id.progressBar);
            buttonContinue = FindViewById<Button>(Resource.Id.button_continue);
            buttonContinue.Click += ButtonContinueClick;
            buttonContinue.Visibility = ViewStates.Gone;
            beaconManager = new BeaconManager(this);
            scanner = new ZXing.Mobile.MobileBarcodeScanner();

            beaconManager.Ranging += BeaconManagerRanging;
    

            beaconManager.EnteredRegion += (sender, args) => SetBeaconText(true);

            beaconManager.ExitedRegion += (sender, args) => SetBeaconText(false);

      
            beacon1.Visibility = beacon2.Visibility = beacon3.Visibility = ViewStates.Invisible;
            options.PossibleFormats = new List<ZXing.BarcodeFormat>()
            { 
                ZXing.BarcodeFormat.QR_CODE
            };

            ViewModel.LoadQuestCommand.Execute(null);

        }


        #region Event Handlers

        void ButtonContinueClick(object sender, EventArgs e)
        {
            if (ViewModel.GameComplete)
            {
                GoToGameComplete();
                return;
            }

            Intent intent = null;
            if (ViewModel.CodeRequired)
                intent = new Intent(this, typeof(QuestCodeActivity));
            else if (ViewModel.QuestionRequired)
                intent = new Intent(this, typeof(QuestQuestionActivity));
            else if (ViewModel.QuestComplete)
                intent = new Intent(this, typeof(QuestCompleteActivity));

            if (intent == null)
                return;

            StartActivity(intent);
            OverridePendingTransition(Resource.Animation.slide_in_up, Resource.Animation.slide_out_up); 
        }

        #endregion

        #region Beacon Logic


        void BeaconManagerRanging(object sender, BeaconManager.RangingEventArgs e)
        {
            if (e.Beacons == null)
            {
                SetBeaconText(false);
                return;
            }
            bool close = false;
            foreach (var beacon in e.Beacons)
            {
                var proximity = Utils.ComputeProximity(beacon);

                if (proximity != Utils.Proximity.Unknown)
                    close = true;

                if (proximity != Utils.Proximity.Immediate)
                    continue;

                var accuracy = Utils.ComputeAccuracy(beacon);
                if (accuracy > .06)
                    continue;

                ViewModel.CheckBeacon(beacon.Major, beacon.Minor);

                if (e.Region.Major.ToString() != secretRegion.Major.ToString())
                    SetBeaconText(close);
            }
        }




        public void OnServiceReady()
        {
            if (region != null)
            {
                beaconManager.StopRanging(region);
                beaconManager.StopMonitoring(region);
                region = null;
            }

            if (secretRegion != null)
                beaconManager.StopRanging(secretRegion);
            else
                secretRegion = new EstimoteSdk.Region(BeaconId, ViewModel.UUID, new Java.Lang.Integer(9999), null);

            if (ViewModel.Quest.Major >= 0)
            {
                region = new EstimoteSdk.Region(BeaconId, ViewModel.UUID, new Java.Lang.Integer(ViewModel.Quest.Major), null);

                // This method is called when BeaconManager is up and running.
                beaconManager.StartRanging(region);
                beaconManager.StartMonitoring(region);
            }
            beaconManager.StartRanging(secretRegion);
            connected = true;
        }

        private void SetBeaconText(bool beaconsClose)
        {
            if (ViewModel.ExtraTaskVisible)
            {
                beaconNearby.SetText(Resource.String.all_found);
                beaconNearby.SetTextColor(EvolveQuest.Shared.Helpers.Color.Blue.ToAndroidColor());
                return;
            }

            if (beaconsClose)
            {
                beaconNearby.SetText(Resource.String.beacons_near);
                beaconNearby.SetTextColor(EvolveQuest.Shared.Helpers.Color.Blue.ToAndroidColor());
            }
            else
            {
                beaconNearby.SetText(Resource.String.no_beacons);
                beaconNearby.SetTextColor(EvolveQuest.Shared.Helpers.Color.LightGray.ToAndroidColor());
            }
        }

        private void CheckBluetooth()
        {
            beaconNearby.Visibility = ViewStates.Gone;
            //Validation checks
            if (!beaconManager.HasBluetooth || !PackageManager.HasSystemFeature(PackageManager.FeatureBluetoothLe))
            {
                beaconsEnabled = false;
            }
            else if (!beaconManager.IsBluetoothEnabled)
            {
                //bluetooth is not enabled
                ServiceContainer.Resolve<IMessageDialog>().SendMessage("Bluetooth Turned Off.",
                    "Please enable Bluetooth for Beacon support or you can enjoy Evolve Quest by scanning QR codes.");
                beaconsEnabled = false;
            }
            else if (!beaconManager.CheckPermissionsAndService())
            {
                ServiceContainer.Resolve<IMessageDialog>().SendMessage("Permission issue.",
                    "Hmm. There seems to be an issue with Bluetooth permissions. Please check with Evolve staff or play by scanning QR codes.");
                beaconsEnabled = false;
            }
            else
            {
                beaconsEnabled = true;
                beaconNearby.Visibility = ViewStates.Visible;
            }
        }

        #endregion

        #region Game State

        void InitializeBeacons()
        {
            RunOnUiThread(() =>
                {
                    beacon1.SetImageResource(Resource.Drawable.ic_no_banana);
                    beacon2.SetImageResource(Resource.Drawable.ic_no_banana);
                    beacon3.SetImageResource(Resource.Drawable.ic_no_banana);
                    beacon1.Visibility = beacon2.Visibility = beacon3.Visibility = ViewStates.Invisible; 
                    for (int i = 0; i < ViewModel.Quest.Beacons.Count; i++)
                    {
                        beacons[i].Visibility = ViewStates.Visible;
                    }

                    UpdateBeacons();
                    mainText.Text = ViewModel.Quest.Clue.Message;

                });
					

            if (connected)
                OnServiceReady();
      
            if (beaconsEnabled)
                beaconManager.Connect(this);

            mainImage.SetImageResource(Resource.Drawable.xamarin_icon);
            imageLoader.DisplayImage(ViewModel.Quest.Clue.Image, mainImage, Resource.Drawable.xamarin_icon);
        }

	

        void UpdateBeacons()
        {
            RunOnUiThread(() =>
                {
                    for (int i = 0; i < ViewModel.Quest.Beacons.Count; i++)
                    {
                        var banana = ViewModel.Quest.Beacons.FirstOrDefault(b => b.Id == i);
                        if (banana == null)
                            continue;

                        beacons[i].SetImageResource(banana.Found ? Resource.Drawable.ic_banana : Resource.Drawable.ic_no_banana);
                    } 
                    buttonContinue.Visibility = ViewModel.ExtraTaskVisible ? ViewStates.Visible : ViewStates.Gone;
                });
        }

   

        void HandlePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            RunOnUiThread(() =>
                {
                    switch (e.PropertyName)
                    {
                        case QuestViewModel.ExtraTaskVisiblePropertyName:
                            buttonContinue.Visibility = ViewModel.ExtraTaskVisible ? ViewStates.Visible : ViewStates.Gone;
                            SetBeaconText(false);
                            break;
                        case BaseViewModel.IsBusyPropertyName:
                            progressBar.Visibility = ViewModel.IsBusy ? ViewStates.Visible : ViewStates.Invisible;
                            break;
                        case QuestViewModel.QuestPropertyName:
                        case QuestViewModel.NewQuestPropertyName:
                            InitializeBeacons();
                            break;
                        case QuestViewModel.QuestsPropertyName:
                            UpdateBeacons();
                            break;
                        case QuestViewModel.CompletionDisplayPropertyName:
                            ActionBar.Title = ViewModel.CompletionDisplay;
                            break;
                        case QuestViewModel.GameCompletePropertyName:
            //GoToGameComplete();
                            break;
                    }

                });
        }

        private void GoToGameComplete()
        {
            var intent = new Intent(this, typeof(GameCompleteActivity));
            StartActivity(intent);
            OverridePendingTransition(Resource.Animation.slide_in_left, Resource.Animation.slide_out_left);      
        }

        #endregion


        #region Options

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
                        if (ViewModel.IsBusy)
                            break;
            
                        scanner.Scan(options).ContinueWith((result) =>
                            {
                                if (result.Result == null)
                                    return;

                                System.Console.WriteLine("Scanned Barcode: " + result.Result.Text);
                                ViewModel.CheckBeacon(result.Result.Text);
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

        #endregion


        #region Life Cycle Events

        public override void OnBackPressed()
        {
            base.OnBackPressed();
            OverridePendingTransition(Resource.Animation.slide_in_right, Resource.Animation.slide_out_right);
        }

        protected override void OnStart()
        {
            base.OnStart();
            if (ViewModel.QuestComplete && Settings.QuestDone)
                ViewModel.LoadNextLevel();
        }


        protected override void OnResume()
        {
            base.OnResume();
            App.CurrentActivity = this;
            CheckBluetooth();

            if (region != null && beaconsEnabled)
                beaconManager.Connect(this);
        }

        protected override void OnPause()
        {
            base.OnPause();
            if (region != null && beaconsEnabled)
                beaconManager.Disconnect();

            connected = false;
        }

        protected override void OnDestroy()
        {
            // Make sure we disconnect from the Estimote.
            if (beaconsEnabled)
                beaconManager.Disconnect();


            connected = false;
            base.OnDestroy();
        }

        #endregion



 

    }
}


