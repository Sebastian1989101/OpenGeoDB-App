using System.Linq;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using OpenGeoDB.Core.Model.Data;
using OpenGeoDB.Core.Services;

namespace OpenGeoDB.Core.ViewModels
{
    public class ChooseZipViewModel : MvxViewModel<Location[]>
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IDeviceInfoService _deviceInfoService;

        public Location[] Data { get; private set; }

        public double BottomSafeArea => _deviceInfoService.GetDeviceMargins().Bottom;

        public MvxCommand<string> ShowDetailsCommand { get; }

        public ChooseZipViewModel(IMvxNavigationService navigationService, IDeviceInfoService deviceInfoService)
        {
            _navigationService = navigationService;
            _deviceInfoService = deviceInfoService;

            ShowDetailsCommand = new MvxCommand<string>(OnShowDetailsCommandExecute, CanExecuteShowDetailsCommand);

            _deviceInfoService.DeviceMarginsChanged += delegate { RaisePropertyChanged(nameof(BottomSafeArea)); };
        }

        public override void Prepare(Location[] parameter)
        {
			Data = parameter;
		}

		private void OnShowDetailsCommandExecute(string key)
		{
			var result = Data.First(data => data.ZipCode == key);
            if (result != null)
                _navigationService.Navigate<DetailViewModel, Location>(result);
		}

		private bool CanExecuteShowDetailsCommand(string key)
		{
            return !string.IsNullOrEmpty(key) && Data.Any(data => data.ZipCode == key);
		}
    }
}
