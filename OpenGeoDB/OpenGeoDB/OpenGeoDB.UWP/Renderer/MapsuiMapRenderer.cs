using OpenGeoDB.Core.Controls;
using OpenGeoDB.UWP.Renderer;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(MapsuiMap), typeof(MapsuiMapRenderer))]
namespace OpenGeoDB.UWP.Renderer
{
    public class MapsuiMapRenderer : ViewRenderer<MapsuiMap, Mapsui.UI.Uwp.MapControl>
    {
        Mapsui.UI.Uwp.MapControl _mapNativeControl;
        MapsuiMap _mapViewControl;

        protected override void OnElementChanged(ElementChangedEventArgs<MapsuiMap> e)
        {
            base.OnElementChanged(e);
            if (_mapViewControl == null && e.NewElement != null)
                _mapViewControl = e.NewElement;

            if (_mapNativeControl == null && _mapViewControl != null)
            {
                _mapNativeControl = new Mapsui.UI.Uwp.MapControl
                    {
                        Map = _mapViewControl.NativeMap
                    };

                SetNativeControl(_mapNativeControl);
            }
        }
    }
}