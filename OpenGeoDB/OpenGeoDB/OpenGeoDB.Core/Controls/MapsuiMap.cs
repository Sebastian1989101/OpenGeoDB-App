using Mapsui;
using Mapsui.Projection;
using Mapsui.Utilities;
using Xamarin.Forms;
using Color = Mapsui.Styles.Color;

namespace OpenGeoDB.Core.Controls
{
    public class MapsuiMap : View
    {
        public static readonly BindableProperty FocusLatitudeProperty = BindableProperty.Create(
            nameof(FocusLatitude), typeof(double), typeof(MapsuiMap), 0d, BindingMode.OneWay, null, OnFocusLocationChanged);

        public static readonly BindableProperty FocusLongitudeProperty = BindableProperty.Create(
            nameof(FocusLongitude), typeof(double), typeof(MapsuiMap), 0d, BindingMode.OneWay, null, OnFocusLocationChanged);

        public double FocusLatitude
        {
            get { return (double) GetValue(FocusLatitudeProperty); }
            set { SetValue(FocusLatitudeProperty, value); }
        }

        public double FocusLongitude
        {
            get { return (double) GetValue(FocusLongitudeProperty); }
            set { SetValue(FocusLongitudeProperty, value); }
        }

        public Map NativeMap { get; }

        public MapsuiMap()
        {
            NativeMap = new Map();

            if (Device.RuntimePlatform == Device.iOS)
                NativeMap.BackColor = Color.White;

            var mapLayer = OpenStreetMap.CreateTileLayer();
            NativeMap.Layers.Add(mapLayer);
        }

        private static void OnFocusLocationChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            MapsuiMap mapsuiMap = bindable as MapsuiMap;
            if (mapsuiMap != null)
            {
                mapsuiMap.NativeMap.NavigateTo(SphericalMercator.FromLonLat(mapsuiMap.FocusLongitude, mapsuiMap.FocusLatitude));
                mapsuiMap.NativeMap.NavigateTo(100);
            }
        }
    }
}