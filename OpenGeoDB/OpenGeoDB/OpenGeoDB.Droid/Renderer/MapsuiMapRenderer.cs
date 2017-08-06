using OpenGeoDB.Core.Controls;
using OpenGeoDB.Droid.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(MapsuiMap), typeof(MapsuiMapRenderer))]
namespace OpenGeoDB.Droid.Renderer
{
    public class MapsuiMapRenderer : ViewRenderer<MapsuiMap, Mapsui.UI.Android.MapControl>
    {
        Mapsui.UI.Android.MapControl _mapNativeControl;
        MapsuiMap _mapViewControl;

        protected override void OnElementChanged(ElementChangedEventArgs<MapsuiMap> e)
        {
            base.OnElementChanged(e);
            if (_mapViewControl == null && e.NewElement != null)
                _mapViewControl = e.NewElement;

            if (_mapNativeControl == null && _mapViewControl != null)
            {
                _mapNativeControl = new Mapsui.UI.Android.MapControl(Context, null)
                    {
                        Map = _mapViewControl.NativeMap, 
                    };

                SetNativeControl(_mapNativeControl);
            }
        }
    }
}