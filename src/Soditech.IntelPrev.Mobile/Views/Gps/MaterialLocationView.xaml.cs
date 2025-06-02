using Microsoft.Maui.Controls.Maps;
using Soditech.IntelPrev.Mobile.ViewModels.Gps;

namespace Soditech.IntelPrev.Mobile.Views.Gps;

public partial class MaterialLocationView : ContentPage
{
    private readonly MaterialLocationViewModel _viewModel;

    public MaterialLocationView()
    {
        InitializeComponent();
        _viewModel = (MaterialLocationViewModel)BindingContext;

        // Set up map reference
        if (LocationMap != null)
        {
            _viewModel.SetMap(LocationMap);
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Initialize map when view appears
        if (LocationMap != null && _viewModel != null)
        {
            _viewModel.SetMap(LocationMap);
        }
    }
}
