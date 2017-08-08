using System;
using OpenGeoDB.Core.Model.Data;
using Plugin.Settings.Abstractions;

namespace OpenGeoDB.Core.Services
{
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
			get { return _settings.GetValueOrDefault(nameof(NearbyMarkerCount), 10); }
			set { _settings.AddOrUpdateValue(nameof(NearbyMarkerCount), value); }
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
