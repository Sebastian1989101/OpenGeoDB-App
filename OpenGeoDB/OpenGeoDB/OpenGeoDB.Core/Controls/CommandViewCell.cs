using System.Windows.Input;
using Xamarin.Forms;

namespace OpenGeoDB.Core.Controls
{
    public class CommandViewCell : CommandViewCell<object>
	{

	}

	public class CommandViewCell<TCommandParameter> : ViewCell
	{
		public static readonly BindableProperty CommandProperty = BindableProperty.Create(
			nameof(Command), typeof(ICommand), typeof(CommandViewCell<TCommandParameter>));

		public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(
			nameof(CommandParameter), typeof(TCommandParameter), typeof(CommandViewCell<TCommandParameter>));

		public ICommand Command
		{
			get { return (ICommand)GetValue(CommandProperty); }
			set { SetValue(CommandProperty, value); }
		}

		public TCommandParameter CommandParameter
		{
			get { return (TCommandParameter)GetValue(CommandParameterProperty); }
			set { SetValue(CommandParameterProperty, value); }
		}

		public CommandViewCell()
		{
			Tapped += (sender, e) =>
				{
					if (Command?.CanExecute(CommandParameter) == true)
						Command?.Execute(CommandParameter);
				};
		}
	}
}
