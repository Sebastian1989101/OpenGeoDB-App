using System.Diagnostics;
using System.Linq;
using Foundation;
using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Platform;
using MvvmCross.Platform;
using OpenGeoDB.Core.Pages;
using OpenGeoDB.Core.Services;
using UIKit;
using Xamarin.Forms;

namespace OpenGeoDB.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : MvxApplicationDelegate
    {
        // class-level declarations
        private bool _finishedLaunching;

        public override UIWindow Window
        {
            get;
            set;
        }

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Window = new UIWindow(UIScreen.MainScreen.Bounds);

            var setup = new Setup(this, Window);
            setup.Initialize();

            var startup = Mvx.Resolve<IMvxAppStart>();
            startup.Start();

            Window.MakeKeyAndVisible();

            NSNotificationCenter.DefaultCenter.AddObserver(UIApplication.DidChangeStatusBarOrientationNotification, n =>
                {
                    UpdateDeviceMargins();
                });

            NSNotificationCenter.DefaultCenter.AddObserver(UIApplication.DidChangeStatusBarFrameNotification, n =>
                {
                    UpdateDeviceMargins();
                });

            UpdateDeviceMargins();
            _finishedLaunching = true;

            return true;
        }

        public override void OnResignActivation(UIApplication application)
        {
            // Invoked when the application is about to move from active to inactive state.
            // This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) 
            // or when the user quits the application and it begins the transition to the background state.
            // Games should use this method to pause the game.
        }

        public override void DidEnterBackground(UIApplication application)
        {
            // Use this method to release shared resources, save user data, invalidate timers and store the application state.
            // If your application supports background exection this method is called instead of WillTerminate when the user quits.
        }

        public override void WillEnterForeground(UIApplication application)
        {
            // Called as part of the transiton from background to active state.
            // Here you can undo many of the changes made on entering the background.
        }

        public override void OnActivated(UIApplication application)
        {
            // Restart any tasks that were paused (or not yet started) while the application was inactive. 
            // If the application was previously in the background, optionally refresh the user interface.
        }

        public override void WillTerminate(UIApplication application)
        {
            // Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
        }

        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations(UIApplication application, UIWindow forWindow)
        {
            if (Xamarin.Forms.Application.Current?.MainPage != null)
            {
                if (HasSafeArea() && CurrentPageIsOfType<SettingsPage>())
                {
                    if (UIApplication.SharedApplication.StatusBarOrientation != UIInterfaceOrientation.Portrait)
                        UIDevice.CurrentDevice.SetValueForKey(NSNumber.FromInt32((int)UIInterfaceOrientationMask.Portrait), (NSString)"orientation");

                    return UIInterfaceOrientationMask.Portrait;
                }
            }

            if (Device.Idiom == TargetIdiom.Phone)
                return UIInterfaceOrientationMask.AllButUpsideDown;

            return UIInterfaceOrientationMask.All;
        }

        private bool CurrentPageIsOfType<TPageType>()
        {
            var mainPage = Xamarin.Forms.Application.Current.MainPage;
            return mainPage is TPageType || (mainPage as NavigationPage)?.CurrentPage is TPageType || mainPage.Navigation?.ModalStack.LastOrDefault() is TPageType;
        }

        private bool HasSafeArea()
        {
            if (!_finishedLaunching)
                return false;

            var safeArea = UIApplication.SharedApplication.KeyWindow.SafeAreaInsets;
            return safeArea.Top > 0 || safeArea.Left > 0 || safeArea.Top > 0 || safeArea.Bottom > 0;
        }

        private void UpdateDeviceMargins()
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
            {
                var deviceInfo = Mvx.Resolve<IDeviceInfoService>();
                var safeArea = UIApplication.SharedApplication.KeyWindow.SafeAreaInsets;

                Debug.WriteLine($"Device Orientation: {UIApplication.SharedApplication.StatusBarOrientation}");
                Debug.WriteLine($"Device margin: Bottom ({safeArea.Bottom})");
                Debug.WriteLine($"Device margin: Left ({safeArea.Left})");
                Debug.WriteLine($"Device margin: Top ({safeArea.Top})");
                Debug.WriteLine($"Device margin: Right ({safeArea.Right})");

                deviceInfo.SetDeviceMargins(new Thickness(safeArea.Left, safeArea.Top, safeArea.Right, safeArea.Bottom));
            }
        }
    }
}


