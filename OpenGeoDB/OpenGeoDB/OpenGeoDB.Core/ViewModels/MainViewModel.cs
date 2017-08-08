using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        private Location[] _locations;

        public string Filter { get; set; }

        public IGrouping<string, Location>[] Data { get; private set; }

        public MvxCommand ShowSettingsCommand { get; }

        public MvxCommand FilterLocationsCommand { get; }
        public MvxCommand<string> ShowDetailsCommand { get; }

        public MainViewModel(LocationRepository locationsRepository, IAppSettings appSettings)
        {
			// Fields
			_locationsRepository = locationsRepository;
			_appSettings = appSettings;

            // Commands
            ShowSettingsCommand = new MvxCommand(() => ShowViewModel<SettingsViewModel>());

            FilterLocationsCommand = new MvxCommand(OnFilterLocationsCommandExecute);
            ShowDetailsCommand = new MvxCommand<string>(OnShowDetailsCommandExecute, CanExecuteShowDetailsCommand);
        }

        public override void Start()
        {
            Initialize().Wait();
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
            IEnumerable<IGrouping<string, Location>> data = _locations
                .Where(location => location.Equals(Filter))
                .OrderBy(location => _appSettings.OrderByZipCode ? location.ZipCode : location.Village)
                .GroupBy(location => location.Village);

            Data = data.ToArray();
            RaisePropertyChanged(() => Data);
		}
		
		private void OnShowDetailsCommandExecute(string key)
		{
            var result = Data.First(data => data.Key == key).Select(grp => grp).ToArray();
            if (result.Length == 1)
                ShowViewModel<DetailViewModel, Location>(result.First());
            else 
                ShowViewModel<ChooseZipViewModel, Location[]>(result);
		}

		private bool CanExecuteShowDetailsCommand(string key)
		{
			return !string.IsNullOrEmpty(key) && Data.Any(data => data.Key == key);
		}
    }
}