using Soditech.IntelPrev.Web.Services.Proxy;
using Microsoft.AspNetCore.Components.Authorization;
using Sensor6ty.Results;
using Soditech.IntelPrev.Users.Shared;
using Soditech.IntelPrev.Users.Shared.Account;

namespace Soditech.IntelPrev.Web.Services.Authentications;

/// <inheritdoc />
public class AuthenticationServices(IServiceProvider serviceProvider) : IAuthenticationServices
{
    private readonly ITokenService _tokenService = serviceProvider.GetRequiredService<ITokenService>();
    private readonly IProxyService _proxyService = serviceProvider.GetRequiredService<IProxyService>();
    private readonly AuthenticationStateProvider _authenticationStateProvider = serviceProvider.GetRequiredService<AuthenticationStateProvider>();
    private readonly ILogger<AuthenticationServices>  _logger = serviceProvider.GetRequiredService<ILogger<AuthenticationServices>>();

    /// <inheritdoc />
    public async Task Logout()
    {
        // Clear token from local storage
        await _tokenService.Clear();

        // Notify authentication state change
        await _authenticationStateProvider.GetAuthenticationStateAsync();

    }
    

    /// <inheritdoc />
    public async Task<TResult<WebLoginCommandResult>> Login(WebLoginCommand authenticationRequest, CancellationToken cancellationToken = default)
    {
        TResult<WebLoginCommandResult> response ;

        try
        {
            response = await _proxyService.PostAsync<WebLoginCommandResult>(UserRoutes.Account.Login, authenticationRequest, cancellationToken);
            
            if (response.IsSuccess)
            {
                await _tokenService.SetTokenAsync(response.Value.AccessToken, cancellationToken);
                await _tokenService.SetRefreshTokenAsync(response.Value.RefreshToken, cancellationToken);
                
                await _authenticationStateProvider.GetAuthenticationStateAsync();
                
            }
            else
            {
                // Log the error, if response.Error.Code is not 401 
                if (response.Error.Code != "401")
                {
                    _logger.LogError("Failed to login: {code} - {message}", response.Error.Code, response.Error.Message);
                }
            }

        }
        catch (Exception ex)
        {
            // Log the exception
            _logger.LogError(ex, "Failed to login");
            response = Result.Failure<WebLoginCommandResult>(new Error("400", "Failed to login"));
        }

        return response;

    }

    /// <inheritdoc />
    public Task<TResult<WebLoginCommandResult>> RefreshToken(string refreshToken, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Result.Failure<WebLoginCommandResult>(new Error("400", "Failed to refresh token")));
    }
    
    /// <inheritdoc />
    public async Task<Result> ForgotPassword(string username, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            return Result.Failure(new Error("400", "Nom d'utilisateur requis."));
        }

        var forgotPasswordRequest = new { Username = username };

        try
        {
            var response = await _proxyService.PostAsync(
                UserRoutes.Users.ForgotPassword, 
                forgotPasswordRequest, 
                cancellationToken);

            if (!response.IsSuccess)
            {
                _logger.LogError("Failed to process forgot password: {code} - {message}", response.Error.Code, response.Error.Message);
            }

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to process forgot password");
            return Result.Failure(new Error("500", "Une erreur s'est produite lors de la réinitialisation du mot de passe."));
        }
    }


    Task<TResult<WebLoginCommandResult>> IAuthenticationServices.RefreshToken(string refreshToken, CancellationToken cancellationToken)
    {
        //TODO: Not yet implemented
        return Task.FromResult(TResult<WebLoginCommandResult>.Failure(
            "refresh_not_available", "La fonctionnalité de rafraîchissement de token n'est pas encore disponible."));
    }

    async Task IAuthenticationServices.Logout()
    {
        await _tokenService.Clear();
        await _authenticationStateProvider.GetAuthenticationStateAsync();
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        var token = await _tokenService.GetTokenAsync();
        return !string.IsNullOrEmpty(token);
    }
}