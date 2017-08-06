﻿using Mapsui;
using Mapsui.Utilities;
using Xamarin.Forms;
using Color = Mapsui.Styles.Color;
using Point = Mapsui.Geometries.Point;

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
            mapLayer.Attribution.Text = mapLayer.Attribution.Url = null;

            NativeMap.Layers.Add(mapLayer);
        }

        private static void OnFocusLocationChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            MapsuiMap mapsuiMap = bindable as MapsuiMap;
            if (mapsuiMap != null)
            {
                mapsuiMap.NativeMap.NavigateTo(new Point(mapsuiMap.FocusLongitude * 111318.30465862, mapsuiMap.FocusLatitude * 131396.69852677));
                mapsuiMap.NativeMap.NavigateTo(100);
            }
        }
    }
}