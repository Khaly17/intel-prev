using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Threading.Tasks;
using Flurl.Http;
using Flurl.Http.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.ApplicationModel;
using Soditech.IntelPrev.Mobile.Helpers;
using Soditech.IntelPrev.Mobile.Services.Account.Models;
using Soditech.IntelPrev.Mobile.Services.Storage;

namespace Soditech.IntelPrev.Mobile.Services.Account;

public class AccessTokenManager(IServiceProvider serviceProvider) : IAccessTokenManager
{
    private const string LoginUrlSegment = "api/account/login";
    private const string RefreshTokenUrlSegment = "api/account/refreshToken";

    private readonly IDataStorageService _dataStorageService = serviceProvider.GetRequiredService<IDataStorageService>();

    //private readonly AuthenticateModel _authenticateModel;

    public DateTime AccessTokenRetrieveTime { get; set; }

    /// <inheritdoc />
    public async Task<AuthenticateResultModel> RefreshTokenAsync()
    {
        var refreshTokenModel = new
        {
            refreshToken = AuthenticateResult?.RefreshToken,
            grant_type = "refresh_token"
        };

        using var client = CreateApiClient();
        var response = await client
            .Request(RefreshTokenUrlSegment)
            .PostJsonAsync(refreshTokenModel)
            .GetResponseAsync<AuthenticateResultModel>();

        if (!response.IsSuccess || response.Value == null)
        {
            AuthenticateResult = null;
            return AuthenticateResult;
            //throw new Exception(response.Error.Code + ": " + response.Error.Message);
        }

        AuthenticateResult = response.Value;
        AuthenticateResult.RefreshTokenExpireDate = DateTime.UtcNow.Add(AppConsts.RefreshTokenExpiration);
        
        await _dataStorageService.StoreAuthenticateResultAsync(AuthenticateResult);

        return AuthenticateResult;
    }

    public AuthenticateResultModel? AuthenticateResult { get; set; }

    public bool IsUserLoggedIn => AuthenticateResult?.AccessToken != null;

    public bool IsRefreshTokenExpired =>
        AuthenticateResult == null || DateTime.UtcNow >= AuthenticateResult.RefreshTokenExpireDate;

    public void Logout()
    {
        AuthenticateResult = null;
    }
    public async Task<AuthenticateResultModel?> LoginAsync(AuthenticateModel authenticateModel)
    {
        AccessTokenManager.EnsureUserNameAndPasswordProvided(authenticateModel);

        authenticateModel.AppVersion = AppInfo.VersionString;
        using var client = CreateApiClient();

        var response = await client
            .Request(LoginUrlSegment)
            .PostJsonAsync(authenticateModel)
            .GetResponseAsync<AuthenticateResultModel>();

        if (!response.IsSuccess || response.Value == null)
        {
            AuthenticateResult = null;
            throw new InvalidOperationException("Invalid username or password");
        }

        AuthenticateResult = response.Value;
        AuthenticateResult.RefreshTokenExpireDate = DateTime.UtcNow.Add(AppConsts.RefreshTokenExpiration);

        return AuthenticateResult;
    }

    private static void EnsureUserNameAndPasswordProvided(AuthenticateModel authenticateModel)
    {
        if (authenticateModel.Email == null ||
            authenticateModel.Password == null)
        {
            throw new ArgumentException("Username or password fields cannot be empty!");
        }
    }

    private static IFlurlClient CreateApiClient()
    {
        var client = new FlurlClientBuilder(ApiUrlConfig.BaseUrl)
            .ConfigureInnerHandler(h => {
                #if DEBUG
                h.ServerCertificateCustomValidationCallback = new Func<HttpRequestMessage, X509Certificate2?, X509Chain?, SslPolicyErrors, bool>((message, cert, chain, errors) => true);
                #endif
            })
            .Build();
        
        client.WithHeader("Accept", new MediaTypeWithQualityHeaderValue("application/json"));
        
        client.Settings.JsonSerializer = new DefaultJsonSerializer(new JsonSerializerOptions {
            PropertyNameCaseInsensitive = false,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        });
        
        return client;
    }
    
}