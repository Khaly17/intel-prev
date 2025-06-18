using Microsoft.Maui.Controls;
using Soditech.IntelPrev.Mobile.ViewModels.Settings;

namespace Soditech.IntelPrev.Mobile.Views.Settings;

public partial class SettingsView : ContentPage, IXamarinView
{
	private readonly SettingsViewModel _viewModel;

	public SettingsView()
	{
		InitializeComponent();
		_viewModel = (SettingsViewModel)BindingContext;
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		_viewModel?.InitializeAsync();
	}

	protected override bool OnBackButtonPressed()
	{
		return base.OnBackButtonPressed();
	}
}