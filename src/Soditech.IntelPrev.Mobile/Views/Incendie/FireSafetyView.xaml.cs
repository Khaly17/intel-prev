using Microsoft.Maui.Controls;
using Soditech.IntelPrev.Mobile.ViewModels.Incendie;

namespace Soditech.IntelPrev.Mobile.Views.Incendie;

public partial class FireSafetyView : ContentPage
{
    public FireSafetyView()
    {
        InitializeComponent();
        BindingContext = new FireSafetyViewModel(
            fireSafetyDescription: "Initial description",
            safetyPrinciples: "Initial safety principles",
            safetyCauses: "Initial causes");
    }
}