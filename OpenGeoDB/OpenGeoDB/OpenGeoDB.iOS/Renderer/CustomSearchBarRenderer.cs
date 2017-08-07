using System;
using System.Drawing;
using OpenGeoDB.iOS.Renderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(SearchBar), typeof(CustomSearchBarRenderer))]
namespace OpenGeoDB.iOS.Renderer
{
	public class CustomSearchBarRenderer : SearchBarRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
		{
			base.OnElementChanged(e);

			if (Control != null)
			{
				if (e?.NewElement != null && Control != null)
				{
					Control.InputAccessoryView = new UIToolbar(new RectangleF(0, 0, Convert.ToSingle(e.NewElement.WidthRequest), 44))
						{
							Translucent = true,
							Items = new[]
								{
									new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
									new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate { Control.ResignFirstResponder(); })
								}
						};
				}
			}
		}
	}
}
