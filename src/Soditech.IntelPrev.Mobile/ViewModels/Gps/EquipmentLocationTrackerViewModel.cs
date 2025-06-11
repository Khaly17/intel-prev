using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Soditech.IntelPrev.Mobile.Core.Dependency;
using Soditech.IntelPrev.Mobile.ViewModels.Base;
using Soditech.IntelPrev.Prevensions.Shared.Enums;
using Soditech.IntelPrev.Prevensions.Shared.GeoLocations;
using Soditech.IntelPrev.Preventions.Shared;
using Soditech.IntelPrev.Preventions.Shared.Equipments;
using Soditech.IntelPrev.Preventions.Shared.Materials;
using Soditech.IntelPrev.Proxy;

namespace Soditech.IntelPrev.Mobile.ViewModels.Gps;

public partial class EquipmentLocationTrackerViewModel : MauiViewModel, IQueryAttributable
{
    private readonly IProxyService _proxyClientService = DependencyResolver.GetRequiredService<IProxyService>();

    public ICommand PageAppearingCommand => new AsyncRelayCommand(async () => await InitializeAsync());

    public IEnumerable<EquipmentResult> _equipments;
    public IEnumerable<EquipmentResult> Equipments
    {
        get => _equipments;
        set
        {
            SetProperty(field: ref _equipments, value);
        }
    }

    public EquipmentResult _selectedEquipment;

    public EquipmentResult SelectedEquipment
    {
        get => _selectedEquipment;
        set => SetProperty(ref _selectedEquipment, value);
    }


    public string currentLocationDisplay;

    public string CurrentLocationDisplay
    {
        get => currentLocationDisplay;
        set => SetProperty(ref currentLocationDisplay, value);
    }

    private Location currentLocation;
    private bool isCheckingLocation;

    private MaterialType materialType;
    public MaterialType MaterialType
    {
        get => materialType;
        set => SetProperty(ref materialType, value);
    }

    private string _equipmentTypeDisplay = default;

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
            IntelPrev.Preventions.Shared.Materials.MaterialType.Extinguisher => "Extincteur",
            IntelPrev.Preventions.Shared.Materials.MaterialType.Dae => "Défibrillateur",
            IntelPrev.Preventions.Shared.Materials.MaterialType.AssemblyPoint => "Point de rassemblement",
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
                $"{PreventionRoutes.Equipments.GetEquipmentsByType}".Replace("{type}", materialType.ToString()));

            if (result.IsSuccess && result.Value != null)
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
            await HandleExceptionAsync(ex);
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
            isCheckingLocation = true;
            var status = await CheckAndRequestLocationPermission();

            if (status == PermissionStatus.Granted)
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(5));
                var cancelTokenSource = new CancellationTokenSource();
                currentLocation = await Geolocation.Default.GetLocationAsync(request, cancelTokenSource.Token);

                if (currentLocation != null)
                {
                    CurrentLocationDisplay = $"Lat: {currentLocation.Latitude:F6}, Long: {currentLocation.Longitude:F6}";
                }
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Erreur", "Impossible d'obtenir votre position", "OK");
        }
        finally
        {
            isCheckingLocation = false;
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

        if (currentLocation == null)
        {
            await Shell.Current.DisplayAlert("Erreur", "Veuillez définir une position", "OK");
            return;
        }

        try
        {
            IsBusy = true;

            var type _= _selectedEquipment.Type.ToGeoLocationType();
            var result = await _proxyClientService.PostAsync(PreventionRoutes.Equipments.UpdateGeoLocation, new UpdateGeoLocationCommand
            {
                EquipmentId = _selectedEquipment.Id,
                Latitude = currentLocation.Latitude,
                Longitude = currentLocation.Longitude,
                Altitude = currentLocation.Altitude,
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
            await HandleExceptionAsync(ex);
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
