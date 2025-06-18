using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Soditech.IntelPrev.Mobile.Core.Dependency;
using Soditech.IntelPrev.Mobile.ViewModels.Base;
using Soditech.IntelPrev.Prevensions.Shared;
using Soditech.IntelPrev.Prevensions.Shared.Buildings;
using Soditech.IntelPrev.Prevensions.Shared.Floors;
using Soditech.IntelPrev.Proxy;
using Soditech.IntelPrev.Reports.Shared.Alerts;

namespace Soditech.IntelPrev.Mobile.ViewModels.Alerts;

public partial class BuildingSelectionViewModel : MauiViewModel, IQueryAttributable
{
    private string _title = "Sélection du bâtiment";
    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }
    //Main message

    private string _mainMesssage = "Veuillez selectionner le batiment où l'incident s'est produit";

    public string MainMessage
    {
	    get => _mainMesssage;
	    set => SetProperty(ref _mainMesssage, value);
    }
    private IEnumerable<BuildingResult> _buildingList = default!;
    public IEnumerable<BuildingResult> BuildingList
    {
        get => _buildingList;
        set => SetProperty(ref _buildingList, value);
    }

    private IEnumerable<FloorResult> _floorList = [];
    public IEnumerable<FloorResult> FloorList
    {
        get => _floorList;
        set => SetProperty(ref _floorList, value);

    }
    private CreateAlertCommand _createAlertCommand = default!;

    private BuildingResult _building = default!;
    public BuildingResult Building
    {
        get => _building;
        set
        {
            SetProperty( ref _building, value);
            _ = GoToFloorSelectionPageAsync();
        }
    }

    private FloorResult _floor = default!;
    public FloorResult Floor
    {
        get => _floor;
        set
        {
            SetProperty(ref _floor, value);
            _ = GoToAlertCreationSummaryPageAsync();
        }
    }

    private bool _isFloorSelection ;
    public bool IsFloorSelection
    {
        get => _isFloorSelection;
        set => SetProperty(ref _isFloorSelection, value);
    }

    private string _pageInstruction = "Sélectionnez le bâtiment concerné";
    public string PageInstruction
    {
        get => _pageInstruction;
        set => SetProperty(ref _pageInstruction, value);
    }



    private readonly IProxyService _proxyClientService = DependencyResolver.GetRequiredService<IProxyService>();

    public ICommand PageAppearingCommand => new AsyncRelayCommand(async () => await InitializeAsync());

    public ICommand ReturnCommand => new AsyncRelayCommand(ReturnAsync);
    public ICommand BackCommand => new AsyncRelayCommand(ReturnAsync);

    /// <inheritdoc />
    public override async Task InitializeAsync()
    {
        IsBusy = true;

        // when we click on return back button
        if (BuildingList != null)
        {
            IsBusy = false;
            return;
        }

        await GetBuildingsAsync();
        IsBusy = false;
    }


    //get all buildings
    private async Task GetBuildingsAsync()
    {
        var buildingsResult =
            await _proxyClientService.GetAsync<IEnumerable<BuildingResult>>(PreventionRoutes.Buildings.GetAll);
        if (buildingsResult.IsSuccess)
        {
            BuildingList = buildingsResult.Value;
        }
    }

    private async Task GoToFloorSelectionPageAsync()
    {
        IsBusy = true;
        _createAlertCommand.Description += $" au bâtiment {Building.Name}";

        if (_building.Floors is { Count: > 0 })
        {
            Title = "Sélection de l'étage";
            PageInstruction = "Sélectionnez l'étage concerné";
            IsFloorSelection = true;
            FloorList = _building.Floors;
        }
        else
        {
            await Shell.Current.GoToAsync(new ShellNavigationState(AppRoutes.AlertSummaryPage),
                new ShellNavigationQueryParameters
                {
                { "Alert", _createAlertCommand },
                { "Building", _building }
                });
        }

        IsBusy = false;
    }

    private async Task GoToAlertCreationSummaryPageAsync()
    {
        if (Floor != null)
        {
            // if Floor.Number == 1, return "1er étage" else return "2ème étage"
            if (Floor.Number == 1)
            {
                _createAlertCommand.Description += $", {Floor.Number}ier étage.";
            }
            else
            {
                _createAlertCommand.Description += $", {Floor.Number}ème étage.";
            }
        }

        await Shell.Current.GoToAsync(new ShellNavigationState(AppRoutes.AlertSummaryPage),
            new ShellNavigationQueryParameters
            {
                { "Alert", _createAlertCommand },
                { "Building", _building },
                { "Floor", _floor }
            });
    }

    private async Task ReturnAsync()
    {
        if (IsFloorSelection)
        {
            IsFloorSelection = false;
            Title = "Sélection du bâtiment";
            PageInstruction = "Sélectionnez le bâtiment concerné";
        }
        else
        {
            await Shell.Current.GoToAsync("..");
        }
    }

    void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("Alert", out var alert))
        {
            _createAlertCommand = (CreateAlertCommand)alert;
        }
        else
        {
            //TODO: handle error
        }
    }


}