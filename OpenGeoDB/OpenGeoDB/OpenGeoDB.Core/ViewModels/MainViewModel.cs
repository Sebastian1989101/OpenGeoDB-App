using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using OpenGeoDB.Core.Model.Data;
using OpenGeoDB.Core.Repository;
using OpenGeoDB.Core.Services;

namespace OpenGeoDB.Core.ViewModels
{
    public class MainViewModel : MvxViewModel
	{
		private readonly LocationRepository _locationsRepository;
        private readonly IAppSettings _appSettings;

        private readonly IMvxNavigationService _navigationService;

	    private Location[] _locations;
        string filter;

        public string Filter
        {
            get { return filter; }
            set
            {
                filter = value;
                if (string.IsNullOrEmpty(filter))
                    FilterLocationsCommand.Execute(null);
            }
        }

        public LocationCategoryGroup[] Data { get; private set; }

        public MvxCommand ShowSettingsCommand { get; }

        public MvxCommand FilterLocationsCommand { get; }
        public MvxCommand<string> ShowDetailsCommand { get; }

        public MainViewModel(LocationRepository locationsRepository, IAppSettings appSettings, IMvxNavigationService navigationService)
        {
            // Fields
            _locationsRepository = locationsRepository;
			_appSettings = appSettings;

            _navigationService = navigationService;

            // Commands
            ShowSettingsCommand = new MvxCommand(() => _navigationService.Navigate<SettingsViewModel>());

            FilterLocationsCommand = new MvxCommand(OnFilterLocationsCommandExecute);
            ShowDetailsCommand = new MvxCommand<string>(OnShowDetailsCommandExecute, CanExecuteShowDetailsCommand);
        }

        public override async void Start()
        {
            await Initialize();
        }

        public override void ViewAppearing()
        {
            FilterLocationsCommand.Execute(null); 
            base.ViewAppearing();
        }

	    public override async Task Initialize()
	    {
	        _locations = await _locationsRepository.GetAllAsync();
        }

		private void OnFilterLocationsCommandExecute()
		{
		    if (_locations == null)
		        return;

            List<LocationCategoryGroup> groupData = new List<LocationCategoryGroup>();

            if (_appSettings.OrderByZipCode)
            {
                IEnumerable<IGrouping<string, Location>> data = _locations
                    .Where(location => location.Equals(Filter))
                    .OrderBy(location => location.ZipCode)
                    .GroupBy(location => location.Village);

                foreach (var grouping in data.GroupBy(d => d.First().ZipCode[0]))
                {
                    LocationCategoryGroup currentGroupData = new LocationCategoryGroup(_appSettings);
                    foreach (var compendiumEntry in grouping)
                        currentGroupData.Add(compendiumEntry);

                    groupData.Add(currentGroupData);
                }
            }
            else
            {
                IEnumerable<IGrouping<string, Location>> data = _locations
                    .Where(location => location.Equals(Filter))
                    .OrderBy(location => location.Village)
                    .GroupBy(location => location.Village);

                foreach (var grouping in data.GroupBy(d => d.First().Village[0]))
                {
                    LocationCategoryGroup currentGroupData = new LocationCategoryGroup(_appSettings);
                    foreach (var compendiumEntry in grouping)
                        currentGroupData.Add(compendiumEntry);

                    groupData.Add(currentGroupData);
                }
            }

            Data = groupData.ToArray();
            RaisePropertyChanged(() => Data);
		}
		
		private void OnShowDetailsCommandExecute(string key)
		{
            Location[] result = Data.First(x => x.Any(y => y.Key == key))
                                    .First(data => data.Key == key)
                                    .Select(grp => grp).ToArray();
            
            if (result.Length == 1)
                _navigationService.Navigate<DetailViewModel, Location>(result.First());
            else 
                _navigationService.Navigate<ChooseZipViewModel, Location[]>(result);
		}

		private bool CanExecuteShowDetailsCommand(string key)
		{
            return !string.IsNullOrEmpty(key) && Data.First(x => x.Any(y => y.Key == key)).Any(data => data.Key == key);
		}
    }
}