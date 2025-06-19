using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Foundation;
using Microsoft.Maui;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Hosting;
using Soditech.IntelPrev.Mobile.Core.Dependency;
using Soditech.IntelPrev.Mobile.Services.Notifications;
using UIKit;
using UserNotifications;

namespace Soditech.IntelPrev.Mobile;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
    //     private readonly ILogger<AppDelegate> _logger =
    // Core.Dependency.DependencyResolver.GetRequiredService<ILogger<AppDelegate>>();

    private IPushDemoNotificationActionService? _notificationActionService;
    private INotificationRegistrationService? _notificationRegistrationService;
    private IDeviceInstallationService? _deviceInstallationService;

    private IPushDemoNotificationActionService NotificationActionService =>
        _notificationActionService ??= DependencyResolver.GetRequiredService<IPushDemoNotificationActionService>();

    private IDeviceInstallationService DeviceInstallationService =>
        _deviceInstallationService ??= DependencyResolver.GetRequiredService<IDeviceInstallationService>();

    private INotificationRegistrationService NotificationRegistrationService =>
        _notificationRegistrationService ??= DependencyResolver.GetRequiredService<INotificationRegistrationService>();

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    Task CompleteRegistrationAsync(NSData deviceToken)
    {
        DeviceInstallationService.Token = deviceToken.ToHexString();
        return NotificationRegistrationService.RefreshRegistrationAsync();
    }

    void ProcessNotificationActions(NSDictionary? userInfo)
    {
        if (userInfo == null)
            return;

        try
        {
            // If your app isn't in the foreground, the notification goes to Notification Center.
            // If your app is in the foreground, the notification goes directly to your app and you
            // need to process the notification payload yourself.
            var actionValue = userInfo.ObjectForKey(new NSString("action")) as NSString;

            if (!string.IsNullOrWhiteSpace(actionValue?.Description))
                NotificationActionService.TriggerAction(actionValue.Description);
        }
        catch (Exception ex)
        {
            // _logger.LogError(ex, "Error processing notification actions");
            Debug.Write(ex);
        }
    }

    [Export("application:didRegisterForRemoteNotificationsWithDeviceToken:")]
    public void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
    {
        CompleteRegistrationAsync(deviceToken)
            .ContinueWith((task) =>
            {
                if (task.IsFaulted)
                    throw task.Exception;
            });
    }

    [Export("application:didReceiveRemoteNotification:")]
    public void ReceivedRemoteNotification(UIApplication application, NSDictionary? userInfo)
    {
        ProcessNotificationActions(userInfo);
    }

    [Export("application:didFailToRegisterForRemoteNotificationsWithError:")]
    public static void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
    {
        //TOOD: For production scenarios, you'll want to implement proper logging and error handling
        //in the FailedToRegisterForRemoteNotifications method
        // _logger.LogError("Failed to register for remote notifications: {Error}", error.Description);
    }

    [Export("application:didFinishLaunchingWithOptions:")]
    public override bool FinishedLaunching(UIApplication application, NSDictionary? launchOptions)
    {
        if (DeviceInstallationService.NotificationsSupported)
        {
            UNUserNotificationCenter.Current.RequestAuthorization(
                UNAuthorizationOptions.Alert |
                UNAuthorizationOptions.Badge |
                UNAuthorizationOptions.Sound,
                (approvalGranted, error) =>
                {
                    if (approvalGranted && error == null)
                    {
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            UIApplication.SharedApplication.RegisterForRemoteNotifications();
                        });
                    }
                });
        }

        using (var userInfo = launchOptions?.ObjectForKey(UIApplication.LaunchOptionsRemoteNotificationKey) as NSDictionary)
        {
            ProcessNotificationActions(userInfo);
        }

        return base.FinishedLaunching(application, launchOptions);
    }
}