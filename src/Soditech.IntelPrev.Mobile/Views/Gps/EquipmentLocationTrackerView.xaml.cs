using Soditech.IntelPrev.Mobile.ViewModels.Gps;

namespace Soditech.IntelPrev.Mobile.Views.Gps;

public partial class EquipmentLocationTrackerView : ContentPage
{
    public EquipmentLocationTrackerView()
    {
        InitializeComponent();
        BindingContext = new EquipmentLocationTrackerViewModel();
    }
}
