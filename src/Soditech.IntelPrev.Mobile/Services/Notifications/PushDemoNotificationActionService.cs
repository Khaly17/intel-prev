using System;
using System.Collections.Generic;
using System.Linq;

namespace Soditech.IntelPrev.Mobile.Services.Notifications;

public class PushDemoNotificationActionService : IPushDemoNotificationActionService
{
    readonly Dictionary<string, PushDemoAction> _actionMappings = new()
    {
        { "action_a", PushDemoAction.ActionA },
        { "action_b", PushDemoAction.ActionB }
    };

    public event EventHandler<PushDemoAction> ActionTriggered = delegate { };

    public void TriggerAction(string action)
    {
        if (!_actionMappings.TryGetValue(action, out var pushDemoAction))
            return;

        var exceptions = new List<Exception>();

        foreach (var handler in ActionTriggered.GetInvocationList())
        {
            try
            {
                handler.DynamicInvoke(this, pushDemoAction);
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }
        }

        if (exceptions.Any())
            throw new AggregateException(exceptions);
    }
}

public enum PushDemoAction
{
    ActionA,
    ActionB
}