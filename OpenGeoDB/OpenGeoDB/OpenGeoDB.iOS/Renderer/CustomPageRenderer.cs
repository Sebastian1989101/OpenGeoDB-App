using OpenGeoDB.iOS.Renderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Page), typeof(CustomPageRenderer))]
namespace OpenGeoDB.iOS.Renderer
{
    public class CustomPageRenderer : PageRenderer
    {
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0) && NavigationController != null && NavigationController.NavigationBar != null)
                NavigationController.NavigationBar.PrefersLargeTitles = true;
        }
    }
}
