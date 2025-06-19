using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace Soditech.IntelPrev.Web.Services.Helper;

/// <summary>
/// Provides navigation services including opening URLs in new tabs and exposing methods from NavigationManager.
/// </summary>
public class NavigationService(IServiceProvider serviceProvider)
{
    private readonly NavigationManager _navigationManager = serviceProvider.GetRequiredService<NavigationManager>();
    private readonly IJSRuntime _jsRuntime = serviceProvider.GetRequiredService<IJSRuntime>();
    private readonly ILogger<NavigationService> _logger = serviceProvider.GetRequiredService<ILogger<NavigationService>>();

    /// <summary>
    /// Navigates to the specified URL in a new browser tab.
    /// </summary>
    /// <param name="url">The URL to navigate to.</param>
    public async Task NavigateToNewTabAsync(string url)
    {
        try
        {
            await _jsRuntime.InvokeVoidAsync("openInNewTab", url);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to navigate to new tab");
            _navigationManager.NavigateTo(url); // Fallback to normal navigation
        }
    }

    /// <summary>
    /// Gets the current URI.
    /// </summary>
    public string Uri => _navigationManager.Uri;

    /// <summary>
    /// Gets the base URI.
    /// </summary>
    public string BaseUri => _navigationManager.BaseUri;

    /// <summary>
    /// Navigates to the specified URI.
    /// </summary>
    /// <param name="uri">The URI to navigate to.</param>
    /// <param name="forceLoad">If true, bypasses client-side routing and forces the browser to load the new page from the server.</param>
    public void NavigateTo(string uri, bool forceLoad = false) => _navigationManager.NavigateTo(uri, forceLoad);

    /// <summary>
    /// Navigates to the specified URI with the given navigation options.
    /// </summary>
    /// <param name="uri">The URI to navigate to.</param>
    /// <param name="options">The navigation options.</param>
    public void NavigateTo(string uri, NavigationOptions options) => _navigationManager.NavigateTo(uri, options);

    /// <summary>
    /// Converts a relative URI to an absolute URI.
    /// </summary>
    /// <param name="relativeUri">The relative URI to convert.</param>
    /// <returns>The absolute URI.</returns>
    public Uri ToAbsoluteUri(string? relativeUri) => _navigationManager.ToAbsoluteUri(relativeUri);
}
