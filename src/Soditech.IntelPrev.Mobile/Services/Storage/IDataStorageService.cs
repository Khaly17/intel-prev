using System.Threading.Tasks;
using Soditech.IntelPrev.Mobile.Models.Common;
using Soditech.IntelPrev.Mobile.Services.Account.Models;

namespace Soditech.IntelPrev.Mobile.Services.Storage;

public interface IDataStorageService
{
    // Authentication and session related methods
    Task StoreAccessTokenAsync(string newAccessToken, string newEncryptedAccessToken);
    AuthenticateResultModel RetrieveAuthenticateResult();
    Task StoreAuthenticateResultAsync(AuthenticateResultModel? authenticateResultModel);
    UserLoginInfoPersistanceModel RetrieveLoginInfo();
    Task StoreLoginInformationAsync(UserLoginInfoPersistanceModel loginInfo);
    void ClearSessionPersistance();

    // Settings related methods
    T GetValueOrDefault<T>(string key, T defaultValue = default);
    Task SetValue<T>(string key, T value);
    bool HasValue(string key);
    void RemoveValue(string key);
}