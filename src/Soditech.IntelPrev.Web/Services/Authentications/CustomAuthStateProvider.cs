using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace Soditech.IntelPrev.Web.Services.Authentications;


/// <summary>
/// Provides the authentication state of the user.
/// </summary>
/// <param name="serviceProvider"></param>
public class CustomAuthStateProvider(IServiceProvider serviceProvider) : AuthenticationStateProvider
{
    private readonly ITokenService _tokenService = serviceProvider.GetRequiredService<ITokenService>();
    private readonly HttpClient _http = serviceProvider.GetRequiredService<HttpClient>();

  

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var jwt = await _tokenService.GetTokenAsync();

        var identity = new ClaimsIdentity();
        _http.DefaultRequestHeaders.Authorization = null;

        if (!string.IsNullOrEmpty(jwt))
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(jwt);
            
            // Get the claims
            var claims = token.Claims.ToList(); 
            
            // Get the role claim
            var roleClaim = claims.Where(c => c.Type == "role").ToList();
            if (roleClaim.Count != 0)
            {
                // for each role claim, add the roles to the claims.
                claims.AddRange(roleClaim.Select(claim => new Claim(ClaimTypes.Role, claim.Value)));
            }
          
            identity = new ClaimsIdentity(claims, "Bearer");
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", jwt.Replace("\"", ""));
        }

        var user = new ClaimsPrincipal(identity);
        var state = new AuthenticationState(user);
        
        NotifyAuthenticationStateChanged(Task.FromResult(state));

        return state;
    }
}