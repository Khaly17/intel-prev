using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;

namespace Soditech.IntelPrev.Users.Persistence.Seeders;

/// <summary>
/// Creates initial data that is needed to property run the application
/// and make client-to-server communication possible.
/// </summary>
public class OpenIddictDataSeeder (IServiceProvider serviceProvider)
{
	private readonly IConfiguration _configuration = serviceProvider.GetRequiredService<IConfiguration>();
	private readonly IOpenIddictApplicationManager _applicationManager = serviceProvider.GetRequiredService<IOpenIddictApplicationManager>();
	
	
	public async Task CreateApplicationsAsync()
	{
		var commonScopes = new List<string>
		{
            OpenIddictConstants.Permissions.Scopes.Address,
            OpenIddictConstants.Permissions.Scopes.Email,
            OpenIddictConstants.Permissions.Scopes.Phone,
            OpenIddictConstants.Permissions.Scopes.Profile,
            OpenIddictConstants.Permissions.Scopes.Roles,
            OpenIddictConstants.Scopes.OfflineAccess,
		};

		var configurationSection = _configuration.GetSection("OpenIddict:Applications") ??
		                           throw new Exception("`OpenIddict:Applications` key does not exists on appSettings");

		// Blazor Client
		var blazorClientId = configurationSection["IntelPrev_Blazor:ClientId"];
		if (!string.IsNullOrWhiteSpace(blazorClientId))
		{
			var blazorRootUrl = configurationSection["IntelPrev_Blazor:RootUrl"]?.TrimEnd('/');

			await CreateApplicationAsync(
				name: blazorClientId!,
				type: OpenIddictConstants.ClientTypes.Public,
				consentType: OpenIddictConstants.ConsentTypes.Explicit,
				displayName: "Blazor Application",
				secret: null,
				grantTypes:
				[
                    OpenIddictConstants.GrantTypes.AuthorizationCode,
                    OpenIddictConstants.GrantTypes.RefreshToken,
					OpenIddictConstants.GrantTypes.Password
				],
				scopes: commonScopes,
				redirectUri: $"{blazorRootUrl}/authentication/login-callback",//configurationSection["IntelPrev_Blazor:RedirectUri"],
				postLogoutRedirectUri: $"{blazorRootUrl}/authentication/logout-callback"
			);
		}

		// Swagger Client
		var swaggerClientId = configurationSection["IntelPrev_Swagger:ClientId"];
		if (!string.IsNullOrWhiteSpace(swaggerClientId))
		{
			var swaggerRootUrl = configurationSection["IntelPrev_Swagger:RootUrl"]?.TrimEnd('/');

			await CreateApplicationAsync(
				name: swaggerClientId!,
				type: OpenIddictConstants.ClientTypes.Public,
				consentType: OpenIddictConstants.ConsentTypes.Implicit,
				displayName: "Swagger Application",
				secret: null,
				grantTypes:
				[
                    OpenIddictConstants.GrantTypes.AuthorizationCode
				],
				scopes: commonScopes,
				redirectUri: $"{swaggerRootUrl}/swagger/oauth2-redirect.html"
			);
		}

	}

    private async Task CreateApplicationAsync(
    string name,
    string type,
    string consentType,
    string displayName,
    string? secret,
    List<string> grantTypes,
    List<string> scopes,
    string? redirectUri = null,
    string? postLogoutRedirectUri = null)
    {
        ValidateClientSecrets(type, secret);
        if (await IsClientIdTakenAsync(name)) return;

        var descriptor = CreateDescriptor(name, type, consentType, displayName, secret);
        AddRedirectUris(descriptor, redirectUri, postLogoutRedirectUri);
        AddGrantTypesAndPermissions(descriptor, grantTypes, type);
        AddScopes(descriptor, scopes);

        await _applicationManager.CreateAsync(descriptor);
    }

    private void ValidateClientSecrets(string type, string? secret)
    {
        if (!string.IsNullOrEmpty(secret) && type.Equals(OpenIddictConstants.ClientTypes.Public, StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("NoClientSecretCanBeSetForPublicApplications");

        if (string.IsNullOrEmpty(secret) && type.Equals(OpenIddictConstants.ClientTypes.Confidential, StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("TheClientSecretIsRequiredForConfidentialApplications");
    }

    private async Task<bool> IsClientIdTakenAsync(string name)
    {
        return await _applicationManager.FindByClientIdAsync(name) is not null;
    }

    private static OpenIddictApplicationDescriptor CreateDescriptor(string name, string type, string consentType, string displayName, string? secret)
    {
        var descriptor = new OpenIddictApplicationDescriptor
        {
            ClientId = name,
            ClientType = type,
            ClientSecret = secret,
            ConsentType = consentType,
            DisplayName = displayName
        };

        if (consentType == OpenIddictConstants.ConsentTypes.Explicit)
        {
            descriptor.Requirements.Add(OpenIddictConstants.Requirements.Features.ProofKeyForCodeExchange);
            descriptor.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.CodeIdToken);
            descriptor.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.CodeIdTokenToken);
            descriptor.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.Token);
        }

        return descriptor;
    }

    private void AddRedirectUris(OpenIddictApplicationDescriptor descriptor, string? redirectUri, string? postLogoutRedirectUri)
    {
        if (!string.IsNullOrEmpty(redirectUri))
        {
            if (!Uri.TryCreate(redirectUri, UriKind.Absolute, out var uri) || !uri.IsWellFormedOriginalString())
                throw new ArgumentException("InvalidRedirectUri");

            if (!descriptor.RedirectUris.Contains(uri))
                descriptor.RedirectUris.Add(uri);
        }

        if (!string.IsNullOrEmpty(postLogoutRedirectUri))
        {
            if (!Uri.TryCreate(postLogoutRedirectUri, UriKind.Absolute, out var uri) || !uri.IsWellFormedOriginalString())
                throw new ArgumentException("InvalidPostLogoutRedirectUri");

            if (!descriptor.PostLogoutRedirectUris.Contains(uri))
                descriptor.PostLogoutRedirectUris.Add(uri);
        }

        if (redirectUri != null || postLogoutRedirectUri != null)
        {
            descriptor.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.EndSession);
        }
    }

    private static void AddGrantTypesAndPermissions(OpenIddictApplicationDescriptor descriptor, List<string> grantTypes, string clientType)
    {
        ArgumentNullException.ThrowIfNull(grantTypes);
        _ = new[]
        {
        OpenIddictConstants.GrantTypes.Implicit,
        OpenIddictConstants.GrantTypes.Password,
        OpenIddictConstants.GrantTypes.AuthorizationCode,
        OpenIddictConstants.GrantTypes.ClientCredentials,
        OpenIddictConstants.GrantTypes.DeviceCode,
        OpenIddictConstants.GrantTypes.RefreshToken
    };

        foreach (var grant in grantTypes)
        {
            switch (grant)
            {
                case OpenIddictConstants.GrantTypes.AuthorizationCode:
                    descriptor.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode);
                    descriptor.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.Code);
                    descriptor.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Authorization);
                    descriptor.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Token);
                    descriptor.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Revocation);
                    descriptor.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Introspection);
                    break;

                case OpenIddictConstants.GrantTypes.Implicit:
                    descriptor.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.Implicit);
                    descriptor.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Authorization);
                    descriptor.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.IdToken);

                    if (clientType.Equals(OpenIddictConstants.ClientTypes.Public, StringComparison.OrdinalIgnoreCase))
                    {
                        descriptor.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.IdTokenToken);
                        descriptor.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.Token);
                    }
                    break;

                case OpenIddictConstants.GrantTypes.Password:
                    descriptor.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.Password);
                    descriptor.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Token);
                    descriptor.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Revocation);
                    descriptor.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Introspection);
                    break;

                case OpenIddictConstants.GrantTypes.ClientCredentials:
                    descriptor.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.ClientCredentials);
                    descriptor.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Token);
                    descriptor.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Revocation);
                    descriptor.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Introspection);
                    break;

                case OpenIddictConstants.GrantTypes.RefreshToken:
                    descriptor.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.RefreshToken);
                    descriptor.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Token);
                    descriptor.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Revocation);
                    descriptor.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Introspection);
                    break;

                case OpenIddictConstants.GrantTypes.DeviceCode:
                    descriptor.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.DeviceCode);
                    descriptor.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.DeviceAuthorization);
                    descriptor.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Token);
                    descriptor.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Revocation);
                    descriptor.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Introspection);
                    break;

                default:
                    // Ajout dynamique pour grantType personnalisé
                    descriptor.Permissions.Add(OpenIddictConstants.Permissions.Prefixes.GrantType + grant);
                    break;
            }
        }
    }

    private void AddScopes(OpenIddictApplicationDescriptor descriptor, List<string> scopes)
    {
        ArgumentNullException.ThrowIfNull(scopes);
        foreach (var scope in scopes)
            descriptor.Permissions.Add(scope);
    }

}