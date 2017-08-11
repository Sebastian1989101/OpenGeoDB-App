using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;

namespace OpenGeoDB.Core.ViewModels
{
    public class LegalContentViewModel : MvxViewModel<string>
    {
        public string Content { get; private set; }

        public override Task Initialize(string parameter)
        {
            Content = parameter;
            return App.CompletedTask;
        }
    }
}