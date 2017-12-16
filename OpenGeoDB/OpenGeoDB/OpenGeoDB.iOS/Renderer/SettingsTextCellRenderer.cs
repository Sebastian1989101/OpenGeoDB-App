using OpenGeoDB.Core.Controls;
using OpenGeoDB.iOS.Renderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(SettingsTextCell), typeof(SettingsTextCellRenderer))]
namespace OpenGeoDB.iOS.Renderer
{
    public class SettingsTextCellRenderer : TextCellRenderer
    {
        public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
        {
            var textCell = (TextCell)item;

            var tableViewCell = reusableCell as CellTableViewCell;
            if (tableViewCell == null)
                tableViewCell = new CellTableViewCell(UITableViewCellStyle.Value1, item.GetType().FullName);
            else
                tableViewCell.Cell.PropertyChanged -= tableViewCell.HandlePropertyChanged;

            tableViewCell.Cell = textCell;
            textCell.PropertyChanged += tableViewCell.HandlePropertyChanged;
            tableViewCell.PropertyChanged = HandlePropertyChanged;

            tableViewCell.TextLabel.Text = textCell.Text;
            tableViewCell.DetailTextLabel.Text = textCell.Detail;

            WireUpForceUpdateSizeRequested(item, tableViewCell, tv);

            UpdateIsEnabled(tableViewCell, textCell);

            UpdateBackground(tableViewCell, item);
            tableViewCell.Accessory = UITableViewCellAccessory.DisclosureIndicator;

            return tableViewCell;
        }

        static void UpdateIsEnabled(CellTableViewCell cell, TextCell entryCell)
        {
            cell.UserInteractionEnabled = entryCell.IsEnabled;
            cell.TextLabel.Enabled = entryCell.IsEnabled;
            cell.DetailTextLabel.Enabled = entryCell.IsEnabled;
        }
    }
}