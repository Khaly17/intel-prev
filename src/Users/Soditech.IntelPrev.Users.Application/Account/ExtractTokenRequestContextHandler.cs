using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Http;
using OpenIddict.Abstractions;
using OpenIddict.Server;

namespace Soditech.IntelPrev.Users.Application.Account;

public class ExtractTokenRequestContextHandler : IOpenIddictServerHandler<OpenIddictServerEvents.ExtractTokenRequestContext>
{
    public async ValueTask  HandleAsync(OpenIddictServerEvents.ExtractTokenRequestContext context)
    {
        var request = context.Transaction.GetHttpRequest() ?? throw new InvalidOperationException();

        if (HttpMethods.IsPost(request.Method))
        {
            if (request.ContentType?.StartsWith("application/json", StringComparison.OrdinalIgnoreCase) == true)
            {
                // Handle JSON payload
                request.EnableBuffering();
                try
                {
                    context.Request = await request.ReadFromJsonAsync<OpenIddictRequest>() ?? new OpenIddictRequest();
                }
                finally
                {
                    request.Body.Position = 0L;
                }
            }
            else if (request.ContentType?.StartsWith("application/x-www-form-urlencoded", StringComparison.OrdinalIgnoreCase) == true)
            {
                // Handle x-www-form-urlencoded payload
                var form = await request.ReadFormAsync();
                context.Request = new OpenIddictRequest(form.ToDictionary());
            }
            else
            {
                context.Reject(OpenIddictConstants.Errors.InvalidRequest);
                return;
            }

        }
        else
        {
            context.Reject(OpenIddictConstants.Errors.InvalidRequest);
            return;
        }


        // Unlike a standard OAuth 2.0 implementation, ASP.NET Core Identity's login endpoint doesn't
        // specify the grant_type parameter. Since it's the only authentication method supported anyway,
        // assume all token requests are resource owner password credentials (ROPC) requests.
        //context.Request.GrantType = GrantTypes.Password;

        // The latest version of the ASP.NET Core Identity API package uses "email" instead of the
        // standard OAuth 2.0 username parameter. To work around that, the email parameter is manually
        // mapped to the standard OAuth 2.0 username parameter.
        context.Request.Username = (string?)context.Request["email"]
                                   ?? (string?)context.Request["Email"];

        context.Request.Password = (string?)context.Request["Password"] 
                                   ?? (string?)context.Request["password"];
    
        if (context.Request.GrantType == OpenIddictConstants.GrantTypes.RefreshToken)
        {
            context.Request.RefreshToken = (string?)context.Request["refresh_token"]
                                       ?? (string?)context.Request["refreshToken"]
                                       ?? (string?)context.Request["RefreshToken"];
            context.Request["refreshToken"] = null;
            context.Request["RefreshToken"] = null;

        }

    }
    
}