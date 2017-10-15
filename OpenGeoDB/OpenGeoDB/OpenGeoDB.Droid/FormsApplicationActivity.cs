using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.OS;
using MvvmCross.Forms.Droid.Views;
using Xamarin.Forms;

namespace OpenGeoDB.Droid
{
    //todo: Fix rotation (navigate back on rotate)
    [Activity(Label = "FormsApplicationActivity", Icon = "@drawable/Icon", ScreenOrientation = ScreenOrientation.Portrait)]
    public class FormsApplicationActivity : MvxFormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Forms.Init(this, bundle);
            UserDialogs.Init(this);
        }
    }
}