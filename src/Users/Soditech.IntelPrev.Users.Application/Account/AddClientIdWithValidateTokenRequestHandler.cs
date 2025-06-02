using OpenIddict.Abstractions;
using OpenIddict.Server;

namespace Soditech.IntelPrev.Users.Application.Account;

public class AddClientIdWithValidateTokenRequestHandler : IOpenIddictServerHandler<OpenIddictServerEvents.ValidateTokenRequestContext>
{
    public ValueTask  HandleAsync(OpenIddictServerEvents.ValidateTokenRequestContext context)
    {
        var clientId = context.Request.GetParameter("client_id")?.ToString();

        if (string.IsNullOrWhiteSpace(clientId))
        {
            //TODO: get the client id from app settings
            clientId = "IntelPrev_Blazor";  
            context.Request.SetParameter("client_id", new OpenIddictParameter(clientId));
        }
        
        var grantType = context.Request.GetParameter("grant_type")?.ToString();
        if (string.IsNullOrWhiteSpace(grantType))
        {
            grantType = "password";
            context.Request.SetParameter("grant_type", new OpenIddictParameter(grantType));
        }
        
        var scopes = context.Request.GetParameter("scope")?.ToString();
        if (string.IsNullOrWhiteSpace(scopes))
        {
            scopes = OpenIddictConstants.Scopes.OfflineAccess;
            context.Request.SetParameter("scope", new OpenIddictParameter(scopes));
        }
        
        return ValueTask.CompletedTask;
    }
    
}