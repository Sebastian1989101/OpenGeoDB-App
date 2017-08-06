using CoreGraphics;
using Foundation;
using OpenGeoDB.Core.Controls;
using OpenGeoDB.iOS.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(MapsuiMap), typeof(MapsuiMapRenderer))]
namespace OpenGeoDB.iOS.Renderer
{
    [Preserve(AllMembers = true)]
    public class MapsuiMapRenderer : ViewRenderer<MapsuiMap, Mapsui.UI.iOS.MapControl>
    {
        Mapsui.UI.iOS.MapControl _mapNativeControl;
        MapsuiMap _mapViewControl;

        protected override void OnElementChanged(ElementChangedEventArgs<MapsuiMap> e)
        {
            base.OnElementChanged(e);
            if (_mapViewControl == null && e.NewElement != null)
                _mapViewControl = e.NewElement;

            if (_mapNativeControl == null && _mapViewControl != null)
            {
                var rectangle = _mapViewControl.Bounds;
                var rect = new CGRect(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);

                _mapNativeControl = new Mapsui.UI.iOS.MapControl(rect)
                    {
                        Map = _mapViewControl.NativeMap,
                        Frame = rect
                    };

                SetNativeControl(_mapNativeControl);
            }
        }
    }
}