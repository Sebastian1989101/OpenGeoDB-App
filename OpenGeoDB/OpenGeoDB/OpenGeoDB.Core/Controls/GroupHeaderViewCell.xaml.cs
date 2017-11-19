using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OpenGeoDB.Core.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GroupHeaderViewCell
    {
        public static readonly BindableProperty HeaderProperty = BindableProperty.Create(
            nameof(Header), typeof(string), typeof(GroupHeaderViewCell), null, BindingMode.OneWay, null, OnHeaderChanged);

        protected override Layout Layout => CellLayout;

        public string Header
        {
            get => (string)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        public GroupHeaderViewCell()
        {
            InitializeComponent();
        }

        private static void OnHeaderChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is GroupHeaderViewCell viewCell)
                viewCell.HeaderLabel.Text = newvalue?.ToString();
        }
    }
}