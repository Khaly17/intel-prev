namespace Soditech.IntelPrev.Mobile.Core.DataStorage;

public class DataStorageKey
{
	public const string CurrentSession_TokenInfo = "CurrentSession.TokenInfo";
	public const string CurrentSession_LoginInfo = "CurrentSession.LoginInfo";

	// Settings keys
	public const string Settings_GeoLocationEnabled = "Settings.GeoLocationEnabled";
	public const string Settings_NotificationsEnabled = "Settings.NotificationsEnabled";
	public const string Settings_SelectedLanguage = "Settings.SelectedLanguage";

	// New permission access timestamp keys
	public const string Settings_GeoLocationLastAccessed = "settings_geolocation_last_accessed";
	public const string Settings_NearbyDevicesLastAccessed = "settings_nearbydevices_last_accessed";
	public const string Settings_CameraLastAccessed = "settings_camera_last_accessed";

}