using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;
using OpenGeoDB.Core.Model.Data;
using OpenGeoDB.Core.Repository;

namespace OpenGeoDB.Core.ViewModels
{
    public class DetailViewModel : MvxViewModel<Location>
	{
		private readonly LocationRepository _locationsRepository;

		public Location Location { get; private set; }
        public Location[] NearbyMarker { get; private set; }

        public MvxCommand<Location> ChangeLocationCommand { get; }

        public DetailViewModel(LocationRepository locationsRepository)
        {
            _locationsRepository = locationsRepository;

            ChangeLocationCommand = new MvxCommand<Location>(location => ShowViewModel<DetailViewModel, Location>(location));
        }

        public override async Task Initialize(Location parameter)
        {
            Location = parameter;
            NearbyMarker = await _locationsRepository.GetNearbyEntries(parameter, 10, false);
        }
    }
}