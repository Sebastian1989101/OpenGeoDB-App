using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using Mapsui;
using Mapsui.Layers;
using Mapsui.Projection;
using Mapsui.Providers;
using Mapsui.Styles;
using Mapsui.Utilities;
using Mapsui.Widgets;
using MvvmCross.Platform;
using OpenGeoDB.Core.Model.Data;
using OpenGeoDB.Core.Resources;
using OpenGeoDB.Core.Services;
using Xamarin.Forms;
using Color = Mapsui.Styles.Color;

namespace OpenGeoDB.Core.Controls
{
    public class MapsuiMap : View
    {
        private const string MARKER_RESOURCE = "OpenGeoDB.Core.Resources.Marker.png";
        private const string HIGHLIGHT_MARKER_RESOURCE = "OpenGeoDB.Core.Resources.Marker.Highlight.png";

        private static IAppSettings _appSettings;

        public static readonly BindableProperty FocusLatitudeProperty = BindableProperty.Create(
            nameof(FocusLatitude), typeof(double), typeof(MapsuiMap), 0d, BindingMode.OneWay, null, OnFocusLocationChanged);

        public static readonly BindableProperty FocusLongitudeProperty = BindableProperty.Create(
            nameof(FocusLongitude), typeof(double), typeof(MapsuiMap), 0d, BindingMode.OneWay, null, OnFocusLocationChanged);

        public static readonly BindableProperty MapMarkerProperty = BindableProperty.Create(
            nameof(MapMarker), typeof(IEnumerable<Location>), typeof(MapsuiMap), null, BindingMode.OneWay, null, OnMapMarkerChanged);

        public static readonly BindableProperty ChooseLocationCommandProperty = BindableProperty.Create(
            nameof(ChooseLocationCommand), typeof(ICommand), typeof(MapsuiMap));

        public double FocusLatitude
        {
            get => (double) GetValue(FocusLatitudeProperty);
            set => SetValue(FocusLatitudeProperty, value);
        }

        public double FocusLongitude
        {
            get => (double) GetValue(FocusLongitudeProperty);
            set => SetValue(FocusLongitudeProperty, value);
        }

        public IEnumerable<Location> MapMarker
        {
            get => (IEnumerable<Location>) GetValue(MapMarkerProperty);
            set => SetValue(MapMarkerProperty, value);
        }

        public ICommand ChooseLocationCommand
        {
            get => (ICommand)GetValue(ChooseLocationCommandProperty);
            set => SetValue(ChooseLocationCommandProperty, value);
        }

        public Map NativeMap { get; }

        public MapsuiMap()
        {
            _appSettings = Mvx.Resolve<IAppSettings>();
            NativeMap = new Map();

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    NativeMap.BackColor = Color.White;
                    break;
                case Device.Android:
                    NativeMap.BackColor = Color.FromArgb(0xFF, 0x11, 0x11, 0x11);
                    break;
                case Device.UWP:
                    NativeMap.BackColor = Color.Black;
                    break;
            }

            var mapLayer = OpenStreetMap.CreateTileLayer();
            mapLayer.Attribution = null;

            NativeMap.Layers.Add(mapLayer);
            NativeMap.Info += (sender, e) => 
	            {
                    if (e.Feature?["Location"] != null && ChooseLocationCommand != null)
	                {
	                    if (e.Feature?["Location"] is Location location && ChooseLocationCommand.CanExecute(location))
                            ChooseLocationCommand.Execute(location);
	                }
	            };

            var deviceInfoService = Mvx.Resolve<IDeviceInfoService>();
            NativeMap.Widgets.Add(new Hyperlink
                {
                    Text = "© OpenStreetMap contributors",
                    Url = "http://www.openstreetmap.org/copyright",
                    VerticalAlignment = VerticalAlignment.Bottom,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    CornerRadius = 4,
                    MarginX = 0,
                    MarginY = (float)Math.Max(deviceInfoService.GetDeviceMargins().Bottom, 5f),
                    PaddingX = 32,
                    PaddingY = 8,
                    BackColor = new Color(255, 255, 255, 196)
                });
        }

        private static void OnFocusLocationChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is MapsuiMap mapsuiMap)
            {
                mapsuiMap.NativeMap.NavigateTo(SphericalMercator.FromLonLat(mapsuiMap.FocusLongitude, mapsuiMap.FocusLatitude));
                mapsuiMap.NativeMap.NavigateTo(100);

				Features features = GetIconFeatures(mapsuiMap.FocusLatitude, mapsuiMap.FocusLongitude);
				mapsuiMap.AddFeatures(features, "HighlightMarkerLayer", HIGHLIGHT_MARKER_RESOURCE);

                mapsuiMap.NativeMap.ViewChanged(true);
            }
		}

		private static void OnMapMarkerChanged(BindableObject bindable, object oldValue, object newValue)
		{
		    if (bindable is MapsuiMap mapsuiMap)
            {
                Features features = GetIconFeatures(mapsuiMap.MapMarker);
                mapsuiMap.AddFeatures(features, "MarkerLayer", MARKER_RESOURCE);

                const string LABEL_LAYERNAME = "LABEL";
                ILayer matchingLayer = mapsuiMap.NativeMap.Layers.FirstOrDefault(l => l.Name == LABEL_LAYERNAME);
				if (matchingLayer != null)
					mapsuiMap.NativeMap.Layers.Remove(matchingLayer);
                
                Layer labelLayer = new Layer(LABEL_LAYERNAME)
	                {
	                    DataSource = new MemoryProvider(GetLabelFeatures(mapsuiMap.MapMarker)),
	                    Style = null
	                };

				mapsuiMap.NativeMap.Layers.Add(labelLayer);
				mapsuiMap.NativeMap.ViewChanged(true);
            }
		}

        private void AddFeatures(Features features, string layername, string symbolStyle)
        {
			if (features.Any())
			{
				ILayer matchingLayer = NativeMap.Layers.FirstOrDefault(l => l.Name == layername);
				if (matchingLayer != null)
					NativeMap.Layers.Remove(matchingLayer);

				Layer layer = new Layer(layername)
					{
						Name = layername,
						DataSource = new MemoryProvider(features),
						Style = GetSymbolStyle(symbolStyle)
					};

				NativeMap.Layers.Add(layer);
				NativeMap.InfoLayers.Add(layer);
			}
        }

		private static Features GetIconFeatures(double latitude, double longitude)
		{
			Features features = new Features();
            Feature feature = new Feature 
                { 
                    Geometry = SphericalMercator.FromLonLat(longitude, latitude)
                };

			features.Add(feature);
			return features;
		}

        private static Features GetIconFeatures(IEnumerable<Location> entries)
        {
            Features features = new Features();
            foreach (Location entry in entries)
            {
                Feature feature = new Feature 
                    { 
                        Geometry = SphericalMercator.FromLonLat(entry.Longitude, entry.Latitude), 
                        ["Location"] = entry 
                    };

                features.Add(feature);
            }

            return features;
        }

        private static SymbolStyle GetSymbolStyle(string icon)
		{
            Assembly assembly = typeof(CoreApp).GetTypeInfo().Assembly;
			Stream image = assembly.GetManifestResourceStream(icon);
			int bitmapId = BitmapRegistry.Instance.Register(image);

			return new SymbolStyle { BitmapId = bitmapId, SymbolScale = 0.2, SymbolOffset = new Offset(0, 72) };
		}

		private static Features GetLabelFeatures(IEnumerable<Location> entries)
		{
			var features = new Features();
			foreach (Location entry in entries)
			{
                string distanceText = entry.Distance.ToString();
                switch (entry.DistanceType)
                {
                    case DistanceType.Kilometers:
                        distanceText = string.Format(AppResources.DistanceType_Kilometers_Text, entry.Distance);
                        break;
                    case DistanceType.NauticalMiles:
                        distanceText = string.Format(AppResources.DistanceType_NauticalMiles_Text, entry.Distance);
                        break;
                    case DistanceType.Miles:
                        distanceText = string.Format(AppResources.DistanceType_Miles_Text, entry.Distance);
                        break;
                }

				Feature feature = new Feature { Geometry = SphericalMercator.FromLonLat(entry.Longitude, entry.Latitude) };
                if (_appSettings.ShowZipCodeAboveNearbyMarker)
                {
                    LabelStyle labelStyle = new LabelStyle
                        {
                            Text = $"{entry.ZipCode} {entry.Village}",
                            Font = { Size = 11 },
                            ForeColor = Color.FromArgb(255, 0, 0, 0),
                            BackColor = new Brush(Color.FromArgb(196, 255, 255, 255)),
                            HorizontalAlignment = LabelStyle.HorizontalAlignmentEnum.Center,
                            Offset = new Offset(0, -64)
                        };

                    labelStyle.Halo = new Pen(labelStyle.ForeColor, 0.2);
                    feature.Styles.Add(labelStyle);
                }

                feature.Styles.Add(new LabelStyle
                    {
                        Text = distanceText,
                        Font = { Size = 10 },
                        ForeColor = Color.FromArgb(255, 0, 0, 0),
                        BackColor = new Brush(Color.FromArgb(196, 255, 255, 255)),
                        HorizontalAlignment = LabelStyle.HorizontalAlignmentEnum.Center, 
                        Offset = new Offset(0, -46)
                    });

				features.Add(feature);
			}

			return features;
		}
    }
}