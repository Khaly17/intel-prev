using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Server;
using OpenIddict.Server.AspNetCore;
using Soditech.IntelPrev.Users.Application.Account;

namespace Soditech.IntelPrev.Users.Application.Extensions.OpenIddict;

public static class RegisterCustomEventsHandlerExtensions
{
    public static OpenIddictServerBuilder AddCustomHandlers(this OpenIddictServerBuilder builder)
    {
        // Remove the built-in event handler responsible for extracting standard OAuth 2.0 token requests
        // (that always use form-URL encoding) and replace it by an equivalent supporting JSON payloads.
        builder.RemoveEventHandler(OpenIddictServerAspNetCoreHandlers.ExtractPostRequest<OpenIddictServerEvents.ExtractTokenRequestContext>.Descriptor);

        builder.AddEventHandler<OpenIddictServerEvents.ExtractTokenRequestContext>(x =>
        {
            x.UseScopedHandler<ExtractTokenRequestContextHandler>();

            x.AddFilter<OpenIddictServerAspNetCoreHandlerFilters.RequireHttpRequest>();
            x.SetOrder(OpenIddictServerAspNetCoreHandlers.ExtractPostRequest<OpenIddictServerEvents.ExtractTokenRequestContext>.Descriptor.Order);
        });

        builder.AddEventHandler<OpenIddictServerEvents.ProcessSignInContext>(x =>
        {
            x.UseScopedHandler<ProcessSignInContextHandler>();
            x.SetOrder(int.MinValue);
        });

        builder.AddEventHandler<OpenIddictServerEvents.GenerateTokenContext>(x =>
        {
            x.UseScopedHandler<AddCustomClaimsAndUpdateAppVersionWithGenerateTokenHandler>();
            x.SetOrder(int.MinValue);
        });

        builder.AddEventHandler<OpenIddictServerEvents.ValidateTokenRequestContext>(x =>
        {
            x.UseScopedHandler<AddClientIdWithValidateTokenRequestHandler>();
            x.SetOrder(int.MinValue);
        });

        return builder;
    }
}