using Xamarin.Forms;

namespace OpenGeoDB.Core.Pages
{
    public partial class MainPage 
    {
        public MainPage()
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
