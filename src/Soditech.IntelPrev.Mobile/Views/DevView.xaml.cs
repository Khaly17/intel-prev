using System;

namespace Soditech.IntelPrev.Mobile.Views;

public partial class DevView : IXamarinView
{
	public DevView()
	{
		InitializeComponent();
	}
	private async void OnBackButtonClicked(object sender, EventArgs e)
	{
		await Navigation.PopAsync(); // Navigates back to the previous page
	}
}