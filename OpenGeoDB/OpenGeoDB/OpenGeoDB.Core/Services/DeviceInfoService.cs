using System;
using Xamarin.Forms;

namespace OpenGeoDB.Core.Services
{
    public interface IDeviceInfoService
    {
        event EventHandler DeviceMarginsChanged;

        Thickness GetDeviceMargins();
        void SetDeviceMargins(Thickness deviceMargins);
    }

    public class DeviceInfoService : IDeviceInfoService
    {
        private Thickness _deviceMargins = new Thickness(0);

        public event EventHandler DeviceMarginsChanged;

        public Thickness GetDeviceMargins()
        {
            return _deviceMargins;
        }

        public void SetDeviceMargins(Thickness deviceMargins)
        {
            _deviceMargins = deviceMargins;
            DeviceMarginsChanged?.Invoke(this, null);
        }
    }
}
