using Microsoft.Maui.Controls;
using Soditech.IntelPrev.Mobile.ViewModels.Sensibilisation;

namespace Soditech.IntelPrev.Mobile.Views.Sensibilisation;
public partial class RiskPreventionTipsView : ContentPage
{
    public RiskPreventionTipsView()
    {
        InitializeComponent();
        BindingContext = new RiskPreventionViewModel();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is RiskPreventionViewModel viewModel)
        {
            viewModel.PageAppearingCommand.Execute(null);
        }
    }
}

