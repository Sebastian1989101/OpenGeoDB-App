using System;
using System.Collections.Generic;
using System.Linq;
using MvvmCross.Core.ViewModels;
using OpenGeoDB.Core.DependencyServices;
using OpenGeoDB.Core.Model.Data;
using OpenGeoDB.Core.Services;

namespace OpenGeoDB.Core.ViewModels
{
    public class SettingsViewModel : MvxViewModel
	{
		public string Version { get; }
		public IAppSettings Settings { get; }

        public List<int> AvailableNearbyMarkerCount => Enumerable.Range(0, 20).ToList();
        public List<DistanceType> AvailableDistanceTypes => Enum.GetValues(typeof(DistanceType)).Cast<DistanceType>().ToList();

        public SettingsViewModel(IVersionService versionService, IAppSettings settings)
		{
            Version = versionService.GetAppVersion();
            Settings = settings;
        }
    }
}
