namespace Soditech.IntelPrev.Mobile.Models.Settings;

public class ChangePasswordModel
{
    private string _currentPassword = string.Empty;
    private string _newPassword = string.Empty;

    private string _newPasswordRepeat =  string.Empty;

    public string CurrentPassword
    {
        get => _currentPassword;
        set
        {
            _currentPassword = value;
            SetChangePasswordButtonStatus();
        }
    }

    public string NewPassword
    {
        get => _newPassword;
        set
        {
            _newPassword = value;
            SetChangePasswordButtonStatus();
        }
    }

    public string NewPasswordRepeat
    {
        get => _newPasswordRepeat;
        set
        {
            _newPasswordRepeat = value;
            SetChangePasswordButtonStatus();
        }
    }

    public bool IsChangePasswordDisabled { get; set; } = true;

    private void SetChangePasswordButtonStatus()
    {
        IsChangePasswordDisabled = string.IsNullOrWhiteSpace(CurrentPassword)
                                   || string.IsNullOrWhiteSpace(NewPassword)
                                   || string.IsNullOrWhiteSpace(NewPasswordRepeat)
                                   || NewPassword != NewPasswordRepeat;
    }
}