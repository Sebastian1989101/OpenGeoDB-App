using System.Diagnostics;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using OpenGeoDB.Core.ViewModels;
using Xamarin.Forms;

namespace OpenGeoDB.Core
{
    public class AppStart : IMvxAppStart
    {
        private readonly IMvxNavigationService _navigationService;

        public AppStart(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public void Start(object hint = null)
        {
            try
            {
                if (Device.RuntimePlatform == Device.WinPhone || Device.RuntimePlatform == Device.WinRT || Device.RuntimePlatform == Device.UWP)
                {
                    _navigationService.Navigate<MainViewModel>();
                    return;
                }

                _navigationService.Navigate<MainViewModel>().GetAwaiter().GetResult();
            }
            catch (System.Exception e)
            {
                Debug.WriteLine(e);
            }
        }
    }
}
