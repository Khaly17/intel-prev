using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Soditech.IntelPrev.Mobile.Core.Dependency;
using Soditech.IntelPrev.Mobile.Services.Account;

namespace Soditech.IntelPrev.Mobile.Services.ApiClient;

public class ReTryWithNewCredentialHandler : DelegatingHandler
{
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private const string AuthorizationScheme = "Bearer";

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            response = await HandleUnauthorizedResponse(request, response, cancellationToken);
        }

        return response;
    }

    

    private async Task<HttpResponseMessage> HandleUnauthorizedResponse(HttpRequestMessage request, HttpResponseMessage response, CancellationToken cancellationToken)
    {
        await _semaphore.WaitAsync(cancellationToken);

        try
        {
            var tokenManager = DependencyResolver.GetRequiredService<IAccessTokenManager>();

            if (tokenManager.IsRefreshTokenExpired)
            {
                await ReTryWithNewCredentialHandler.HandleSessionExpired(tokenManager);
            }
            else
            {
                response = await RefreshAccessTokenAndSendRequestAgain(request, tokenManager, cancellationToken);
            }
        }
        finally
        {
            _semaphore.Release();
        }

        return response;
    }

    private static async Task HandleSessionExpired(IAccessTokenManager tokenManager)
    {
        tokenManager.Logout();

        await DependencyResolver.GetRequiredService<IAccountService>().LogoutAsync();

        await Shell.Current.GoToAsync(new ShellNavigationState(AppRoutes.LoginPage));
    }
 
    private async Task<HttpResponseMessage> RefreshAccessTokenAndSendRequestAgain(HttpRequestMessage request, 
        IAccessTokenManager tokenManager, CancellationToken cancellationToken)
    {
        await RefreshToken(tokenManager, request);
        return await base.SendAsync(request, cancellationToken);
    }

    private async Task RefreshToken(IAccessTokenManager tokenManager, HttpRequestMessage request)
    {
        var newTokens = await tokenManager.RefreshTokenAsync();

        if (!string.IsNullOrEmpty(newTokens.AccessToken))
        {
            SetRequestAccessToken(newTokens.AccessToken, request);
        }
        else
        {
            await ReTryWithNewCredentialHandler.HandleSessionExpired(tokenManager);
        }
    }

    
    private static void SetRequestAccessToken(string accessToken, HttpRequestMessage request)
    {
        request.Headers.Authorization = new AuthenticationHeaderValue(AuthorizationScheme, accessToken);
    }

}