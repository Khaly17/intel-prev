using Soditech.IntelPrev.Mobile.ViewModels.Gps;

namespace Soditech.IntelPrev.Mobile.Views.Gps;

public partial class EquipmentTypeSelectionView : ContentPage
{
    public EquipmentTypeSelectionView()
    {
        InitializeComponent();
        BindingContext = new EquipmentTypeSelectionViewModel();
    }
}
