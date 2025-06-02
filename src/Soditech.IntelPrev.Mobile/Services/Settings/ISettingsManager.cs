using System.Threading.Tasks;

namespace Soditech.IntelPrev.Mobile.Services.Settings;

public interface ISettingsManager
{
    bool IsGeoLocationEnabled { get; }
    bool AreNotificationsEnabled { get; }
    string SelectedLanguage { get; }

    Task SetGeoLocationEnabled(bool enabled);
    Task SetNotificationsEnabled(bool enabled);
    Task SetSelectedLanguage(string language);
    Task ApplySettings();
    Task ResetToDefaults();
}