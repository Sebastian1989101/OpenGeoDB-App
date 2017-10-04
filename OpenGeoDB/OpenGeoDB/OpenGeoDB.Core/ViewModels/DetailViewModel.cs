using System.ComponentModel;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;
using OpenGeoDB.Core.Model.Data;
using OpenGeoDB.Core.Repository;
using OpenGeoDB.Core.Services;

namespace OpenGeoDB.Core.ViewModels
{
    public class DetailViewModel : MvxViewModel<Location>, INotifyPropertyChanged
	{
		private readonly LocationRepository _locationsRepository;
		private readonly IAppSettings _appSettings;

		public Location Location { get; private set; }
        public Location[] NearbyMarker { get; private set; }

        public MvxAsyncCommand<Location> ChangeLocationCommand { get; }

        public DetailViewModel(LocationRepository locationsRepository, IAppSettings appSettings)
        {
			_locationsRepository = locationsRepository;
            _appSettings = appSettings;

            ChangeLocationCommand = new MvxAsyncCommand<Location>(OnChangeLocationCommandExecute);
        }

        public override async void Prepare(Location parameter)
        {
            await SetLocation(parameter);
		}

		private async Task OnChangeLocationCommandExecute(Location location)
		{
			await SetLocation(location);
		}

        private async Task SetLocation(Location location)
        {
			Location = location;
			NearbyMarker = await _locationsRepository.GetNearbyEntries(location, _appSettings.NearbyMarkerCount, false);
        }
    }
}