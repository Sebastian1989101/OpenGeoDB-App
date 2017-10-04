using MvvmCross.Core.ViewModels;

namespace OpenGeoDB.Core.ViewModels
{
    public class LegalContentViewModel : MvxViewModel<string[]>
    {
        public string Title { get; private set; }
        public string Content { get; private set; }

        public override void Prepare(string[] parameter)
        {
            Title = parameter[0];
            Content = parameter[1];
        }
    }
}