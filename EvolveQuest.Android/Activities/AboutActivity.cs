using Android.App;
using Android.OS;
using Android.Views;
using Android.Preferences;

namespace EvolveQuest.Droid.Activities
{
    [Activity(Label = "About", Icon = "@drawable/ic_launcher", Theme = "@style/Theme.Quest")]
    public class AboutActivity : PreferenceActivity
    {


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            AddPreferencesFromResource(Resource.Xml.preferences_general);

            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetDisplayShowHomeEnabled(true);
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();
            OverridePendingTransition(Resource.Animation.slide_in_right, Resource.Animation.slide_out_right);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    Finish();
                    OverridePendingTransition(Resource.Animation.slide_in_right, Resource.Animation.slide_out_right);
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }


    }
}