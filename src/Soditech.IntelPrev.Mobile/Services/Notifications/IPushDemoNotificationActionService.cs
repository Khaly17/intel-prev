namespace Soditech.IntelPrev.Mobile.Services.Notifications;

public interface IPushDemoNotificationActionService : INotificationActionService
{
    event EventHandler<PushDemoAction> ActionTriggered;
}