using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;
using OpenGeoDB.Core.Model.Data;

namespace OpenGeoDB.Core.ViewModels
{
    public class ChooseZipViewModel : MvxViewModel<Location[]>
    {
        public Location[] Data { get; private set; }

		public MvxCommand<string> ShowDetailsCommand { get; }

        public ChooseZipViewModel()
        {
            ShowDetailsCommand = new MvxCommand<string>(OnShowDetailsCommandExecute, CanExecuteShowDetailsCommand);   
        }

        public override Task Initialize(Location[] parameter)
        {
			Data = parameter;
			return App.CompletedTask;
		}

		private void OnShowDetailsCommandExecute(string key)
		{
			var result = Data.First(data => data.ZipCode == key);
            if (result != null)
                ShowViewModel<DetailViewModel, Location>(result);
		}

		private bool CanExecuteShowDetailsCommand(string key)
		{
            return !string.IsNullOrEmpty(key) && Data.Any(data => data.ZipCode == key);
		}
    }
}
