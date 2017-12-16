using OpenGeoDB.Core.Controls;
using OpenGeoDB.iOS.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(NavigationTextCell), typeof(NavigationTextCellRenderer))]
namespace OpenGeoDB.iOS.Renderer
{
    public class NavigationTextCellRenderer : TextCellRenderer
    {
        public override UIKit.UITableViewCell GetCell(Cell item, UIKit.UITableViewCell reusableCell, UIKit.UITableView tv)
        {
            var cell = base.GetCell(item, reusableCell, tv);
            cell.Accessory = UIKit.UITableViewCellAccessory.DisclosureIndicator;

            return cell;
        }
    }
}
