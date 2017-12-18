using System;
using System.Collections.Generic;
using System.Linq;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using OpenGeoDB.Core.Services;
using OpenGeoDB.Core.Model.Data;
using OpenGeoDB.Core.Resources;
using OpenGeoDB.Core.DependencyServices;
using OpenGeoDB.Core.Converters;
using Acr.UserDialogs;
using System.Globalization;
using Xamarin.Forms;

namespace OpenGeoDB.Core.ViewModels
{
    public class SettingsViewModel : MvxViewModel
	{
        private readonly IUserDialogs _userDialogs;

		public string Version { get; }
		public IAppSettings Settings { get; }

        public MvxCommand ChooseNearbyMarkerCountCommand { get; }
        public MvxCommand ChooseDistanceTypeCommand { get; }

        public MvxCommand ShowUsedSoftwareCommand { get; }
        public MvxCommand ShowSourcecodeCommand { get; }
        public MvxCommand ShowPrivacyPolicyCommand { get; }
        public MvxCommand ShowImprintCommand { get; }

        public SettingsViewModel(IVersionService versionService, IAppSettings settings, IUserDialogs userDialogs, IMvxNavigationService navigationService)
		{
            _userDialogs = userDialogs;

            Version = versionService.GetAppVersion();
            Settings = settings;

            ChooseNearbyMarkerCountCommand = new MvxCommand(OnChooseNearbyMarkerCountCommandExecute);
            ChooseDistanceTypeCommand = new MvxCommand(OnChooseDistanceTypeCommandExecute);

            ShowUsedSoftwareCommand = new MvxCommand(() => navigationService.Navigate<UsedSoftwareViewModel>());
            ShowSourcecodeCommand = new MvxCommand(() => Device.OpenUri(new Uri("https://github.com/Sebastian1989101/OpenGeoDB-App")));
            ShowPrivacyPolicyCommand = new MvxCommand(() => navigationService.Navigate<LegalContentViewModel, string[]>(new[] { AppResources.ViewCell_PrivacyPolicy, AppResources.PrivacyPolicy_Content }));
            ShowImprintCommand = new MvxCommand(() => navigationService.Navigate<LegalContentViewModel, string[]>(new[] { AppResources.ViewCell_Imprint, AppResources.Imprint_Content }));
        }

        private void OnChooseNearbyMarkerCountCommandExecute()
        {
            var actionSheetConfig = new ActionSheetConfig();
            actionSheetConfig.Title = AppResources.ViewCell_NearbyMarkerCount;

            actionSheetConfig.Add("0", () => { Settings.NearbyMarkerCount = 0; RaisePropertyChanged(nameof(Settings)); });
            foreach (var count in Enumerable.Range(0, 8).Select(i => Convert.ToInt32(Math.Pow(2, i))))
                actionSheetConfig.Add(count.ToString(), () => { Settings.NearbyMarkerCount = count; RaisePropertyChanged(nameof(Settings)); });

            _userDialogs.ActionSheet(actionSheetConfig);
        }

        private void OnChooseDistanceTypeCommandExecute()
        {
            var converter = new DistanceTypeConverter();

            var actionSheetConfig = new ActionSheetConfig();
            actionSheetConfig.Title = AppResources.ViewCell_DistanceType;

            foreach (var distanceType in Enum.GetValues(typeof(DistanceType)).Cast<DistanceType>())
            {
                var text = converter.Convert(distanceType, typeof(string), null, CultureInfo.InvariantCulture).ToString();
                actionSheetConfig.Add(text, () => { Settings.DistanceType = distanceType; RaisePropertyChanged(nameof(Settings)); });
            }

            _userDialogs.ActionSheet(actionSheetConfig);
        }
    }
}
