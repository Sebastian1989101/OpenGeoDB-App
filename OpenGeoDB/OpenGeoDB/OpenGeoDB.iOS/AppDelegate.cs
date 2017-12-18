using System;
using System.Diagnostics;
using System.Linq;
using Foundation;
using MTiRate;
using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Platform;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using OpenGeoDB.Core.Model.Messages;
using OpenGeoDB.Core.Services;
using StoreKit;
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

            iRate.SharedInstance.DaysUntilPrompt = 3;
            iRate.SharedInstance.UsesUntilPrompt = 5;
            iRate.SharedInstance.PromptForNewVersionIfUserRated = true;

            iRate.SharedInstance.ShouldPromptForRating = (arg1, arg2) =>
                {
                    if (UIDevice.CurrentDevice.CheckSystemVersion(10, 3))
                    {
                        SKStoreReviewController.RequestReview();
                        return false;
                    }

                    return true;
                };

            Window.MakeKeyAndVisible();
            SetSettings();

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

        public override void DidEnterBackground(UIApplication application)
        {
            Mvx.Resolve<IMvxMessenger>().Publish(new AppStatus(this, AppStatus.StatusChange.EnterBackground));
        }

        public override void WillEnterForeground(UIApplication application)
        {
            Mvx.Resolve<IMvxMessenger>().Publish(new AppStatus(this, AppStatus.StatusChange.EnterForeground));
        }

        private static void SetSettings()
        {
            var bundleShortVersionString = NSBundle.MainBundle.InfoDictionary["CFBundleShortVersionString"].ToString();
            var bundleVersion = NSBundle.MainBundle.InfoDictionary["CFBundleVersion"].ToString();
            var usesCount = NSUserDefaults.StandardUserDefaults.ValueForKey(new NSString("UsesCount"));

            DateTime currentTimeStamp = DateTime.Now;

            int count = 0;
            int.TryParse(usesCount?.ToString(), out count);

            NSUserDefaults.StandardUserDefaults.SetString($"{bundleShortVersionString} ({bundleVersion})", "AppVersion");
            NSUserDefaults.StandardUserDefaults.SetString($"{currentTimeStamp.ToShortDateString()} {currentTimeStamp.ToShortTimeString()}", "LastUsed");
            NSUserDefaults.StandardUserDefaults.SetString($"{++count}", "UsesCount");

            NSUserDefaults.StandardUserDefaults.Synchronize();
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

                var width = UIScreen.MainScreen.Bounds.Width;
                var height = UIScreen.MainScreen.Bounds.Height;

                var deviceOrientation = UIApplication.SharedApplication.StatusBarOrientation;
                if ((!UIApplication.SharedApplication.StatusBarHidden || deviceOrientation == UIInterfaceOrientation.Portrait) && safeArea.Top == 0)
                {
                    nfloat statusBarHeight = UIApplication.SharedApplication.StatusBarFrame.Height;
                    Debug.WriteLine($"Change top to status bar height: {statusBarHeight}");

                    safeArea.Top = statusBarHeight;
                }

                Debug.WriteLine($"Device Orientation: {deviceOrientation}");
                Debug.WriteLine($"Device margin: Bottom ({safeArea.Bottom})");
                Debug.WriteLine($"Device margin: Left ({safeArea.Left})");
                Debug.WriteLine($"Device margin: Top ({safeArea.Top})");
                Debug.WriteLine($"Device margin: Right ({safeArea.Right})");

                deviceInfo.SetDeviceValues(new Thickness(safeArea.Left, safeArea.Top, safeArea.Right, safeArea.Bottom), (float)width, (float)height);
            }
        }
    }
}


