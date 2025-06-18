using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Soditech.IntelPrev.Web.Services.Authentications;
using Sensor6ty.Results;


namespace Soditech.IntelPrev.Web.Services.UserInfo;

public class UserInfoService(IServiceProvider serviceProvider) : IUserInfoService
{
    private readonly ITokenService _tokenService = serviceProvider.GetRequiredService<ITokenService>();
    /// <inheritdoc />
    public async Task<TResult<Models.UserInfo>> GetUserInfoAsync()
    {
        var token = await _tokenService.GetTokenAsync();
        
        if (string.IsNullOrEmpty(token)) return Result.Failure<Models.UserInfo>(new Error("401","Token is empty"));
        
        var jwtHandler = new JwtSecurityTokenHandler();
        var jwtToken = jwtHandler.ReadJwtToken(token);

        var result = new Models.UserInfo
        {
            Id = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value ?? string.Empty,
            TenantId = jwtToken.Claims.FirstOrDefault(c => c.Type == "tenant_id")?.Value ?? string.Empty,
            TenantName = jwtToken.Claims.FirstOrDefault(c => c.Type == "tenant_name")?.Value ?? string.Empty,
            Username = jwtToken.Claims.FirstOrDefault(c => c.Type == "username")?.Value ?? string.Empty,
            Email = jwtToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value ?? string.Empty,
            FullName = jwtToken.Claims.FirstOrDefault(c => c.Type == "name")?.Value ?? string.Empty,
            Roles = jwtToken.Claims.Where(c => c.Type == "role").Select(c => c.Value).ToList()
        };


        return Result.Success(result);
    }
}