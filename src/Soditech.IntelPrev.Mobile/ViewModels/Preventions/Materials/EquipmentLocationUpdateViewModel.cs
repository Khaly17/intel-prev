using Microsoft.Maui.Maps;
using Soditech.IntelPrev.Mobile.Core.Dependency;
using Soditech.IntelPrev.Mobile.ViewModels.Base;
using Soditech.IntelPrev.Prevensions.Shared.Enums;
using Soditech.IntelPrev.Prevensions.Shared.GeoLocations;
using Soditech.IntelPrev.Preventions.Shared;
using Soditech.IntelPrev.Preventions.Shared.Equipments;
using Soditech.IntelPrev.Proxy;

namespace Soditech.IntelPrev.Mobile.ViewModels.Preventions.Materials;

class EquipmentLocationUpdateViewModel : MauiViewModel, IQueryAttributable
{
    private readonly IProxyService _proxyClientService = DependencyResolver.GetRequiredService<IProxyService>();
    
    private bool _isCheckingLocation;
    private Location? _currentLocation;

    private EquipmentResult _equipment ;
    public EquipmentResult Equipment
    {
        get => _equipment;
        set => SetProperty(ref _equipment, value);
    }


    public override async Task InitializeAsync()
    {
        try
        {
            IsBusy = true;
        }
        finally
        {
            IsBusy = false;
        }
    }

    // use _proxyClientService to update the location of the equipment
    private async Task UpdateLocationAsync()
    {
        try
        {
            var type = Equipment.Type.ToGeoLocationType();
            var result = await _proxyClientService.PostAsync(PreventionRoutes.GeoLocations.Create, new AddGeoLocationCommand
            {
                EquipmentId = Equipment.Id,
                Type = type.ToString(),
                Latitude = _currentLocation?.Latitude ?? 0,
                Longitude = _currentLocation?.Longitude ?? 0,
                Altitude = _currentLocation?.Altitude,
                BuildingId = Equipment.BuildingId,
                FloorId = Equipment.FloorId,
            });

            if (result.IsSuccess)
            {
                await Shell.Current.DisplayAlert("Succès", "La position de l'équipement a été mise à jour", "OK");
            }
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(ex);
        }
    }



    /// <summary>
    /// Ask user to go to the location of the equipment and update the location
    /// </summary>
    /// <returns></returns>
    private async Task GetCurrentLocationAsync()
    {
        try
        {
            _isCheckingLocation = true;
            var status = await CheckAndRequestLocationPermission();

            if (status == PermissionStatus.Granted)
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(5));
                var cancelTokenSource = new CancellationTokenSource();
                _currentLocation = await Geolocation.Default.GetLocationAsync(request, cancelTokenSource.Token);

                if (_currentLocation != null)
                {
                    var location = new Location(_currentLocation.Latitude, _currentLocation.Longitude);
                    // var mapSpan  = new MapSpan(location, 0.01, 0.01);
                    //_map?.MoveToRegion(mapSpan);
                }
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Erreur", "Impossible d'obtenir votre position", "OK");
        }
        finally
        {
            _isCheckingLocation = false;
        }
    }

    private async Task<PermissionStatus> CheckAndRequestLocationPermission()
    {
        var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

        if (status == PermissionStatus.Granted)
            return status;

        if (Permissions.ShouldShowRationale<Permissions.LocationWhenInUse>())
        {
            await Shell.Current.DisplayAlert("Permission nécessaire",
                "L'accès à la localisation est nécessaire pour afficher votre position sur la carte", "OK");
        }

        status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
        return status;
    }



    /// <inheritdoc />
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("Equipment", out var equipment))
        {
            Equipment = (EquipmentResult)equipment;
        }
    }
}