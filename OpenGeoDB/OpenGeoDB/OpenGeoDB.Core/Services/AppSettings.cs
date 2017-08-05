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

        public AppSettings(ISettings settings)
        {
            _settings = settings;
        }
    }
}
