using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Server.AspNetCore;
using OpenIddict.Validation.AspNetCore;
using Soditech.IntelPrev.Users.Application.Extensions.OpenIddict;
using Soditech.IntelPrev.Users.Persistence;

namespace Soditech.IntelPrev.Users.WebApi.Extensions.OpenIddict;

public static class OpenIddictExtensions
{
    public static WebApplicationBuilder AddOpenIddictServices(this WebApplicationBuilder builder)
    {
        IConfiguration configuration = builder.Configuration;
        var environment = builder.Environment;

        builder.Services
            .AddOpenIddict()
            .AddCore(options =>
            {
                options.UseEntityFrameworkCore()
                    .UseDbContext<UserDbContext>();
            })
            .AddServer(options =>
            {
                options.UseOpenIddictServerConfig(configuration, environment);
            })
            .AddValidation(options =>
            {
                options.UseLocalServer();

                options.UseAspNetCore();

                options.UseSystemNetHttp();
            });

        builder.Services.AddAuthentication(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

        builder.Services.AddAuthorization();
        
        builder.Services.Configure<BearerTokenOptions>(IdentityConstants.BearerScheme, options =>
        {
            // Forward the sign-in responses returned by ASP.NET Core Identity's
            // login API endpoint to the OpenIddict server stack so that OpenIddict
            // can generate a token response based on the principal prepared by Identity.
            options.ForwardSignIn = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme;

            // Forward the token authentication operations to the OpenIddict validation stack.
            options.ForwardAuthenticate = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
            options.ForwardChallenge = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
            options.ForwardForbid = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
        });

        return builder;
    }
}