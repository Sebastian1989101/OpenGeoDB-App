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
                Grid.SetRow(LatitudePanel, 1);
                Grid.SetColumn(LatitudePanel, 0);
                Grid.SetColumnSpan(LatitudePanel, 1);

                Grid.SetRow(LongitudePanel, 1);
                Grid.SetColumn(LongitudePanel, 1);
                Grid.SetColumnSpan(LongitudePanel, 1);

                Grid.SetRow(MapsuiPanel, 2);
                Grid.SetRowSpan(MapsuiPanel, 2);
            }
            else
            {
                Grid.SetRow(LatitudePanel, 1);
                Grid.SetColumn(LatitudePanel, 0);
                Grid.SetColumnSpan(LatitudePanel, 2);

                Grid.SetRow(LongitudePanel, 2);
                Grid.SetColumn(LongitudePanel, 0);
                Grid.SetColumnSpan(LongitudePanel, 2);

                Grid.SetRow(MapsuiPanel, 3);
                Grid.SetRowSpan(MapsuiPanel, 1);
            }

            base.OnSizeAllocated(width, height);
        }
    }
}