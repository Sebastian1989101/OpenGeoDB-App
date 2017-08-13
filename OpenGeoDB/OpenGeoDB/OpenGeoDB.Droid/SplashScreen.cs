using Android.App;
using Android.Content.PM;
using MvvmCross.Droid.Views;
using Xamarin.Forms;


namespace OpenGeoDB.Droid
{
    [Activity(Label = "PLZ Suche", MainLauncher = true, Icon = "@drawable/Icon", Theme = "@style/Theme.Splash", NoHistory = true, ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashScreen : MvxSplashScreenActivity
    {
        private bool _isInitializationComplete;

        public SplashScreen()
            : base(Resource.Layout.SplashScreen)
        { }

        public override void InitializationComplete()
        {
            if (!_isInitializationComplete)
            {
                _isInitializationComplete = true;
                StartActivity(typeof(FormsApplicationActivity));
            }
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
