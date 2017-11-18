using Xamarin.Forms;

namespace OpenGeoDB.Core.Pages
{
    public partial class DetailPage
    {
        public DetailPage()
        {
            InitializeComponent();
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            if (width > height)
            {
                Grid.SetRow(LatitudePanel, 0);
                Grid.SetColumn(LatitudePanel, 0);
                Grid.SetColumnSpan(LatitudePanel, 1);

                Grid.SetRow(LongitudePanel, 0);
                Grid.SetColumn(LongitudePanel, 1);
                Grid.SetColumnSpan(LongitudePanel, 1);

                LatLongGrid.RowSpacing = 0;
            }
            else
            {
                Grid.SetRow(LatitudePanel, 0);
                Grid.SetColumn(LatitudePanel, 0);
                Grid.SetColumnSpan(LatitudePanel, 2);

                Grid.SetRow(LongitudePanel, 1);
                Grid.SetColumn(LongitudePanel, 0);
                Grid.SetColumnSpan(LongitudePanel, 2);

                LatLongGrid.RowSpacing = 10;
            }

            base.OnSizeAllocated(width, height);
        }
    }
}