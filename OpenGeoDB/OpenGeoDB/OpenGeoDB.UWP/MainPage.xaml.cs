using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Forms.Uwp.Presenters;
using MvvmCross.Platform;
using System.Text;
using MvvmCross.Forms.Views;

namespace OpenGeoDB.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage 
    {
        public MainPage()
        {
            this.InitializeComponent();

            var start = Mvx.Resolve<IMvxAppStart>();
            start.Start();

            var presenter = Mvx.Resolve<IMvxFormsViewPresenter>() as MvxFormsUwpViewPresenter;
            if (presenter != null)
                LoadApplication(presenter.FormsApplication);

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }
    }
}
