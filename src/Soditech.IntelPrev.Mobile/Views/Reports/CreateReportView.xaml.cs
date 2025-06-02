using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soditech.IntelPrev.Mobile.ViewModels.Reports;

namespace Soditech.IntelPrev.Mobile.Views.Reports;

public partial class CreateReportView : ContentPage
{
	public CreateReportView()
	{
		InitializeComponent();
	}

	protected override bool OnBackButtonPressed()
	{
		//go to previous page
		if (BindingContext is CreateReportViewModel viewModel)
		{
			viewModel.GoToPreviousPage();
			return true; // Prevent the default back button behavior
		}
		return base.OnBackButtonPressed(); // Allow the default behavior if BindingContext is not set
	}


}