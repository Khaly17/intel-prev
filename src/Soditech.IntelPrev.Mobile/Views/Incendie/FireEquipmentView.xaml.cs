using Soditech.IntelPrev.Mobile.ViewModels.Incendie;
using System.Collections.ObjectModel;

namespace Soditech.IntelPrev.Mobile.Views.Incendie
{
    public partial class FireEquipmentView : ContentPage
    {
        public FireEquipmentView()
        {
            InitializeComponent();
            BindingContext = new FireEquipmentViewModel();
        }
    }
}
