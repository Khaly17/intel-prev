using System;
using System.Threading.Tasks;
using Controls.UserDialogs.Maui;
using Soditech.IntelPrev.Mobile.Core.DataStorage;
using Soditech.IntelPrev.Mobile.Localization;
using Soditech.IntelPrev.Mobile.Services.Storage;

namespace Soditech.IntelPrev.Mobile.Services.Settings;

public class SettingsManager : ISettingsManager
{
	private readonly IDataStorageService _dataStorageService;

	public bool IsGeoLocationEnabled =>
		_dataStorageService.GetValueOrDefault(DataStorageKey.Settings_GeoLocationEnabled, true);

	public bool AreNotificationsEnabled =>
		_dataStorageService.GetValueOrDefault(DataStorageKey.Settings_NotificationsEnabled, true);

	public string SelectedLanguage =>
		_dataStorageService.GetValueOrDefault(DataStorageKey.Settings_SelectedLanguage, "Français");

	// Permission access timestamps (for UI display)
	public string LocationLastAccessed =>
		_dataStorageService.GetValueOrDefault(DataStorageKey.Settings_GeoLocationLastAccessed, "Jamais accédé");

	public string NearbyDevicesLastAccessed =>
		_dataStorageService.GetValueOrDefault(DataStorageKey.Settings_NearbyDevicesLastAccessed, "Jamais accédé");

	public SettingsManager(IDataStorageService dataStorageService)
	{
		_dataStorageService = dataStorageService;
	}

	public async Task SetGeoLocationEnabled(bool enabled)
	{
		await _dataStorageService.SetValue(DataStorageKey.Settings_GeoLocationEnabled, enabled);

		// If enabling, request permission and update last accessed time
		if (enabled)
		{
			var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
			if (status == PermissionStatus.Granted)
			{
				await UpdateLocationLastAccessed();
			}
			else
			{
				// If permission not granted, suggest going to settings
				MainThread.BeginInvokeOnMainThread(() =>
				{
					UserDialogs.Instance.Confirm(new ConfirmConfig
					{
						Message = L.Localize("LocationPermissionRequired"),
						OkText = L.Localize("OpenSettings"),
						CancelText = L.Localize("Cancel"),
						Action = (confirmed) =>
						{
							if (confirmed)
							{
								AppInfo.Current.ShowSettingsUI();
							}
						}
					});
				});
			}
		}
	}

	public async Task SetNotificationsEnabled(bool enabled)
	{
		await _dataStorageService.SetValue(DataStorageKey.Settings_NotificationsEnabled, enabled);

		// Apply changes immediately if needed
		if (enabled)
		{
			// Request notification permissions if needed
			var status = await Permissions.RequestAsync<Permissions.PostNotifications>();
			if (status != PermissionStatus.Granted)
			{
				MainThread.BeginInvokeOnMainThread(() =>
				{
					UserDialogs.Instance.Confirm(new ConfirmConfig
					{
						Message = L.Localize("NotificationPermissionRequired"),
						OkText = L.Localize("OpenSettings"),
						CancelText = L.Localize("Cancel"),
						Action = (confirmed) =>
						{
							if (confirmed)
							{
								AppInfo.Current.ShowSettingsUI();
							}
						}
					});
				});
			}
		}
	}

	public async Task SetSelectedLanguage(string language)
	{
		if (string.IsNullOrEmpty(language))
		{
			language = "Français"; // Default language
		}

		await _dataStorageService.SetValue(DataStorageKey.Settings_SelectedLanguage, language);

		// Show notification that app restart is required
		UserDialogs.Instance.Alert(new AlertConfig()
		{
			Message = L.Localize("LanguageChangeRequiresRestart"),
		});
	}

	private async Task UpdateLocationLastAccessed()
	{
		// Get current time and format it for display
		var now = DateTime.UtcNow;
		var accessTime = now.ToString("HH:mm");
		await _dataStorageService.SetValue(DataStorageKey.Settings_GeoLocationLastAccessed, $"Dernier accès {accessTime}");
	}

	private async Task UpdateNearbyDevicesLastAccessed()
	{
		await _dataStorageService.SetValue(DataStorageKey.Settings_NearbyDevicesLastAccessed, "Accès au cours des dernières 24 heures");
	}

	public async Task ApplySettings()
	{
		// This method can be called at app startup to apply all settings
		var isGeoLocationEnabled = IsGeoLocationEnabled;
		var areNotificationsEnabled = AreNotificsationsEnabled;
		// var selectedLanguage = SelectedLanguage;

		// Check if location permission is already granted
		if (isGeoLocationEnabled)
		{
			var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
			if (status == PermissionStatus.Granted)
			{
				await UpdateLocationLastAccessed();
			}
			else if (status == PermissionStatus.Denied)
			{
				// If permission was previously denied but setting is enabled,
				// we'll need to request it again or direct user to settings
				// This prevents automatic requests on every app start
				await _dataStorageService.SetValue(DataStorageKey.Settings_GeoLocationEnabled, false);
			}
		}

		// Apply notification settings
		if (areNotificationsEnabled)
		{
			var status = await Permissions.CheckStatusAsync<Permissions.PostNotifications>();
			if (status == PermissionStatus.Denied)
			{
				// Similar logic as above
				await _dataStorageService.SetValue(DataStorageKey.Settings_NotificationsEnabled, false);
			}
		}
	}

	public async Task ResetToDefaults()
	{
		await SetGeoLocationEnabled(true);
		await SetNotificationsEnabled(true);
		await SetSelectedLanguage("Français");

		UserDialogs.Instance.Alert(new AlertConfig()
		{
			Message = L.Localize("SettingsResetToDefaults"),
		});
	}
}