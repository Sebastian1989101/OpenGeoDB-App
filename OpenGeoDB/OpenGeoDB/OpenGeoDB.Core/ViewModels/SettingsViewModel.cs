using System;
using System.Collections.Generic;
using System.Linq;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using OpenGeoDB.Core.DependencyServices;
using OpenGeoDB.Core.Model.Data;
using OpenGeoDB.Core.Resources;
using OpenGeoDB.Core.Services;

namespace OpenGeoDB.Core.ViewModels
{
    public class SettingsViewModel : MvxViewModel
	{
		public string Version { get; }
		public IAppSettings Settings { get; }

        public List<int> AvailableNearbyMarkerCount => Enumerable.Range(0, 20).ToList();
        public List<DistanceType> AvailableDistanceTypes => Enum.GetValues(typeof(DistanceType)).Cast<DistanceType>().ToList();

        public MvxCommand ShowUsedSoftwareCommand { get; }
        public MvxCommand ShowPrivacyPolicyCommand { get; }
        public MvxCommand ShowImprintCommand { get; }

        public SettingsViewModel(IVersionService versionService, IAppSettings settings, IMvxNavigationService navigationService)
		{
            Version = versionService.GetAppVersion();
            Settings = settings;

            ShowUsedSoftwareCommand = new MvxCommand(() => navigationService.Navigate<UsedSoftwareViewModel>());
            ShowPrivacyPolicyCommand = new MvxCommand(() => navigationService.Navigate<LegalContentViewModel, string[]>(new[] { AppResources.ViewCell_PrivacyPolicy, AppResources.PrivacyPolicy_Content }));
            ShowImprintCommand = new MvxCommand(() => navigationService.Navigate<LegalContentViewModel, string[]>(new[] { AppResources.ViewCell_Imprint, AppResources.Imprint_Content }));
        }
    }
}
