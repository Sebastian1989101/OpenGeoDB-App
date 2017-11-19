using MvvmCross.Platform;
using OpenGeoDB.Core.Services;
using Xamarin.Forms;

namespace OpenGeoDB.Core.Controls
{
    public abstract class SafeAreaViewCell : ViewCell
    {
        protected abstract Layout Layout { get; }

        protected SafeAreaViewCell()
        {
            var deviceInfoService = Mvx.Resolve<IDeviceInfoService>();
            deviceInfoService.DeviceMarginsChanged += delegate { UpdateCellPaddings(deviceInfoService); };

            Appearing += delegate { UpdateCellPaddings(deviceInfoService); };
        }

        private void UpdateCellPaddings(IDeviceInfoService deviceInfoService)
        {
            if (Layout == null)
                return;

            var thickness = new Thickness(0);
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    thickness = new Thickness(15, 0);
                    break;

                case Device.Android:
                    thickness = new Thickness(17, 0, 5, 0);
                    break;

                case Device.WinPhone:
                case Device.WinRT:
                case Device.UWP:
                    thickness = new Thickness(0, 0, 5, 0);
                    break;
            }

            var deviceMargins = deviceInfoService.GetDeviceMargins();
            thickness.Left += deviceMargins.Left;
            thickness.Right += deviceMargins.Right;

            Layout.Padding = thickness;
        }
    }
}