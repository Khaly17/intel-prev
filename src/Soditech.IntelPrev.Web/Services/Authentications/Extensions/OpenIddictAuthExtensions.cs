using Microsoft.AspNetCore.Components.Authorization;

namespace Soditech.IntelPrev.Web.Services.Authentications.Extensions;

public static class OpenIddictAuthExtensions
{
    public static IServiceCollection AddOpenIddictServices(this IServiceCollection services)
    {
        services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

        services.AddTransient<Sensor6TyAuthenticationMessageHandler>();

        services.AddScoped<ITokenService, TokenService>();

        services.AddAuthorizationCore();
        
        services.AddCascadingAuthenticationState()
            .AddOptions();

        return services;
    }

    public static IHttpClientBuilder AddBearerTokenHandler(this IHttpClientBuilder builder)
    {
        return builder.AddHttpMessageHandler<Sensor6TyAuthenticationMessageHandler>();
    }
}
