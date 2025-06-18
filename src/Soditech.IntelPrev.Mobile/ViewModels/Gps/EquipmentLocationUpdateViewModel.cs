using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices.Sensors;
using Soditech.IntelPrev.Mobile.Models.Materials;
using Soditech.IntelPrev.Mobile.ViewModels.Base;

namespace Soditech.IntelPrev.Mobile.ViewModels.Gps;

[QueryProperty(nameof(MaterialType), "MaterialType")]
public partial class EquipmentLocationUpdateViewModel : MauiViewModel
{
    [ObservableProperty]
    private MaterialType materialType;

    [ObservableProperty]
    private string equipmentName;

    [ObservableProperty]
    private string description;

    [ObservableProperty]
    private string currentLocationDisplay = "Position non définie";

    private Location currentLocation;

    public string EquipmentTypeDisplay => MaterialType switch
    {
        MaterialType.Extinguisher => "Extincteur",
        MaterialType.Dae => "Défibrillateur",
        MaterialType.AssemblyPoint => "Point de rassemblement",
        _ => "Non défini"
    };

    [RelayCommand]
    private async Task GetCurrentLocation()
    {
        try
        {
            var location = await Geolocation.GetLocationAsync(new GeolocationRequest
            {
                DesiredAccuracy = GeolocationAccuracy.Best,
                Timeout = TimeSpan.FromSeconds(5)
            });

            if (location != null)
            {
                currentLocation = location;
                CurrentLocationDisplay = $"Lat: {location.Latitude:F6}, Long: {location.Longitude:F6}";
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Erreur", "Impossible d'obtenir la position actuelle: " + ex.Message, "OK");
        }
    }

    [RelayCommand]
    private async Task SaveLocation()
    {
        if (string.IsNullOrWhiteSpace(EquipmentName))
        {
            await Shell.Current.DisplayAlert("Erreur", "Veuillez saisir un nom pour l'équipement", "OK");
            return;
        }

        if (currentLocation == null)
        {
            await Shell.Current.DisplayAlert("Erreur", "Veuillez définir une position", "OK");
            return;
        }

        try
        {
            // TODO: Save to your database/service
            await Shell.Current.DisplayAlert("Succès", "Position enregistrée avec succès", "OK");
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Erreur", "Erreur lors de l'enregistrement: " + ex.Message, "OK");
        }
    }
}
