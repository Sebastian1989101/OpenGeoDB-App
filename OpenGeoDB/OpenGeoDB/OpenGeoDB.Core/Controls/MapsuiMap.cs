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
using OpenGeoDB.Core.Model.Data;
using OpenGeoDB.Core.Resources;
using Xamarin.Forms;
using Color = Mapsui.Styles.Color;

namespace OpenGeoDB.Core.Controls
{
    public class MapsuiMap : View
    {
        private const string MARKER_RESOURCE = "OpenGeoDB.Core.Resources.Marker.png";
        private const string HIGHLIGHT_MARKER_RESOURCE = "OpenGeoDB.Core.Resources.Marker.Highlight.png";

        public static readonly BindableProperty FocusLatitudeProperty = BindableProperty.Create(
            nameof(FocusLatitude), typeof(double), typeof(MapsuiMap), 0d, BindingMode.OneWay, null, OnFocusLocationChanged);

        public static readonly BindableProperty FocusLongitudeProperty = BindableProperty.Create(
            nameof(FocusLongitude), typeof(double), typeof(MapsuiMap), 0d, BindingMode.OneWay, null, OnFocusLocationChanged);

        public static readonly BindableProperty MapMarkerProperty = BindableProperty.Create(
            nameof(MapMarker), typeof(IEnumerable<Location>), typeof(MapsuiMap), null, BindingMode.OneWay, null, OnMapMarkerChanged);

        public static readonly BindableProperty ChooseLocationCommandProperty = BindableProperty.Create(
            nameof(ChooseLocationCommand), typeof(ICommand), typeof(MapsuiMap), null, BindingMode.OneWay);

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

        public IEnumerable<Location> MapMarker
        {
            get { return (IEnumerable<Location>) GetValue(MapMarkerProperty); }
            set { SetValue(MapMarkerProperty, value); }
        }

        public ICommand ChooseLocationCommand
        {
            get { return (ICommand)GetValue(ChooseLocationCommandProperty); }
            set { SetValue(ChooseLocationCommandProperty, value); }
        }

        public Map NativeMap { get; }

        public MapsuiMap()
        {
            NativeMap = new Map();

            if (Device.RuntimePlatform == Device.iOS)
                NativeMap.BackColor = Color.White;

            var mapLayer = OpenStreetMap.CreateTileLayer();
            NativeMap.Layers.Add(mapLayer);

            NativeMap.Info += (sender, e) => 
	            {
                    if (e.Feature?["Location"] != null && ChooseLocationCommand != null)
	                {
                        Location location = e.Feature?["Location"] as Location;
                        if (location != null && ChooseLocationCommand.CanExecute(location))
                            ChooseLocationCommand.Execute(location);
	                }
	            };
        }

        private static void OnFocusLocationChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            MapsuiMap mapsuiMap = bindable as MapsuiMap;
            if (mapsuiMap != null)
            {
                mapsuiMap.NativeMap.NavigateTo(SphericalMercator.FromLonLat(mapsuiMap.FocusLongitude, mapsuiMap.FocusLatitude));
                mapsuiMap.NativeMap.NavigateTo(100);

				Features features = GetIconFeatures(mapsuiMap.FocusLatitude, mapsuiMap.FocusLongitude);
				mapsuiMap.AddFeatures(features, "HighlightMarkerLayer", HIGHLIGHT_MARKER_RESOURCE);
            }
		}

		private static void OnMapMarkerChanged(BindableObject bindable, object oldValue, object newValue)
		{
			MapsuiMap mapsuiMap = bindable as MapsuiMap;
			if (mapsuiMap != null)
            {
                Features features = GetIconFeatures(mapsuiMap.MapMarker);
                mapsuiMap.AddFeatures(features, "MarkerLayer", MARKER_RESOURCE);

                Layer labelLayer = new Layer("LABEL")
	                {
	                    DataSource = new MemoryProvider(GetLabelFeatures(mapsuiMap.MapMarker)),
	                    Style = null
	                };

                mapsuiMap.NativeMap.Layers.Add(labelLayer);
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
            Assembly assembly = typeof(App).GetTypeInfo().Assembly;
			Stream image = assembly.GetManifestResourceStream(icon);
			int bitmapId = BitmapRegistry.Instance.Register(image);

			return new SymbolStyle { BitmapId = bitmapId, SymbolScale = 0.2, SymbolOffset = new Offset(0, 72) };
		}

		private static Features GetLabelFeatures(IEnumerable<Location> entries)
		{
			var features = new Features();
			foreach (Location entry in entries)
			{
                string text = entry.Distance.ToString();
                switch (entry.DistanceType)
                {
                    case DistanceType.Kilometers:
                        text = string.Format(AppResources.DistanceType_Kilometers_Text, entry.Distance);
                        break;
                    case DistanceType.NauticalMiles:
                        text = string.Format(AppResources.DistanceType_NauticalMiles_Text, entry.Distance);
                        break;
                    case DistanceType.Miles:
                        text = string.Format(AppResources.DistanceType_Miles_Text, entry.Distance);
                        break;
                }

				Feature feature = new Feature { Geometry = SphericalMercator.FromLonLat(entry.Longitude, entry.Latitude) };
				feature.Styles.Add(new LabelStyle
					{
                        Text = text,
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