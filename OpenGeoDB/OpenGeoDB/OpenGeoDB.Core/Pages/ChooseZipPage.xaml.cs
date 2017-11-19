using Xamarin.Forms;

namespace OpenGeoDB.Core.Pages
{
    public partial class ChooseZipPage
    {
        public ChooseZipPage()
        {
            InitializeComponent();
        }

        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (sender is ListView listView)
                listView.SelectedItem = null;
        }
    }
}
