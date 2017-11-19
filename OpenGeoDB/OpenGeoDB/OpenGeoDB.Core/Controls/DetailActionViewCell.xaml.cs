using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OpenGeoDB.Core.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailActionViewCell 
    {
        public static readonly BindableProperty TextProperty = BindableProperty.Create(
            nameof(Text), typeof(string), typeof(DetailActionViewCell), null, BindingMode.OneWay, null, OnTextChanged);

        public static readonly BindableProperty DetailProperty = BindableProperty.Create(
            nameof(Detail), typeof(string), typeof(DetailActionViewCell), null, BindingMode.OneWay, null, OnDetailChanged);

        public static readonly BindableProperty CommandProperty = BindableProperty.Create(
            nameof(Command), typeof(ICommand), typeof(DetailActionViewCell));

        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(
            nameof(CommandParameter), typeof(object), typeof(DetailActionViewCell));
        
        protected override Layout Layout => CellLayout;

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public string Detail
        {
            get => (string)GetValue(DetailProperty);
            set => SetValue(DetailProperty, value);
        }

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        public DetailActionViewCell()
        {
            InitializeComponent();
            Tapped += delegate { Command?.Execute(CommandParameter); };
            
            DetailLabel.FontSize = TextLabel.FontSize * .7;
            DetailLabel.TextColor = Color.Accent;
        }

        private static void OnTextChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is DetailActionViewCell viewCell)
                viewCell.TextLabel.Text = newvalue?.ToString();
        }

        private static void OnDetailChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is DetailActionViewCell viewCell)
            {
                viewCell.DetailLabel.Text = newvalue?.ToString();
                viewCell.DetailLabel.IsVisible = newvalue != null;
            }
        }
    }
}