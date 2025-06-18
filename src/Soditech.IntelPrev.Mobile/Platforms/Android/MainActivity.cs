using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Firebase.Messaging;
using Java.Lang;
using Microsoft.Maui;
using Soditech.IntelPrev.Mobile.Core.Dependency;
// Added for SoftInput
using Soditech.IntelPrev.Mobile.Services.Notifications;
using Microsoft.Extensions.Logging;

namespace Soditech.IntelPrev.Mobile;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity, Android.Gms.Tasks.IOnSuccessListener
{
	private readonly ILogger<MainActivity> _logger = DependencyResolver.GetRequiredService<ILogger<MainActivity>>();

	private IPushDemoNotificationActionService? _notificationActionService;
	private IDeviceInstallationService? _deviceInstallationService;

	private IPushDemoNotificationActionService NotificationActionService =>
		_notificationActionService ??= DependencyResolver.GetRequiredService<IPushDemoNotificationActionService>();

	private IDeviceInstallationService DeviceInstallationService =>
		_deviceInstallationService ??= DependencyResolver.GetRequiredService<IDeviceInstallationService>();

	public void OnSuccess(Object? result)
	{
		DeviceInstallationService.Token = result?.ToString();
	}

	private void ProcessNotificationsAction(Intent? intent)
	{
		try
		{
			if (intent?.HasExtra("action") != true) return;
			var action = intent.GetStringExtra("action");

			if (!string.IsNullOrEmpty(action))
				NotificationActionService.TriggerAction(action);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error processing notification action");
		}
	}

	protected override void OnNewIntent(Intent? intent)
	{
		base.OnNewIntent(intent);
		ProcessNotificationsAction(intent);
	}

	protected override void OnCreate(Bundle? savedInstanceState)
	{
		base.OnCreate(savedInstanceState);

		if (DeviceInstallationService.NotificationsSupported)
		{
			FirebaseMessaging.Instance.GetToken().AddOnSuccessListener(this);
		}
				

		ProcessNotificationsAction(Intent);
	}
}