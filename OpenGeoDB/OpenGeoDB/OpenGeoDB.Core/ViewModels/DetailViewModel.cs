using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;
using OpenGeoDB.Core.Model.Data;
using OpenGeoDB.Core.Repository;
using OpenGeoDB.Core.Services;

namespace OpenGeoDB.Core.ViewModels
{
    public class DetailViewModel : MvxViewModel<Location>
	{
		private readonly LocationRepository _locationsRepository;
		private readonly IAppSettings _appSettings;

		public Location Location { get; private set; }
        public Location[] NearbyMarker { get; private set; }

        public MvxCommand<Location> ChangeLocationCommand { get; }

        public DetailViewModel(LocationRepository locationsRepository, IAppSettings appSettings)
        {
			_locationsRepository = locationsRepository;
            _appSettings = appSettings;

            ChangeLocationCommand = new MvxCommand<Location>(location => ShowViewModel<DetailViewModel, Location>(location));
        }

        public override async Task Initialize(Location parameter)
        {
            Location = parameter;
            NearbyMarker = await _locationsRepository.GetNearbyEntries(parameter, _appSettings.NearbyMarkerCount, false);
        }
    }
}