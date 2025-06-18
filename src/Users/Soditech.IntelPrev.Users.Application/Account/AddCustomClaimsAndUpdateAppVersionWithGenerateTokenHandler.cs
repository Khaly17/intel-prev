using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;
using OpenIddict.Server;
using Sensor6ty.Repositories;
using Soditech.IntelPrev.Users.Persistence.Models;

namespace Soditech.IntelPrev.Users.Application.Account;


public class AddCustomClaimsAndUpdateAppVersionWithGenerateTokenHandler(IServiceProvider serviceProvider): IOpenIddictServerHandler<OpenIddictServerEvents.GenerateTokenContext>
{
    private readonly IRepository<User> _userRepository = serviceProvider.GetRequiredService<IRepository<User>>();
    
    public async ValueTask  HandleAsync(OpenIddictServerEvents.GenerateTokenContext context)
    {
        var userId = context.Principal.GetClaim(OpenIddictConstants.Claims.Subject);
        if (!string.IsNullOrEmpty(userId))
        {
            //TODO: check if tenant id is valid
            var user = await _userRepository.GetAsync(Guid.Parse(userId));
            if (user != null) 
            {
                context.Principal.SetClaim(OpenIddictConstants.Claims.Name, user.FullName);

                if (user.TenantId != null)
                {
                    context.Principal.SetClaim("tenant_id", user.TenantId.ToString());
                    context.Principal.SetClaim("tenant_name", user.Tenant?.DisplayName);
                }
                
                //TODO: update mobile app version when user is connected to the mobile app
                var appVersion = context.Request?.GetParameter("appVersion")?.ToString();
                if (!string.IsNullOrEmpty(appVersion))
                {
                    user.AppVersion = appVersion;
                    await _userRepository.UpdateAsync(user);
                }
            }
        }
    }
}