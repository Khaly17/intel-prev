    using Blazored.LocalStorage;

namespace Soditech.IntelPrev.Web.Services.Authentications;

public class TokenService(IServiceProvider serviceProvider) : ITokenService
{
    private readonly ILocalStorageService _localStorage = serviceProvider.GetRequiredService<ILocalStorageService>();
    private readonly ILogger<TokenService> _logger = serviceProvider.GetRequiredService<ILogger<TokenService>>();
    private readonly ICookieStorageAccessor _cookieStorageAccessor = serviceProvider.GetRequiredService<ICookieStorageAccessor>();
    public async Task<string> GetTokenAsync(CancellationToken cancellationToken)
    {
        //use cookie instead of local storage
        return await _cookieStorageAccessor.GetValueAsync<string>("token");
        // return await _localStorage.GetItemAsync<string>("token", cancellationToken) ?? string.Empty;
    }
    // set the token in the local storage
    public async Task SetTokenAsync(string token, CancellationToken cancellationToken)
    {
        // await _localStorage.SetItemAsync("token", token, cancellationToken);
        //use cookie instead of local storage
        
        await _cookieStorageAccessor.SetValueAsync("token", token);
        
    }

    // get the refresh token from the local storage
    public async Task<string> GetRefreshTokenAsync(CancellationToken cancellationToken)
    {
        //use cookie instead of local storage
        return await _cookieStorageAccessor.GetValueAsync<string>("refreshToken");
        // return await _localStorage.GetItemAsync<string>("refreshToken", cancellationToken) ?? string.Empty;
    }

    // set the refresh token in the local storage
    public async Task SetRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        //use cookie instead of local storage
        await _cookieStorageAccessor.SetValueAsync("refreshToken", refreshToken, 525600);
        // await _localStorage.SetItemAsync("refreshToken", refreshToken, cancellationToken);
    }


    public async Task<(bool, string)> TryRefreshTokenAsync(CancellationToken cancellationToken)
    {
        // Check if the refresh token is available
        //use cookie instead of local storage
        var refreshToken = await _cookieStorageAccessor.GetValueAsync<string>("refreshToken");
        // var refreshToken = await _localStorage.GetItemAsStringAsync("refreshToken", cancellationToken);

        return string.IsNullOrEmpty(refreshToken) ? (false, string.Empty) : (true, refreshToken);
    }

    public async Task Clear(CancellationToken cancellationToken)
    {
        // Clear local storage and redirect to login page
        // await _localStorage.RemoveItemAsync("token", cancellationToken);
        // await _localStorage.RemoveItemAsync("refreshToken", cancellationToken);
        
        // clear cookie instead of local storage
        await _cookieStorageAccessor.SetValueAsync("token", string.Empty);
    }
}