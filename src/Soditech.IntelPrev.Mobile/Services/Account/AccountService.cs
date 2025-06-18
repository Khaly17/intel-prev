using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl.Http;
using Flurl.Http.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Results;
using Soditech.IntelPrev.Mobile.Core.Threading;
using Soditech.IntelPrev.Mobile.Helpers;
using Soditech.IntelPrev.Mobile.Localization;
using Soditech.IntelPrev.Mobile.Services.Account.Models;
using Soditech.IntelPrev.Mobile.Services.ApiClient;
using Soditech.IntelPrev.Mobile.Services.Notifications;
using Soditech.IntelPrev.Mobile.Services.Storage;
using Soditech.IntelPrev.Proxy;
using Soditech.IntelPrev.Users.Shared;
using UserDialogs = Controls.UserDialogs.Maui.UserDialogs;

namespace Soditech.IntelPrev.Mobile.Services.Account;

public class AccountService(IServiceProvider serviceProvider): IAccountService
{
    private readonly IAccessTokenManager _accessTokenManager = serviceProvider.GetRequiredService<IAccessTokenManager>();
    //private readonly INavigation _navigationService = serviceProvider.GetRequiredService<INavigation>();
    private readonly IDataStorageService _dataStorageService = serviceProvider.GetRequiredService<IDataStorageService>();
    private readonly INotificationRegistrationService _notificationRegistrationService = serviceProvider.GetRequiredService<INotificationRegistrationService>();
    private readonly IProxyService _proxyService = serviceProvider.GetRequiredService<IProxyService>();
    private readonly ILogger<AccountService> _logger = serviceProvider.GetRequiredService<ILogger<AccountService>>();

    public async Task LogoutAsync()
    {
        await _notificationRegistrationService.DeregisterDeviceAsync().ConfigureAwait(false);
        _accessTokenManager.Logout();
        _dataStorageService.ClearSessionPersistance();
        AuthenticateResult = null;
    }

    public async Task LoginUserAsync()
    {
        await WebRequestExecutorWithParam.Execute(_accessTokenManager.LoginAsync, AuthenticateModel, AuthenticateSucceed, ex => Task.CompletedTask);
    }
    
    private async Task AuthenticateSucceed(AuthenticateResultModel result)
    {
        if (result.ShouldResetPassword)
        {
            await UserDialogs.Instance.AlertAsync(L.Localize("LoginFailed") + " " + L.Localize("ChangePasswordToLogin"));
            return;
        }

        AuthenticateResult = result;
        await _dataStorageService.StoreAuthenticateResultAsync(result);
        
        FlurlClientConfig.ConfigureFlurlHttp();

        _ = Task.Run(async () =>
        {
            var tagsResult = await _proxyService.GetAsync<IEnumerable<string>>(UserRoutes.Users.UserNotificationTags);
            if (tagsResult.IsSuccess)
            {
                var tags = tagsResult.Value;
                await _notificationRegistrationService.RegisterDeviceAsync(tags.ToArray());
            }
            else
            {
                await _notificationRegistrationService.RegisterDeviceAsync();
            }
        });
    }

   public async Task<Result> ChangePasswordAsync(string userId, string oldPassword, string newPassword)
    {
        var payload = new
        {
            UserId = userId,
            OldPassword = oldPassword,
            NewPassword = newPassword
        };

        var result = await _proxyService.PostAsync(UserRoutes.Users.UpdatePassword, payload);
        return result;
    }


    public async Task<bool> VerifyCurrentPinAsync(string pin)
    {
        return true;
    }
    public async Task<Result> ForgotPasswordAsync(string username)
    {
        try
        {
            var requestBody = new { UserName = username };
            var client = AccountService.CreateClientApi();
            var response = await client.Request(UserRoutes.Users.ForgotPassword)
                .PostJsonAsync(requestBody)
                .GetResponseAsync();

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while sending forgot password request");
            return Result.Failure(new Error("500", ex.Message));
        }
    }

    private static IFlurlClient CreateClientApi()
    {
        var client = new FlurlClientBuilder(ApiUrlConfig.BaseUrl);

#if DEBUG
        client.ConfigureInnerHandler(handler => 
        {
            handler.ServerCertificateCustomValidationCallback = (_, _, _, _) => true;
        });
#endif
        return client.Build();
    }


    public AuthenticateModel AuthenticateModel { get; set; }
    public AuthenticateResultModel? AuthenticateResult { get; set; }
    
    public bool IsUserLoggedIn =>  AuthenticateResult?.AccessToken != null;
}