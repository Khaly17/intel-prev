using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Soditech.IntelPrev.Mobile.Core.Dependency;
using Soditech.IntelPrev.Mobile.ViewModels.Base;
using Soditech.IntelPrev.Prevensions.Shared;
using Soditech.IntelPrev.Prevensions.Shared.Buildings;
using Soditech.IntelPrev.Proxy;

namespace Soditech.IntelPrev.Mobile.ViewModels.Preventions.Buildings;

public class PreventionBuildingsViewModel : MauiViewModel
{
    private bool _isRefreshing;
    public bool IsRefreshing
    {
        get => _isRefreshing;
        set => SetProperty(ref _isRefreshing, value);
    }
    
    private bool _isBusy;
    public bool IsBusy
    {
        get => _isBusy;
        set => SetProperty(ref _isBusy, value);
    }

    private IEnumerable<BuildingResult> _buildings = default!;
    public IEnumerable<BuildingResult> Buildings
    {
        get => _buildings;
        set => SetProperty(ref _buildings, value);
    }

    private BuildingResult _selectedBuilding;
    public BuildingResult SelectedBuilding
    {
        get => _selectedBuilding;
        set => SetProperty(ref _selectedBuilding, value);
    }

    private readonly IProxyService _proxyClientService = DependencyResolver.GetRequiredService<IProxyService>();

    public ICommand PageAppearingCommand => new AsyncRelayCommand(async () => await InitializeAsync());

    public ICommand BuildingSelectedCommand => new RelayCommand<BuildingResult>(building =>
    {
        if (building != null)
        {
            SelectedBuilding = building;
            // Handle building selection if needed
            // Example: Navigate to building details
            // await _navigationService.NavigateToAsync($"{nameof(BuildingDetailsView)}?id={building.Id}");
        }
    });

    public IAsyncRelayCommand RefreshCommand => new AsyncRelayCommand(async () => await InitializeAsync());

    public override async Task InitializeAsync()
    {
        try
        {
            IsBusy = true;
            var buildingsResult = await _proxyClientService.GetAsync<IEnumerable<BuildingResult>>(PreventionRoutes.Buildings.GetAll);
            if (buildingsResult.IsSuccess)
            {
                Buildings = buildingsResult.Value;
            }
            else
            {
                //TODO: handle error
            }
        }
        finally
        {
            IsBusy = false;
        }
    }
}
