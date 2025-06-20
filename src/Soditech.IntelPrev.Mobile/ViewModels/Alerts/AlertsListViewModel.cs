using System;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using Controls.UserDialogs.Maui;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Devices.Sensors;
using Soditech.IntelPrev.Mobile.ViewModels.Base;
using Soditech.IntelPrev.Prevensions.Shared.Buildings;
using Soditech.IntelPrev.Prevensions.Shared.Floors;
using Soditech.IntelPrev.Reports.Shared.Alerts;
using Soditech.IntelPrev.Reports.Shared.Enums;

namespace Soditech.IntelPrev.Mobile.ViewModels.Alerts;

public class AlertsListViewModel : MauiViewModel
{
	private Location? _location = default!;
	private CancellationTokenSource? _cancelTokenSource;
	private bool _isCheckingLocation;
	private readonly bool _isCheckLocationFailled = true;
	private BuildingResult _building { get; set; } = default!;
	private FloorResult _floor { get; set; } = default!;
	private CreateAlertCommand _alert = default!;
    private AlertTypeItem _selectedAlertType;

	public ICommand PageAppearingCommand => new AsyncRelayCommand(async () => await InitializeAsync());
	public ICommand AlertCommand => new AsyncRelayCommand<AlertType>(AlertAsync);
    public ICommand SelectionChangedCommand => new AsyncRelayCommand<AlertTypeItem>(HandleSelection);
	
	public ObservableCollection<AlertTypeItem> AlertTypes { get; } = new();

    public AlertTypeItem SelectedAlertType
    {
        get => _selectedAlertType;
        set => SetProperty(ref _selectedAlertType, value);
    }

    private async Task HandleSelection(AlertTypeItem item)
    {
        if (item != null)
        {
            await AlertAsync(item.Type);
            
            // Reset selection after a short delay to allow reselection
            await Task.Delay(300);
            SelectedAlertType = null;
        }
    }

	/// <inheritdoc />
	public override async Task InitializeAsync()
	{
		IsBusy = true;
		await GetCurrentLocationAsync();
        
        // Initialize the alert types if not already populated
        if (AlertTypes.Count == 0)
        {
            PopulateAlertTypes();
        }
        
		IsBusy = false;
	}

    private void PopulateAlertTypes()
    {
        AlertTypes.Clear();
        AlertTypes.Add(new AlertTypeItem
        {
            Type = AlertType.Fire,
            Title = "Alerte incendie",
            Description = "En cas d'incendie, appuyer sur le bouton",
            Icon = "alert_fire.png"
        });
        
        AlertTypes.Add(new AlertTypeItem
        {
            Type = AlertType.Danger,
            Title = "Alerte danger",
            Description = "En cas de danger grave, accident, incident ou attentat, appuyer sur le bouton",
            Icon = "alert_danger.png"
        });
        
        AlertTypes.Add(new AlertTypeItem
        {
            Type = AlertType.Wounded,
            Title = "Alerte blessé",
            Description = "En cas de blessé immédiat, appuyer sur le bouton",
            Icon = "alert_injury.png"
        });
        
        AlertTypes.Add(new AlertTypeItem
        {
            Type = AlertType.Evacuation,
            Title = "Alerte évacuation",
            Description = "En cas d'évacuation urgente, appuyer sur le bouton",
            Icon = "alert_evacuation.png"
        });
    }

	private async Task GetCurrentLocationAsync()
	{
		try
		{
			_isCheckingLocation = true;

			var status = await AlertsListViewModel.CheckAndRequestLocationPermission();

			if (status == PermissionStatus.Granted)
			{
				var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(5));

				_cancelTokenSource = new CancellationTokenSource();

				_location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);

				if (_location != null)
				{
					_alert = new CreateAlertCommand
					{
						Longitude = _location.Longitude,
						Latitude = _location.Latitude,
						Altitude = _location.Altitude,
					};
				}
			}
		}
		// Catch one of the following exceptions:
		//   FeatureNotSupportedException
		//   FeatureNotEnabledException
		//   PermissionException
		catch (ArgumentException)
		{
			// Unable to get location
		}
		finally
		{
			_isCheckingLocation = false;
		}
	}

	private static async Task<PermissionStatus> CheckAndRequestLocationPermission()
	{
		var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

		if (status == PermissionStatus.Granted) return status;

		if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
			// Prompt the user to turn on in settings
			// On iOS once a permission has been denied it may not be requested again from the application
			return status;

		if (Permissions.ShouldShowRationale<Permissions.LocationWhenInUse>())
		{
			// Prompt the user with additional information as to why the permission is needed
		}

		status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

		return status;
	}

	public void CancelRequest()
	{
		if (_isCheckingLocation && _cancelTokenSource is { IsCancellationRequested: false })
			_cancelTokenSource.Cancel();
	}

	private async Task AlertAsync(AlertType type)
	{
        switch (type)
        {
			case AlertType.Fire:
                _alert.Title = "Fire Alert";
                _alert.Description = "Fire Alert";
                break;
            
            case AlertType.Danger:
                _alert.Title = "Danger Alert";
                _alert.Description = "Danger Alert";
                break;
            
            case AlertType.Evacuation:
                _alert.Title = "Evacuation Alert";
                _alert.Description = "Evacuation Alert";
                break;
            
            case AlertType.Wounded:
                _alert.Title = "Wounded Alert";
                _alert.Description = "Wounded Alert";
                break;
            default:
                _alert.Title = "Danger Alert";
                _alert.Description = "Danger Alert";
                type = AlertType.Danger;
				break;
        }

		_alert.Type = type.ToString();

		if (_currentUser?.BuildingId == null)
		{
			await GoToBuildingSelectionAsync();
			return;
		}

		var buildingResponse = await UserDialogs.Instance.ConfirmAsync(
			$"Est-ce dans le batiment `{_currentUser.BuildingName}` que se produit l'incident ?",
			_currentUser.BuildingName, "Oui", "Non, je choisis le bat");

		if (!buildingResponse)
		{
			await GoToBuildingSelectionAsync();
			return;
		}

		_alert.BuildingId = _currentUser.BuildingId.Value;

		if (_currentUser.FloorId == null)
		{
			await GoToFloorSelectionAsync();
			return;
		}

		var floorResponse = await UserDialogs.Instance.ConfirmAsync(
			$"Est-ce dans l'etage `{_currentUser.FloorNumber}` que se produit l'incident ?",
			_currentUser.BuildingName, "Oui", "Non, je choisis le bat");

		if (floorResponse)
		{
			_alert.FloorId = _currentUser.FloorId;
			await GoToSummaryAsync();
		}
		else
		{
			await GoToFloorSelectionAsync();
		}
	}

	private async Task GoToBuildingSelectionAsync()
	{
		await Shell.Current.GoToAsync(new ShellNavigationState(AppRoutes.AlertBuildingSelectionPage),
			new ShellNavigationQueryParameters
			{
				{ "Alert", _alert }
			});
	}

	private async Task GoToFloorSelectionAsync()
	{
		await Shell.Current.GoToAsync(new ShellNavigationState(AppRoutes.AlertFloorSelectionPage),
			new ShellNavigationQueryParameters
			{
				{ "Alert", _alert },
				{ "Building", _building }
			});
	}

	private async Task GoToSummaryAsync()
	{
		await Shell.Current.GoToAsync(new ShellNavigationState(AppRoutes.AlertSummaryPage),
			new ShellNavigationQueryParameters
			{
				{ "Alert", _alert },
				{ "Building", _building },
				{ "Floor", _floor }
			});
	}
}

// Model class for alert types
public class AlertTypeItem
{
    public AlertType Type { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Icon { get; set; }
}