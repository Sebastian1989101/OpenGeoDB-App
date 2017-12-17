using Android.App;
using Android.Content.PM;
using MvvmCross.Droid.Views;
using Xamarin.Forms;


namespace OpenGeoDB.Droid
{
    [Activity(
        Label = "PLZ Suche",
        Icon = "@drawable/icon",
        Theme = "@style/AppTheme.Splash",
        MainLauncher = true,
        NoHistory = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class SplashScreen : MvxSplashScreenActivity
    {
        public SplashScreen()
            : base(Resource.Layout.SplashScreen)
        { }

        protected override void TriggerFirstNavigate()
        {
            StartActivity(typeof(FormsApplicationActivity));
            base.TriggerFirstNavigate();
        }

        protected override void OnCreate(Android.OS.Bundle bundle)
        {
            Forms.Init(this, bundle);
            Forms.ViewInitialized += (sender, e) =>
                {
                    if (!string.IsNullOrWhiteSpace(e.View.StyleId))
                        e.NativeView.ContentDescription = e.View.StyleId;
                };

            base.OnCreate(bundle);
        }
    }
}
