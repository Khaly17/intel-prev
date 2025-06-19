using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices.Sensors;
using Soditech.IntelPrev.Mobile.Core.Dependency;
using Soditech.IntelPrev.Mobile.Models.Materials;
using Soditech.IntelPrev.Mobile.ViewModels.Base;
using Soditech.IntelPrev.Prevensions.Shared;
using Soditech.IntelPrev.Prevensions.Shared.Equipments;
using Soditech.IntelPrev.Prevensions.Shared.GeoLocations;
using Soditech.IntelPrev.Proxy;

namespace Soditech.IntelPrev.Mobile.ViewModels.Gps;

public partial class EquipmentLocationTrackerViewModel : MauiViewModel, IQueryAttributable
{
    private readonly IProxyService _proxyClientService = DependencyResolver.GetRequiredService<IProxyService>();
    private readonly ILogger<EquipmentLocationTrackerViewModel> _logger = DependencyResolver.GetRequiredService<ILogger<EquipmentLocationTrackerViewModel>>();

    public ICommand PageAppearingCommand => new AsyncRelayCommand(async () => await InitializeAsync());

    private IEnumerable<EquipmentResult> _equipments = null!;
    public IEnumerable<EquipmentResult> Equipments
    {
        get => _equipments;
        set => SetProperty(field: ref _equipments, value);
    }

    private EquipmentResult _selectedEquipment = null!;

    public EquipmentResult SelectedEquipment
    {
        get => _selectedEquipment;
        set => SetProperty(ref _selectedEquipment, value);
    }


    private string _currentLocationDisplay = string.Empty;

    public string CurrentLocationDisplay
    {
        get => _currentLocationDisplay;
        set => SetProperty(ref _currentLocationDisplay, value);
    }

    private Location _currentLocation;
    private bool _isCheckingLocation;

    private MaterialType _materialType;
    public MaterialType MaterialType
    {
        get => _materialType;
        set => SetProperty(ref _materialType, value);
    }

    private string _equipmentTypeDisplay = string.Empty;

    public string EquipmentTypeDisplay
    {
        get => _equipmentTypeDisplay;
        set => SetProperty(field: ref _equipmentTypeDisplay, value);
    }




    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        EquipmentTypeDisplay = MaterialType switch
        {
            MaterialType.Extinguisher => "Extincteur",
            MaterialType.Dae => "Défibrillateur",
            MaterialType.AssemblyPoint => "Point de rassemblement",
            _ => "Non défini"
        };
        await LoadEquipmentsByType();
    }

    private async Task LoadEquipmentsByType()
    {
        try
        {
            IsBusy = true;

            var result = await _proxyClientService.GetAsync<List<EquipmentResult>>(
                $"{PreventionRoutes.Equipments.GetEquipmentsByType}".Replace("{type}", _materialType.ToString()));

            if (result is { IsSuccess: true, Value: not null })
            {
                Equipments = new List<EquipmentResult>(result.Value);
                if (Equipments.Any())
                {
                    SelectedEquipment = Equipments.FirstOrDefault();
                }
            }
        }
        catch (Exception ex)
        {
            await MauiViewModel.HandleExceptionAsync(ex);
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task GetCurrentLocation()
    {
        try
        {
            _isCheckingLocation = true;
            var status = await EquipmentLocationTrackerViewModel.CheckAndRequestLocationPermission();

            if (status == PermissionStatus.Granted)
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(5));
                var cancelTokenSource = new CancellationTokenSource();
                _currentLocation = await Geolocation.Default.GetLocationAsync(request, cancelTokenSource.Token);

                if (_currentLocation != null)
                {
                    CurrentLocationDisplay = $"Lat: {_currentLocation.Latitude:F6}, Long: {_currentLocation.Longitude:F6}";
                }
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Erreur", "Impossible d'obtenir votre position", "OK");
            _logger.LogError(ex, "Unable to get user location");
        }
        finally
        {
            _isCheckingLocation = false;
        }
    }

    private static async Task<PermissionStatus> CheckAndRequestLocationPermission()
    {
        var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

        if (status == PermissionStatus.Granted)
            return status;

        if (Permissions.ShouldShowRationale<Permissions.LocationWhenInUse>())
        {
            await Shell.Current.DisplayAlert("Permission nécessaire",
                "L'accès à la localisation est nécessaire pour enregistrer la position de l'équipement", "OK");
        }

        status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
        return status;
    }

    [RelayCommand]
    private async Task SaveLocation()
    {
        if (_selectedEquipment == null)
        {
            await Shell.Current.DisplayAlert("Erreur", "Veuillez sélectionner un équipement", "OK");
            return;
        }

        if (_currentLocation == null)
        {
            await Shell.Current.DisplayAlert("Erreur", "Veuillez définir une position", "OK");
            return;
        }

        try
        {
            IsBusy = true;

            // var type  = _selectedEquipment.Type.ToGeoLocationType();
            var result = await _proxyClientService.PostAsync(PreventionRoutes.Equipments.UpdateGeoLocation, new UpdateGeoLocationCommand
            {
                EquipmentId = _selectedEquipment.Id,
                Latitude = _currentLocation.Latitude,
                Longitude = _currentLocation.Longitude,
                Altitude = _currentLocation.Altitude,
                BuildingId = _selectedEquipment.BuildingId,
                FloorId = _selectedEquipment.FloorId,
            });

            if (result.IsSuccess)
            {
                await Shell.Current.DisplayAlert("Succès", "Position de l'équipement mise à jour avec succès", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await Shell.Current.DisplayAlert("Erreur", "Erreur lors de la mise à jour de la position", "OK");
            }
        }
        catch (Exception ex)
        {
            await MauiViewModel.HandleExceptionAsync(ex);
        }
        finally
        {
            IsBusy = false;
        }
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("MaterialType", out var type))
        {
            MaterialType = (MaterialType)Enum.Parse(typeof(MaterialType), type.ToString());
        }

    }
}
