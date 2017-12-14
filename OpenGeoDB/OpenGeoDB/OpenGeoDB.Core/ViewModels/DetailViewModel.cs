using System.ComponentModel;
using System.Threading.Tasks;
using Acr.UserDialogs;
using MvvmCross.Core.ViewModels;
using OpenGeoDB.Core.Model.Data;
using OpenGeoDB.Core.Repository;
using OpenGeoDB.Core.Services;
using Xamarin.Forms;

namespace OpenGeoDB.Core.ViewModels
{
    public class DetailViewModel : MvxViewModel<Location>, INotifyPropertyChanged
    {
        private readonly LocationRepository _locationsRepository;

        private readonly IAppSettings _appSettings;
        private readonly IProgressDialog _progressDialogs;
        private readonly IDeviceInfoService _deviceInfoService;

        public string Title => $"{Location.ZipCode} {Location.Village}";

        public Location Location { get; private set; }
        public Location[] NearbyMarker { get; private set; }

        public Thickness SafeAreaMargins 
        {
            get
            {
                var margin = _deviceInfoService.GetDeviceMargins();
                return new Thickness(margin.Left, 10, margin.Right, 0);
            }
        }

        public MvxAsyncCommand<Location> ChangeLocationCommand { get; }

        public DetailViewModel(LocationRepository locationsRepository, IAppSettings appSettings, IUserDialogs userDialogs, IDeviceInfoService deviceInfoService)
        {
            _locationsRepository = locationsRepository;

            _appSettings = appSettings;
            _deviceInfoService = deviceInfoService;

            _progressDialogs = userDialogs.Progress(new ProgressDialogConfig
                {
                    Title = Resources.AppResources.ProgressDialog_Loading,
                    IsDeterministic = false,
                    AutoShow = false
                });

            ChangeLocationCommand = new MvxAsyncCommand<Location>(OnChangeLocationCommandExecute);

            _deviceInfoService.DeviceMarginsChanged += delegate 
                {
                    RaisePropertyChanged(nameof(SafeAreaMargins));
                };
        }

        public override void Prepare(Location parameter)
        {
            Task.Run(() => SetLocation(parameter, true));
		}

		private async Task OnChangeLocationCommandExecute(Location location)
		{
			await SetLocation(location);
		}

        private async Task SetLocation(Location location, bool isPreparing = false)
        {
			try
            {
                if (!isPreparing)
                    _progressDialogs.Show();

                Location = location;
                NearbyMarker = await _locationsRepository.GetNearbyEntries(location, _appSettings.NearbyMarkerCount, false);
            }
            finally
            {
                if (!isPreparing && _progressDialogs.IsShowing)
                    _progressDialogs.Hide();
            }
        }
    }
}