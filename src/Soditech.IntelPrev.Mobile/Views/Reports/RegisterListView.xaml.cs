using Microsoft.Maui.Controls;
using Soditech.IntelPrev.Mobile.ViewModels.Reports;

namespace Soditech.IntelPrev.Mobile.Views.Reports;

public partial class RegisterListView : ContentPage
{
	public RegisterListView()
	{
		InitializeComponent();
        BindingContext = new RegisterListViewModel();
    }
}
