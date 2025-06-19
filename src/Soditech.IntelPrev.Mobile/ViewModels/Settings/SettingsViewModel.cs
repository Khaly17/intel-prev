using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.ApplicationModel;
using Soditech.IntelPrev.Mobile.Core.Dependency;
using Soditech.IntelPrev.Mobile.Services.Settings;
using Soditech.IntelPrev.Mobile.ViewModels.Base;

namespace Soditech.IntelPrev.Mobile.ViewModels.Settings;

public class SettingsViewModel : MauiViewModel
{
	private readonly ISettingsManager _settingsManager;
	private readonly ILogger<SettingsViewModel> _logger = DependencyResolver.GetRequiredService<ILogger<SettingsViewModel>>();

	// Settings properties with direct binding to toggles
	private bool _isGeoLocationEnabled;
	public bool IsGeoLocationEnabled
	{
		get => _isGeoLocationEnabled;
		set
		{
			if (SetProperty(ref _isGeoLocationEnabled, value))
			{
				// Execute asynchronously and ensure we show the permission dialog
				MainThread.BeginInvokeOnMainThread(async () =>
				{
					await _settingsManager.SetGeoLocationEnabled(value);

					// If enabled, directly request the permission
					if (value)
					{
						var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
						if (status != PermissionStatus.Granted)
						{
							// If permission was denied, open system settings
							AppInfo.Current.ShowSettingsUI();
						}
					}

					// Refresh the UI after permission check
					await LoadPermissionStatusAsync();
				});
			}
		}
	}

	private bool _areNotificationsEnabled;
	public bool AreNotificationsEnabled
	{
		get => _areNotificationsEnabled;
		set
		{
			if (SetProperty(ref _areNotificationsEnabled, value))
			{
				MainThread.BeginInvokeOnMainThread(async () =>
				{
					await _settingsManager.SetNotificationsEnabled(value);

					// If enabled, request permission
					if (value)
					{
						await Permissions.RequestAsync<Permissions.PostNotifications>();
					}
				});
			}
		}
	}

	private string _selectedLanguage;
	public string SelectedLanguage
	{
		get => _selectedLanguage;
		set
		{
			if (SetProperty(ref _selectedLanguage, value))
			{
				MainThread.BeginInvokeOnMainThread(async () => await _settingsManager.SetSelectedLanguage(value));
            }
		}
	}

	// Permission status (for display only)
	private bool _isLocationPermissionGranted;
	public bool IsLocationPermissionGranted
	{
		get => _isLocationPermissionGranted;
		set => SetProperty(ref _isLocationPermissionGranted, value);
	}

	private bool _areNearbyDevicesPermissionGranted;
	public bool AreNearbyDevicesPermissionGranted
	{
		get => _areNearbyDevicesPermissionGranted;
		set => SetProperty(ref _areNearbyDevicesPermissionGranted, value);
	}

	private bool _isCameraPermissionGranted;
	public bool IsCameraPermissionGranted
	{
		get => _isCameraPermissionGranted;
		set => SetProperty(ref _isCameraPermissionGranted, value);
	}

	// Permission access information
	private string _locationLastAccessed = "Dernier accès 15:29";
	public string LocationLastAccessed
	{
		get => _locationLastAccessed;
		set => SetProperty(ref _locationLastAccessed, value);
	}

	private string _nearbyDevicesLastAccessed = "Accès au cours des dernières 24 heures";
	public string NearbyDevicesLastAccessed
	{
		get => _nearbyDevicesLastAccessed;
		set => SetProperty(ref _nearbyDevicesLastAccessed, value);
	}

	public ObservableCollection<string> AvailableLanguages { get; } = new();

	// Commands
	public ICommand ResetSettingsCommand => new AsyncRelayCommand(ResetSettingsToDefaultAsync);
	public ICommand OpenAppInfoCommand => new AsyncRelayCommand(OpenAppInfoAsync);
	public ICommand RequestLocationPermissionCommand => new AsyncRelayCommand(RequestLocationPermissionAsync);

	public SettingsViewModel()
	{
		_settingsManager = DependencyResolver.GetRequiredService<ISettingsManager>();
		LoadSettings();
		InitializeLanguages();
	}

	public override async Task InitializeAsync()
	{
		await LoadPermissionStatusAsync();
		return;
	}

	private void LoadSettings()
	{
		// Load settings from the manager
		_isGeoLocationEnabled = _settingsManager.IsGeoLocationEnabled;
		_areNotificationsEnabled = _settingsManager.AreNotificationsEnabled;
		_selectedLanguage = _settingsManager.SelectedLanguage;

		// Set default permission access times (will be refreshed in InitializeAsync)
		_locationLastAccessed = "Dernier accès 15:29";
		_nearbyDevicesLastAccessed = "Accès au cours des dernières 24 heures";
	}

	private async Task LoadPermissionStatusAsync()
	{
		try
		{
			// Check actual permission states from the system
			var locationStatus = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
			_isLocationPermissionGranted = locationStatus == PermissionStatus.Granted;
			OnPropertyChanged(nameof(IsLocationPermissionGranted));

			// Ensure the toggle matches the actual permission state
			_isGeoLocationEnabled = _settingsManager.IsGeoLocationEnabled && _isLocationPermissionGranted;
			OnPropertyChanged(nameof(IsGeoLocationEnabled));

			// Check nearby devices permission
			var nearbyStatus = await Permissions.CheckStatusAsync<Permissions.Bluetooth>();
			_areNearbyDevicesPermissionGranted = nearbyStatus == PermissionStatus.Granted;
			OnPropertyChanged(nameof(AreNearbyDevicesPermissionGranted));

			// Check camera permission
			var cameraStatus = await Permissions.CheckStatusAsync<Permissions.Camera>();
			_isCameraPermissionGranted = cameraStatus == PermissionStatus.Granted;
			OnPropertyChanged(nameof(IsCameraPermissionGranted));

			// Get last accessed times if available
			if (_settingsManager is SettingsManager fullManager)
			{
				// Translate the access times to French
				var lastAccessed = fullManager.LocationLastAccessed;
				if (lastAccessed.StartsWith("Last accessed"))
				{
					var time = lastAccessed.Replace("Last accessed ", "");
					_locationLastAccessed = $"Dernier accès {time}";
				}
				else
				{
					_locationLastAccessed = "Jamais accédé";
				}

				_nearbyDevicesLastAccessed = "Accès au cours des dernières 24 heures";

				OnPropertyChanged(nameof(LocationLastAccessed));
				OnPropertyChanged(nameof(NearbyDevicesLastAccessed));
			}
		}
		catch (Exception ex)
		{
			// Handle permission check errors
			_logger.LogError(ex, "Error checking permissions in SettingsViewModel");
			_isLocationPermissionGranted = false;
		}
	}

	private void InitializeLanguages()
	{
		AvailableLanguages.Clear();
		AvailableLanguages.Add("Français");
		AvailableLanguages.Add("English");
	}

	private async Task ResetSettingsToDefaultAsync()
	{
		await _settingsManager.ResetToDefaults();

		// Refresh the UI with the default values
		LoadSettings();
		await LoadPermissionStatusAsync();
	}

	private async Task OpenAppInfoAsync()
	{
		// Open the app info in system settings
		AppInfo.Current.ShowSettingsUI();
		await Task.CompletedTask;
	}

	private async Task RequestLocationPermissionAsync()
	{
		var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
		if (status != PermissionStatus.Granted)
		{
			// If permission denied, open settings
			AppInfo.Current.ShowSettingsUI();
		}

		// Refresh permissions
		await LoadPermissionStatusAsync();
	}
}