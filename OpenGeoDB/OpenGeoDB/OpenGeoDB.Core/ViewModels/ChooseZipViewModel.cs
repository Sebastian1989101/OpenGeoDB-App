using System.Linq;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using OpenGeoDB.Core.Model.Data;

namespace OpenGeoDB.Core.ViewModels
{
    public class ChooseZipViewModel : MvxViewModel<Location[]>
    {
        private readonly IMvxNavigationService _navigationService;

        public Location[] Data { get; private set; }

        public MvxCommand<string> ShowDetailsCommand { get; }

        public ChooseZipViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            ShowDetailsCommand = new MvxCommand<string>(OnShowDetailsCommandExecute, CanExecuteShowDetailsCommand);
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
