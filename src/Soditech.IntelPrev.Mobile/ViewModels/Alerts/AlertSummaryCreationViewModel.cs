using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Sensor6ty.Results;
using Soditech.IntelPrev.Mobile.Core.Dependency;
using Soditech.IntelPrev.Mobile.ViewModels.Base;
using Soditech.IntelPrev.Prevensions.Shared.Buildings;
using Soditech.IntelPrev.Prevensions.Shared.Floors;
using Soditech.IntelPrev.Proxy;
using Soditech.IntelPrev.Reports.Shared;
using Soditech.IntelPrev.Reports.Shared.Alerts;

namespace Soditech.IntelPrev.Mobile.ViewModels.Alerts;

public partial class AlertSummaryCreationViewModel : MauiViewModel, IQueryAttributable
{
	private readonly IProxyService _proxyClientService = DependencyResolver.GetRequiredService<IProxyService>();

	private CreateAlertCommand _createAlertCommand = default!;
	private BuildingResult _building = default!;
	private FloorResult? _floor;

	[ObservableProperty]
	private bool _isBusy;

	public string AlertTypeName => _createAlertCommand?.Type switch
	{
		"Fire" => "Alerte incendie",
		"Danger" => "Alerte danger",
		"Wounded" => "Alerte blessé",
		"Evacuation" => "Alerte évacuation",
		_ => "Type inconnu"
	};

	public string BuildingName => _building?.Name ?? "Non spécifié";

	public bool HasFloor => _floor != null;

	public int? FloorNumber => _floor?.Number;

	public string LocationDetails =>
		$"Lat: {_createAlertCommand?.Latitude:F6}\nLong: {_createAlertCommand?.Longitude:F6}";

	public ICommand CreateAlertCommand => new AsyncRelayCommand(CreateAlertAsync);
	public ICommand ReturnCommand => new AsyncRelayCommand(ReturnAsync);

	private async Task CreateAlertAsync()
	{
		try
		{
			IsBusy = true;

			// Update alert with final details
			_createAlertCommand.BuildingId = _building.Id;
			_createAlertCommand.FloorId = _floor?.Id;

			var alertResult = await _proxyClientService.PostAsync<Result>(
				ReportRoutes.Alerts.Create,
				_createAlertCommand);

			if (alertResult.IsSuccess)
			{
				await Shell.Current.GoToAsync(
					new ShellNavigationState(AppRoutes.AlertCreatedPage),
					new ShellNavigationQueryParameters
					{
						{ "Alert", _createAlertCommand },
						{ "Building", _building },
						{ "Floor", _floor }
					});
			}
			else
			{
				await Shell.Current.DisplayAlert(
					"Erreur",
					"Impossible de créer l'alerte. Veuillez réessayer.",
					"OK");
			}
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlert(
				"Erreur",
				"Une erreur s'est produite. Veuillez réessayer.",
				"OK");
		}
		finally
		{
			IsBusy = false;
		}
	}

	private async Task ReturnAsync()
	{
		await Shell.Current.GoToAsync(new ShellNavigationState(AppRoutes.MainViewPage));
	}

	void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
	{
		if (query.TryGetValue("Alert", out var alert))
		{
			_createAlertCommand = (CreateAlertCommand)alert;
		}

		if (query.TryGetValue("Building", out var building))
		{
			_building = (BuildingResult)building;
		}

		if (query.TryGetValue("Floor", out var floor))
		{
			_floor = (FloorResult)floor;
		}

		OnPropertyChanged(nameof(AlertTypeName));
		OnPropertyChanged(nameof(BuildingName));
		OnPropertyChanged(nameof(HasFloor));
		OnPropertyChanged(nameof(FloorNumber));
		OnPropertyChanged(nameof(LocationDetails));
	}
}