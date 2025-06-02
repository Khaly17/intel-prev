using FastEndpoints;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using Soditech.IntelPrev.Users.Shared;

namespace Soditech.IntelPrev.Users.WebApi.Endpoints.Account;


[HttpPost(UserRoutes.Account.RefreshToken)]
[Tags("account")]
[AllowAnonymous]
public class TokenEndpoint : Endpoint<RefreshRequest>
{

    public override async Task HandleAsync(RefreshRequest req, CancellationToken ct)
    {
        var openIdConnectRequest = HttpContext.GetOpenIddictServerRequest() ??
                                   throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

        if (openIdConnectRequest.IsRefreshTokenGrantType())
        {
            // Retrieve the claims principal stored in the authorization code/refresh token.
            var authenticateResult = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

            if (authenticateResult is { Succeeded: true, Principal: not null })
            {
                // Sign in the user.
                await HttpContext.SignInAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme, authenticateResult.Principal);

                return;
            }
            else if (authenticateResult.Failure is not null)
            {
                var failureMessage = authenticateResult.Failure.Message;
                var failureException = authenticateResult.Failure.InnerException;

                await SendAsync(new OpenIddictResponse
                {
                    Error = OpenIddictConstants.Errors.InvalidRequest,
                    ErrorDescription = failureMessage + failureException
                }, 401, ct);

                return;
            }
        }

        await SendAsync(new OpenIddictResponse
        {
            Error = OpenIddictConstants.Errors.InvalidRequest,
            ErrorDescription = "Error in refreshing token"
        }, 401, ct);
    }
}
