using System.Windows.Input;

namespace Soditech.IntelPrev.Mobile.Views.Components
{
	public partial class ConfirmationScreenView : ContentView
	{
		public static readonly BindableProperty MainMessageProperty =
			BindableProperty.Create(nameof(MainMessage), typeof(string), typeof(ConfirmationScreenView),
				defaultValue: "Votre alerte a été envoyée avec succès!");

		public static readonly BindableProperty SubMessageProperty =
			BindableProperty.Create(nameof(SubMessage), typeof(string), typeof(ConfirmationScreenView),
				defaultValue: "Que souhaitez-vous faire?");

		public static readonly BindableProperty PrimaryButtonTextProperty =
			BindableProperty.Create(nameof(PrimaryButtonText), typeof(string), typeof(ConfirmationScreenView),
				defaultValue: "Effectuer une autre alerte");

		public static readonly BindableProperty SecondaryButtonTextProperty =
			BindableProperty.Create(nameof(SecondaryButtonText), typeof(string), typeof(ConfirmationScreenView),
				defaultValue: "Retourner à l'Accueil");

		public static readonly BindableProperty PrimaryButtonCommandProperty =
			BindableProperty.Create(nameof(PrimaryButtonCommand), typeof(ICommand), typeof(ConfirmationScreenView));

		public static readonly BindableProperty SecondaryButtonCommandProperty =
			BindableProperty.Create(nameof(SecondaryButtonCommand), typeof(ICommand), typeof(ConfirmationScreenView));

		public static readonly BindableProperty HeaderBackgroundColorProperty =
			BindableProperty.Create(nameof(HeaderBackgroundColor), typeof(Color), typeof(ConfirmationScreenView),
				defaultValue: Colors.Black);

		public string MainMessage
		{
			get => (string)GetValue(MainMessageProperty);
			set => SetValue(MainMessageProperty, value);
		}

		public string SubMessage
		{
			get => (string)GetValue(SubMessageProperty);
			set => SetValue(SubMessageProperty, value);
		}

		public string PrimaryButtonText
		{
			get => (string)GetValue(PrimaryButtonTextProperty);
			set => SetValue(PrimaryButtonTextProperty, value);
		}

		public string SecondaryButtonText
		{
			get => (string)GetValue(SecondaryButtonTextProperty);
			set => SetValue(SecondaryButtonTextProperty, value);
		}

		public ICommand PrimaryButtonCommand
		{
			get => (ICommand)GetValue(PrimaryButtonCommandProperty);
			set => SetValue(PrimaryButtonCommandProperty, value);
		}

		public ICommand SecondaryButtonCommand
		{
			get => (ICommand)GetValue(SecondaryButtonCommandProperty);
			set => SetValue(SecondaryButtonCommandProperty, value);
		}

		public Color HeaderBackgroundColor
		{
			get => (Color)GetValue(HeaderBackgroundColorProperty);
			set => SetValue(HeaderBackgroundColorProperty, value);
		}

		public ConfirmationScreenView()
		{
			InitializeComponent();
		}
	}
}