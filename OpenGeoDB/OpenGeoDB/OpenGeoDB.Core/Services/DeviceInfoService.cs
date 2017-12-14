using System;
using Xamarin.Forms;

namespace OpenGeoDB.Core.Services
{
    public interface IDeviceInfoService
    {
        float Width { get; }
        float Height { get; }

        event EventHandler DeviceMarginsChanged;

        Thickness GetDeviceMargins();
        void SetDeviceValues(Thickness deviceMargins, float width, float height);
    }

    public class DeviceInfoService : IDeviceInfoService
    {
        private Thickness _deviceMargins = new Thickness(0);
        private float _width, _height;

        public float Width => _width;
        public float Height => _height;

        public event EventHandler DeviceMarginsChanged;

        public Thickness GetDeviceMargins()
        {
            return _deviceMargins;
        }

        public void SetDeviceValues(Thickness deviceMargins, float width, float height)
        {
            _width = width;
            _height = height;

            _deviceMargins = deviceMargins;
            DeviceMarginsChanged?.Invoke(this, null);
        }
    }
}
