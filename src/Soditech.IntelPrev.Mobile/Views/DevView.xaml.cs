using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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