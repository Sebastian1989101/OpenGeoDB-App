using System.ComponentModel;
using System.Threading.Tasks;
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
        private readonly IDeviceInfoService _deviceInfoService;

        public bool IsLoading { get; private set; }

        public Location Location { get; private set; }
        public Location[] NearbyMarker { get; private set; }

        public Thickness SafeAreaMargins 
        {
            get
            {
                var margin = _deviceInfoService.GetDeviceMargins();
                return new Thickness(margin.Left, 0, margin.Right, 0);
            }
        }

        public MvxAsyncCommand<Location> ChangeLocationCommand { get; }

        public DetailViewModel(LocationRepository locationsRepository, IAppSettings appSettings, IDeviceInfoService deviceInfoService)
        {
            _locationsRepository = locationsRepository;
            _appSettings = appSettings;
            _deviceInfoService = deviceInfoService;

            ChangeLocationCommand = new MvxAsyncCommand<Location>(OnChangeLocationCommandExecute);

            _deviceInfoService.DeviceMarginsChanged += delegate 
                {
                    RaisePropertyChanged(nameof(SafeAreaMargins));
                };
        }

        public override void Prepare(Location parameter)
        {
            Task.Run(() => SetLocation(parameter));
		}

		private async Task OnChangeLocationCommandExecute(Location location)
		{
			await SetLocation(location);
		}

        private async Task SetLocation(Location location)
        {
			try
            {
                IsLoading = true;

                Location = location;
                NearbyMarker = await _locationsRepository.GetNearbyEntries(location, _appSettings.NearbyMarkerCount, false);
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}