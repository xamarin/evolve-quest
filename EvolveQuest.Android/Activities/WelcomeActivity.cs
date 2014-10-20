using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Content.PM;
using EvolveQuest.Shared.ViewModels;
using EvolveQuest.Shared.Helpers;
using EvolveQuest.Shared.Interfaces;
using Android.Util;

namespace EvolveQuest.Droid.Activities
{
    [Activity(Label = "Evolve Quest",
        MainLauncher = true,
        ScreenOrientation = ScreenOrientation.Portrait,
        Theme = "@android:style/Theme.Holo.Light.NoActionBar")]
    public class WelcomeActivity : Activity
    {
        ProgressBar progressBar;
        Button buttonPlay;
        ImageButton buttonAbout;
        WelcomeViewModel viewModel;
        QuestMapView map;
        ImageView background;
        LockableScrollView scrollView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            App.CurrentActivity = this;
            SetContentView(Resource.Layout.welcome);
            // Create your application here
            viewModel = new WelcomeViewModel();
            viewModel.PropertyChanged += HandlePropertyChanged;

            progressBar = FindViewById<ProgressBar>(Resource.Id.progressBar);
            buttonPlay = FindViewById<Button>(Resource.Id.button_play);
            buttonAbout = FindViewById<ImageButton>(Resource.Id.button_about);
            background = FindViewById<ImageView>(Resource.Id.background);
            map = FindViewById<QuestMapView>(Resource.Id.map);

            scrollView = FindViewById<LockableScrollView>(Resource.Id.map_scroll);
            //scrollView.IsScrollable = true;
            scrollView.SmoothScrollingEnabled = true;

            map.SetPins(new []
                {
                    MapPinPosition.Center,
                    MapPinPosition.Right,
                    MapPinPosition.Left,
                    MapPinPosition.Center,
                    MapPinPosition.Left,
                    MapPinPosition.Center,
                    MapPinPosition.Right,
                    MapPinPosition.Left,
                    MapPinPosition.Right,
                    MapPinPosition.Center,
                    MapPinPosition.Left,
                    MapPinPosition.Center
                });

            buttonPlay.Click += (sender, args) =>
            {
                if (viewModel.GameLoaded && viewModel.Game != null && viewModel.Game.Started)
                {
                    GoToGame();
                    return;
                }
                viewModel.LoadGameCommand.Execute(null);
            };

            buttonAbout.Click += (sender, args) =>
            {
                StartActivity(new Intent(this, typeof(AboutActivity)));
                OverridePendingTransition(Resource.Animation.slide_in_left, Resource.Animation.slide_out_left);
            };
        }


        protected override void OnStart()
        {
            base.OnStart();
            SetMap();
        }


        private void GoToGame()
        {
            var intent = new Intent(this, typeof(QuestActivity));
            StartActivity(intent);
            OverridePendingTransition(Resource.Animation.slide_in_left, Resource.Animation.slide_out_left);
        }

        void HandlePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case BaseViewModel.IsBusyPropertyName:
                    {
                        buttonPlay.Enabled = buttonAbout.Enabled = !viewModel.IsBusy;
                        progressBar.Visibility = viewModel.IsBusy ? ViewStates.Visible : ViewStates.Invisible;
                    }
                    break;
                case WelcomeViewModel.GameLoadedPropertyName:
                    {
                        if (viewModel.GameLoaded)
                        {
                            if (viewModel.Game.Started)
                                GoToGame();
                            else
                                ServiceContainer.Resolve<IMessageDialog>().SendMessage("Evolve Quest", "Your quest begins on October 6th, 2014. Check back soon.");
                        }
                    }
                    break;
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_welcome, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        private void SetMap()
        {
            var quests = 11.0;
            var currentQuest = Settings.CurrentQuest;
            if (viewModel.Game != null)
                quests = (double)viewModel.Game.Quests.Count - 1;
            map.CurrentPin = currentQuest;

            if (quests <= 0.0)
                return;

            var percent = ((double)currentQuest / quests);

            if (percent > 1)
                percent = 1;

            if (percent <= 0.0)
                background.SetImageResource(Resource.Drawable.quest_map1);
            else if (percent < .2)
                background.SetImageResource(Resource.Drawable.quest_map2);
            else if (percent < .45)
                background.SetImageResource(Resource.Drawable.quest_map3);
            else if (percent < .6)
                background.SetImageResource(Resource.Drawable.quest_map4);
            else if (percent < 1)
                background.SetImageResource(Resource.Drawable.quest_map5);
            else
                background.SetImageResource(Resource.Drawable.quest_map6);
				
            scrollView.PostDelayed(() =>
                {
                    var y = (map.Height - DipToPixels(this, 448)) * percent;
                    scrollView.SmoothScrollTo(scrollView.ScrollX, (int)y);
                }, 1000);
        }

        public static float DipToPixels(Context context, float dipValue)
        {
            var metrics = context.Resources.DisplayMetrics;
            return TypedValue.ApplyDimension(ComplexUnitType.Dip, dipValue, metrics);
        }
   
    }
}