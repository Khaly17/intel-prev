using System.Windows.Input;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;

namespace Soditech.IntelPrev.Mobile.Views.Components
{
	public partial class SectionCard : ContentView
	{
		public static readonly BindableProperty TitleProperty =
			BindableProperty.Create(nameof(Title), typeof(string), typeof(SectionCard), string.Empty);

		public static readonly BindableProperty IsExpandedProperty =
			BindableProperty.Create(nameof(IsExpanded), typeof(bool), typeof(SectionCard), true,
				propertyChanged: OnIsExpandedChanged);

		public static readonly BindableProperty FieldsProperty =
			BindableProperty.Create(nameof(Fields), typeof(ObservableCollection<object>), typeof(SectionCard), null);

		//make toggleCommand bindable
		public static readonly BindableProperty ToggleCommandProperty =
			BindableProperty.Create(nameof(ToggleCommand), typeof(ICommand), typeof(SectionCard), null);

		public ICommand ToggleCommand { get; private set; }

		public string Title
		{
			get => (string)GetValue(TitleProperty);
			set => SetValue(TitleProperty, value);
		}

		public bool IsExpanded
		{
			get => (bool)GetValue(IsExpandedProperty);
			set => SetValue(IsExpandedProperty, value);
		}

		public ObservableCollection<object> Fields
		{
			get => (ObservableCollection<object>)GetValue(FieldsProperty);
			set => SetValue(FieldsProperty, value);
		}

		public SectionCard()
		{
			InitializeComponent();
			ToggleCommand = new Command(() => IsExpanded = !IsExpanded);
			BindingContext = this;
		}

		private static void OnIsExpandedChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var control = (SectionCard)bindable;
			// Animation could be added here
		}
	}
}