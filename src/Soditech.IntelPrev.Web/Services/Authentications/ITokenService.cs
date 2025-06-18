using System.Threading;
using System.Threading.Tasks;

namespace Soditech.IntelPrev.Web.Services.Authentications;

public interface ITokenService
{
    Task<string> GetTokenAsync(CancellationToken cancellationToken = default);
    Task SetTokenAsync(string token, CancellationToken cancellationToken);
    Task<string> GetRefreshTokenAsync(CancellationToken cancellationToken = default);
    Task SetRefreshTokenAsync(string token, CancellationToken cancellationToken);
    Task<(bool, string)> TryRefreshTokenAsync(CancellationToken cancellationToken = default);
    Task Clear(CancellationToken cancellationToken = default);

}