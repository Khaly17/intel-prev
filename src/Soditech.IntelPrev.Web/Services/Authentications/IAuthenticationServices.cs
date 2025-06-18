using System.Threading;
using System.Threading.Tasks;
using Sensor6ty.Results;
using Soditech.IntelPrev.Users.Shared.Account;

namespace Soditech.IntelPrev.Web.Services.Authentications;

/// <summary>
/// Allows to authenticate the user by providing login and refresh token methods.
/// </summary>
public interface IAuthenticationServices
{
    Task<TResult<WebLoginCommandResult>> Login(WebLoginCommand authenticationRequest, CancellationToken cancellationToken = default);
    Task<TResult<WebLoginCommandResult>> RefreshToken(string refreshToken, CancellationToken cancellationToken = default);
    
    Task Logout();
    
    Task<Result> ForgotPassword(string userName, CancellationToken cancellationToken = default);
    public Task<bool> IsAuthenticatedAsync();
}