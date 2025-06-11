using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using Soditech.IntelPrev.Preventions.Shared;
using Soditech.IntelPrev.Preventions.Shared.Materials;
using Soditech.IntelPrev.Mobile.Core.Dependency;
using Soditech.IntelPrev.Mobile.ViewModels.Base;
using Soditech.IntelPrev.Prevensions.Shared.Enums;
using Soditech.IntelPrev.Proxy;
using IMap = Microsoft.Maui.Maps.IMap;
using Soditech.IntelPrev.Prevensions.Shared.GeoLocations;

namespace Soditech.IntelPrev.Mobile.ViewModels.Gps
{
    public partial class MaterialLocationViewModel : MauiViewModel, IQueryAttributable
    {
        private readonly IProxyService _proxyClientService = DependencyResolver.GetRequiredService<IProxyService>();
        private CancellationTokenSource? _cancelTokenSource;
        private bool _isCheckingLocation;
        private Location? _currentLocation;
        private MaterialType _materialType;
        private IMap? _map;

        [ObservableProperty]
        private string _pageTitle = "Localisation";


        private IEnumerable<Pin> _mapPins = [];
        public IEnumerable<Pin> MapPins
        {
            get => _mapPins;
            set => SetProperty(ref _mapPins, value);
        }

        public ICommand PageAppearingCommand => new AsyncRelayCommand(async () => await InitializeAsync());

        // Add these fields to fix the errors
        private Point _mapCenter;
        public Point MapCenter
        {
            get => _mapCenter;
            set => SetProperty(ref _mapCenter, value);
        }

        private ObservableCollection<CustomMapMarker> _markers = new();
        public ObservableCollection<CustomMapMarker> Markers
        {
            get => _markers;
            set => SetProperty(ref _markers, value);
        }

        public void SetMap(IMap map)
        {
            _map = map;
        }

        public override async Task InitializeAsync()
        {
            try
            {
                IsBusy = true;
                await GetCurrentLocationAsync();
                await LoadMaterialLocationsAsync();
            }
            finally
            {
                IsBusy = false;
            }
        }

        
        private async Task GetCurrentLocationAsync()
        {
            try
            {
                _isCheckingLocation = true;
                var status = await CheckAndRequestLocationPermission();

                if (status == PermissionStatus.Granted)
                {
                    var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(5));
                    _cancelTokenSource = new CancellationTokenSource();
                    _currentLocation = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);

                    if (_currentLocation != null)
                    {
                        var location = new Location(_currentLocation.Latitude, _currentLocation.Longitude);
                        var mapSpan _ = new MapSpan(location, 0.01, 0.01);
                        _map?.MoveToRegion(mapSpan);
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

        private async Task LoadMaterialLocationsAsync()
        {
            try
            {
                var route = _materialType switch
                {
                    MaterialType.Extinguisher => PreventionRoutes.GeoLocations.GetAllByType.Replace("{type}", GeoLocationType.Extinguisher.ToString()),
                    MaterialType.Dae => PreventionRoutes.GeoLocations.GetAllByType.Replace("{type}", GeoLocationType.Dae.ToString()),
                    MaterialType.AssemblyPoint => PreventionRoutes.GeoLocations.GetAllByType.Replace("{type}", GeoLocationType.GatheringPoint.ToString()),
                    _ => throw new ArgumentOutOfRangeException()
                };

                var result = await _proxyClientService.GetAsync<IEnumerable<GeoLocationResult>>(route);

                if (result.IsSuccess)
                {
                    var mapPins = (from material in result.Value
                        select new Pin
                        {
                            Label = _materialType == MaterialType.AssemblyPoint 
                                ? $"{material.GatheringPointName}\n({material.BuildingName} - {material.FloorNumber})"
                                : $"{material.EquipmentName}\n({material.BuildingName} - {material.FloorNumber})",
                            Location = new Location(material.Latitude, material.Longitude),
                            Type = PinType.Place,
                            Address = $"{material.BuildingName} - {material.FloorNumber}",
                        }).ToList();

                    foreach (var pin in mapPins)
                    {
                        _map?.Pins.Add(pin);
                    }

                    MapPins = mapPins;
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erreur", "Impossible de charger les emplacements", "OK");
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

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("MaterialType", out var type))
            {
                _materialType = (MaterialType)type;
                PageTitle = _materialType switch
                {
                    MaterialType.Extinguisher => "Extincteurs",
                    MaterialType.Dae => "Défibrillateurs",
                    MaterialType.AssemblyPoint => "Points de rassemblement",
                    _ => "Équipements"
                };
            }
        }
    }

    public class CustomMapMarker
    {
        public Point Location { get; set; }
        public string IconSource { get; set; } = string.Empty;
        public bool IsCurrentLocation { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
