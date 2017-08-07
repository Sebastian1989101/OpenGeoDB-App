using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;
using OpenGeoDB.Core.Model.Data;

namespace OpenGeoDB.Core.ViewModels
{
    public class DetailViewModel : MvxViewModel<Location>
    {
        public Location Location { get; private set; }

        public override Task Initialize(Location parameter)
        {
            Location = parameter;
            return App.CompletedTask;
        }
    }
}