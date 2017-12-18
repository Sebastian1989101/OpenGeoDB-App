using MvvmCross.Forms.Platform;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using OpenGeoDB.Core.Model.Messages;

namespace OpenGeoDB.Core
{
    public class App : MvxFormsApplication
    {
        protected override void OnStart()
        {
            Mvx.Resolve<IMvxMessenger>().Publish(new AppStatus(this, AppStatus.StatusChange.Start));
            base.OnStart();
        }

        protected override void OnSleep()
        {
            Mvx.Resolve<IMvxMessenger>().Publish(new AppStatus(this, AppStatus.StatusChange.Sleep));
            base.OnSleep();
        }

        protected override void OnResume()
        {
            Mvx.Resolve<IMvxMessenger>().Publish(new AppStatus(this, AppStatus.StatusChange.Resume));
            base.OnResume();
        }
    }
}
