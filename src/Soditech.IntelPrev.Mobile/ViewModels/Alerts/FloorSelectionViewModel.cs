using System.Collections.Generic;
using System.Linq;
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

public partial class FloorSelectionViewModel : MauiViewModel, IQueryAttributable
{
	private IEnumerable<FloorResult> _floorList = [];
	public IEnumerable<FloorResult> FloorList
	{
		get => _floorList;
		set => SetProperty(ref _floorList, value);

	}

	private CreateAlertCommand _createAlertCommand = default!;

	private BuildingResult _building = default!;

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

	private readonly IProxyService _proxyClientService = DependencyResolver.GetRequiredService<IProxyService>();

	public ICommand PageAppearingCommand => new AsyncRelayCommand(async () => await InitializeAsync());

	public ICommand ReturnCommand => new AsyncRelayCommand(ReturnAsync);

	/// <inheritdoc />
	public override async Task InitializeAsync()
	{
		if (!FloorList.Any())
		{
			IsBusy = true;
				await GetFloorsAsync();
			IsBusy = false;
		}
	}


	//get all floors for a building
	private async Task GetFloorsAsync()
	{
		var floorsResult =
			await _proxyClientService.GetAsync<IEnumerable<FloorResult>>(PreventionRoutes.Buildings.Floors.Replace("{id:guid}", _building.Id.ToString()));
		if (floorsResult.IsSuccess)
		{
			FloorList = floorsResult.Value;
		}

		if (!FloorList.Any())
		{
			await GoToAlertCreationSummaryPageAsync();
		}
	}

	private async Task GoToAlertCreationSummaryPageAsync()
    {
		if (Floor != null)
		{
            _createAlertCommand.Description += $", {Floor.Number} ier étage.";
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
		await Shell.Current.GoToAsync("..", new ShellNavigationQueryParameters
		{
			{ "Alert", _createAlertCommand }
		});
	}

	public void ApplyQueryAttributes(IDictionary<string, object> query)
	{
		if (query.TryGetValue("Alert", out var alert))
		{
			_createAlertCommand = (CreateAlertCommand)alert;
		}
		else
		{
			//TODO: handle error
		}

		if (query.TryGetValue("Building", out var building))
		{
			_building = (BuildingResult)building;
			FloorList = _building.Floors.Count > 0 ? _building.Floors : (IEnumerable<FloorResult>)([]);
        }
		else
		{
			//TODO: handle error
		}
	}

}