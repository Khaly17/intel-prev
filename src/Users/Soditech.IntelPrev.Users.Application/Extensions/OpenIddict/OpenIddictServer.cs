using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;
using Soditech.IntelPrev.Users.Shared;

namespace Soditech.IntelPrev.Users.Application.Extensions.OpenIddict;

public static class OpenIddictServer
{
    
    public static OpenIddictServerBuilder UseOpenIddictServerConfig(this OpenIddictServerBuilder builder, IConfiguration configuration, IWebHostEnvironment environment)
    {
        builder.SetIssuer($"{configuration["App:SelfUrl"]}");

        builder.AllowAuthorizationCodeFlow()
            .RequireProofKeyForCodeExchange();

        builder.AllowPasswordFlow()
            .AllowRefreshTokenFlow();

        
        builder.RegisterClaims("tenantId");
        builder.RegisterScopes(OpenIddictConstants.Scopes.OfflineAccess);

        var accessTokenLife = configuration.GetValue<int>("OpenIddict:AccessTokenLife");
        var refreshTokenLife = configuration.GetValue<int>("OpenIddict:RefreshTokenLife");
        //if accessTokenLife is not set, default to 24 hours
        if (accessTokenLife == 0)
        {
            accessTokenLife = 24;
        }
        //if refreshTokenLife is not set, default to 365 days
        if (refreshTokenLife == 0)
        {
            refreshTokenLife = 365;
        }
        
        builder.SetAccessTokenLifetime(TimeSpan.FromHours(accessTokenLife))
            .SetIdentityTokenLifetime(TimeSpan.FromHours(24))
            .SetRefreshTokenLifetime(TimeSpan.FromDays(refreshTokenLife));

        builder
            .SetAuthorizationEndpointUris("/api/account/authorize")
            .SetTokenEndpointUris("/api/account/login", UserRoutes.Account.RefreshToken)
            .SetEndSessionEndpointUris("/api/account/logout")
            .SetUserInfoEndpointUris("/api/account/userinfo")
            .SetIntrospectionEndpointUris("/api/account/introspect")
            .SetRevocationEndpointUris("/api/account/revoke")
            .SetEndUserVerificationEndpointUris("/api/account/verify");

        // Register the ASP.NET Core host and configure the ASP.NET Core options.
        builder.UseAspNetCore()
            .EnableAuthorizationEndpointPassthrough()
            .EnableTokenEndpointPassthrough()
            .EnableEndSessionEndpointPassthrough()
            .EnableUserInfoEndpointPassthrough()
            .EnableEndUserVerificationEndpointPassthrough()
            .EnableStatusCodePagesIntegration();
        
     
        builder
            .AddCustomHandlers()
            .AddCertificate(environment, configuration);
        

        return builder;
    }
    
}