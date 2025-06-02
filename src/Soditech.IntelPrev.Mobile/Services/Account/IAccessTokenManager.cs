using System;
using System.Threading.Tasks;
using Soditech.IntelPrev.Mobile.Services.Account.Models;

namespace Soditech.IntelPrev.Mobile.Services.Account;

public interface IAccessTokenManager
{
    Task<AuthenticateResultModel> LoginAsync(AuthenticateModel authenticateModel);
    void Logout();
    
    bool IsUserLoggedIn { get; }

    bool IsRefreshTokenExpired { get; }

    AuthenticateResultModel AuthenticateResult { get; set; }

    DateTime AccessTokenRetrieveTime { get; set; }
    
    Task<AuthenticateResultModel> RefreshTokenAsync();
}