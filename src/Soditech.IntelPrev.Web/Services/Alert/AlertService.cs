using  Soditech.IntelPrev.Web.Models.Utils;

namespace  Soditech.IntelPrev.Web.Services.Alert;

public class AlertService
{
    public string AlertMessage { get; private set; } = string.Empty;
    public string AlertType { get; private set; } = "success";
    public bool IsAlertVisible { get; private set; }
    public ActionType CurrentActionType { get; private set; }

    public event Action? OnChange;

    public void ShowAlert(string message, ActionType actionType, bool isSuccess = true)
    {
        AlertMessage = message;
        AlertType = isSuccess ? "success" : "danger";
        CurrentActionType = actionType;
        IsAlertVisible = true;

        // Notify any subscribers (e.g., components) of the alert change
        OnChange?.Invoke();

        Task.Delay(3000).ContinueWith(_ =>
        {
            HideAlert();
        });
    }

    public void HideAlert()
    {
        IsAlertVisible = false;
        CurrentActionType = default;
        OnChange?.Invoke();
    }
}
