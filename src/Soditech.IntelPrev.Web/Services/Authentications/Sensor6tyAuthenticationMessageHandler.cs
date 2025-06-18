using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace Soditech.IntelPrev.Web.Services.Authentications;

public class Sensor6TyAuthenticationMessageHandler(IServiceProvider serviceProvider) 
    : DelegatingHandler
{
    private readonly ITokenService _tokenService = serviceProvider.GetRequiredService<ITokenService>();
    private readonly NavigationManager _navigationManager = serviceProvider.GetRequiredService<NavigationManager>();

    protected override async Task<HttpResponseMessage> 
        SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _tokenService.GetTokenAsync(cancellationToken);

        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.Replace("\"", ""));
        }

        var response = await base.SendAsync(request, cancellationToken);

        if (_navigationManager.Uri.Contains("/login") || response.StatusCode != HttpStatusCode.Unauthorized)
            return response;
        
        // Get the return URL from the current location
        var returnUrl = _navigationManager.Uri.Replace(_navigationManager.BaseUri, "");
        
        var path = string.IsNullOrEmpty(returnUrl) ? "/login" : "/login?returnUrl=" + Uri.EscapeDataString(returnUrl);
        // Redirect to login page with returnUrl
        _navigationManager.NavigateTo(path, true);
            
        return response;
    }
}