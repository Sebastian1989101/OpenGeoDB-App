using System;
using OpenGeoDB.Core.Model.Data;
using Plugin.Settings.Abstractions;

namespace OpenGeoDB.Core.Services
{
    public interface IAppSettings
    {
        bool OrderByZipCode { get; set; }

        int NearbyMarkerCount { get; set; }
        bool ShowZipCodeAboveNearbyMarker { get; set; }
        DistanceType DistanceType { get; set; }
    }

    public class AppSettings : IAppSettings
    {
		private readonly ISettings _settings;

		public bool OrderByZipCode
		{
			get { return _settings.GetValueOrDefault(nameof(OrderByZipCode), false); }
			set { _settings.AddOrUpdateValue(nameof(OrderByZipCode), value); }
		}

        public int NearbyMarkerCount
        {
            get { return _settings.GetValueOrDefault(nameof(NearbyMarkerCount), 8); }
            set { _settings.AddOrUpdateValue(nameof(NearbyMarkerCount), value); }
        }

		public bool ShowZipCodeAboveNearbyMarker
		{
			get { return _settings.GetValueOrDefault(nameof(ShowZipCodeAboveNearbyMarker), true); }
			set { _settings.AddOrUpdateValue(nameof(ShowZipCodeAboveNearbyMarker), value); }
		}

		public DistanceType DistanceType
		{
            get { return (DistanceType)Enum.Parse(typeof(DistanceType), _settings.GetValueOrDefault(nameof(DistanceType), DistanceType.Kilometers.ToString())); }
            set { _settings.AddOrUpdateValue(nameof(DistanceType), value.ToString()); }
		}

        public AppSettings(ISettings settings)
        {
            _settings = settings;
        }
    }
}
