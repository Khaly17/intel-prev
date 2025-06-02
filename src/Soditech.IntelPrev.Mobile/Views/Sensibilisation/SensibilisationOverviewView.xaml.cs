using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;
using Soditech.IntelPrev.Mobile.Models;
using Soditech.IntelPrev.Mobile.ViewModels.Sensibilisation;

namespace Soditech.IntelPrev.Mobile.Views.Sensibilisation
{
    public partial class SensibilisationOverviewView : ContentPage
    {
        public SensibilisationOverviewView()
        {
            InitializeComponent();
            BindingContext = new SensibilisationViewModel(
	            sensibilisationDescription: "Initial Description",
	            riskCategories: new ObservableCollection<RiskCategory>(),
	            preventionTips: new ObservableCollection<PreventionTip>()
            );
        }
    }
}

