using Microsoft.Maui.Controls;
using Soditech.IntelPrev.Mobile.ViewModels.Incendie;

namespace Soditech.IntelPrev.Mobile.Views.Incendie;

public partial class FireInstructionsView : ContentPage
{
    public FireInstructionsView()
    {
        InitializeComponent();
        BindingContext = new FireInstructionsViewModel();
    }
}