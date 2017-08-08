using OpenGeoDB.iOS.Renderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Picker), typeof(CustomPickerRenderer))]
namespace OpenGeoDB.iOS.Renderer
{
    public class CustomPickerRenderer : PickerRenderer
    {
		protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
		{
			base.OnElementChanged(e);
            if (Control != null)
                Control.TextAlignment = UITextAlignment.Right;
		}
    }
}
