using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.Core.App;
using Firebase.Messaging;
using Soditech.IntelPrev.Mobile.Core.Dependency;
using Soditech.IntelPrev.Mobile.Services.Notifications;

namespace Soditech.IntelPrev.Mobile;

[Service(Exported = false)]
[IntentFilter(["com.google.firebase.MESSAGING_EVENT"])]
public class PushNotificationFirebaseMessagingService : FirebaseMessagingService
{
    
    public override void OnNewToken(string? token)
    {
        DependencyResolver
            .GetRequiredService<IDeviceInstallationService>()
            .Token = token;

        DependencyResolver
            .GetRequiredService<INotificationRegistrationService>()
            .RefreshRegistrationAsync()
            .ContinueWith((task) =>
            {
                if (task.IsFaulted)
                    throw task.Exception;
            });

        CreateDefaultNotificationChannel();
    }

    public override void OnMessageReceived(RemoteMessage message)
    {
        base.OnMessageReceived(message);

        if (message.Data.TryGetValue("action", out var messageAction))
        {
            DependencyResolver
                .GetRequiredService<IPushDemoNotificationActionService>()
                .TriggerAction(messageAction);
        }

        DisplayNotification(message);
    }

    private void DisplayNotification(RemoteMessage message)
    {
        // Create the notification
        var notificationBuilder = new NotificationCompat.Builder(this, "default_channel")
            .SetContentTitle(message.GetNotification()?.Title) // define or customize the title
            .SetContentText(message.GetNotification()?.Body) // or define or customize the content text
            .SetSmallIcon(Resource.Drawable.intel_prev) // notification icon `ic_notification` or `ic_stat_ic_notification`
            .SetPriority(NotificationCompat.PriorityDefault);


        //  Display the notification
        var notificationManager = (NotificationManager?) GetSystemService(Context.NotificationService);

        // check if notification channel exists, if not create it
        if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
        {
            var channel = new NotificationChannel("default_channel", "Default", NotificationImportance.Default);
            notificationManager?.CreateNotificationChannel(channel);
        }

        // Display notification
        notificationManager?.Notify(0, notificationBuilder.Build());
    }

    private void CreateDefaultNotificationChannel()
    {
        //  Display the notification
        var notificationManager = (NotificationManager?)GetSystemService(Context.NotificationService);

        // check if notification channel exists, if not create it
        if (Build.VERSION.SdkInt < BuildVersionCodes.O) return;

        var channel = new NotificationChannel("default_channel", "Default", NotificationImportance.Default);
        notificationManager?.CreateNotificationChannel(channel);
    }
}