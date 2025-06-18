using Microsoft.Maui.Controls;
using Soditech.IntelPrev.Mobile.ViewModels.Incendie;

namespace Soditech.IntelPrev.Mobile.Views.Incendie;

public partial class FireEquipmentView : ContentPage
{
    public FireEquipmentView()
    {
        InitializeComponent();
        BindingContext = new FireEquipmentViewModel();
    }
}