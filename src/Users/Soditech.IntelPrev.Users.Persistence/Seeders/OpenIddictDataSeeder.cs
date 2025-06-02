using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;

namespace Soditech.IntelPrev.Users.Persistence.OpenIddict;

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
		if (!string.IsNullOrEmpty(secret) && string.Equals(type, OpenIddictConstants.ClientTypes.Public, StringComparison.OrdinalIgnoreCase))
		{
			//No Client Secret Can Be Set For Public Applications
			throw new ArgumentException("NoClientSecretCanBeSetForPublicApplications");
		}

		if (string.IsNullOrEmpty(secret) && string.Equals(type, OpenIddictConstants.ClientTypes.Confidential, StringComparison.OrdinalIgnoreCase))
		{
			//The Client Secret Is Required For Confidential Applications
			throw new ArgumentException("TheClientSecretIsRequiredForConfidentialApplications");
		}

		if (!string.IsNullOrEmpty(name) && await _applicationManager.FindByClientIdAsync(name) != null)
		{
			//The Client Identifier Is Already Taken By Another Application
			return;
		}

		var client = await _applicationManager.FindByClientIdAsync(name);
		if (client == null)
		{
			OpenIddictApplicationDescriptor application;

			if (consentType == OpenIddictConstants.ConsentTypes.Explicit)
			{
				application = new OpenIddictApplicationDescriptor
				{
					ClientId = name,
					ClientType = type,
					ClientSecret = secret,
					ConsentType = consentType,
					DisplayName = displayName,
					Requirements = {
                        OpenIddictConstants.Requirements.Features.ProofKeyForCodeExchange,
					}
				};
				application.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.CodeIdToken);
				application.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.CodeIdTokenToken);
				application.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.Token);
			}
			else
			{
				application = new OpenIddictApplicationDescriptor
				{
					ClientId = name,
					ClientType = type,
					ClientSecret = secret,
					ConsentType = consentType,
					DisplayName = displayName
				};
			}

			ArgumentNullException.ThrowIfNull(grantTypes);
			ArgumentNullException.ThrowIfNull(scopes);


			if (new[] { OpenIddictConstants.GrantTypes.AuthorizationCode, OpenIddictConstants.GrantTypes.Implicit }.All(grantTypes.Contains))
			{
				application.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.CodeIdToken);

				if (string.Equals(type, OpenIddictConstants.ClientTypes.Public, StringComparison.OrdinalIgnoreCase))
				{
					application.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.CodeIdTokenToken);
					application.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.CodeToken);
				}
			}

			if (!string.IsNullOrWhiteSpace(redirectUri) || !string.IsNullOrWhiteSpace(postLogoutRedirectUri))
			{
				application.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.EndSession);
			}

			var buildInGrantTypes = new[]
			{
                OpenIddictConstants.GrantTypes.Implicit,
                OpenIddictConstants.GrantTypes.Password,
                OpenIddictConstants.GrantTypes.AuthorizationCode,
                OpenIddictConstants.GrantTypes.ClientCredentials,
                OpenIddictConstants.GrantTypes.DeviceCode,
                OpenIddictConstants.GrantTypes.RefreshToken
			};

			foreach (var grantType in grantTypes)
			{
				if (grantType == OpenIddictConstants.GrantTypes.AuthorizationCode)
				{
					application.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode);
					application.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.Code);
				}

				if (grantType == OpenIddictConstants.GrantTypes.AuthorizationCode || grantType == OpenIddictConstants.GrantTypes.Implicit)
				{
					application.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Authorization);
				}

				if (grantType == OpenIddictConstants.GrantTypes.AuthorizationCode ||
					grantType == OpenIddictConstants.GrantTypes.ClientCredentials ||
					grantType == OpenIddictConstants.GrantTypes.Password ||
					grantType == OpenIddictConstants.GrantTypes.RefreshToken ||
					grantType == OpenIddictConstants.GrantTypes.DeviceCode)
				{
					application.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Token);
					application.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Revocation);
					application.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Introspection);
				}

				switch (grantType)
				{
					case OpenIddictConstants.GrantTypes.ClientCredentials:
						application.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.ClientCredentials);
						break;
					case OpenIddictConstants.GrantTypes.Implicit:
						application.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.Implicit);
						break;
					case OpenIddictConstants.GrantTypes.Password:
						application.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.Password);
						break;
					case OpenIddictConstants.GrantTypes.RefreshToken:
						application.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.RefreshToken);
						break;
					case OpenIddictConstants.GrantTypes.DeviceCode:
						application.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.DeviceCode);
						application.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.DeviceAuthorization);
						break;
				}

				if (grantType == OpenIddictConstants.GrantTypes.Implicit)
				{
					application.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.IdToken);
					if (string.Equals(type, OpenIddictConstants.ClientTypes.Public, StringComparison.OrdinalIgnoreCase))
					{
						application.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.IdTokenToken);
						application.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.Token);
					}
				}

				if (!buildInGrantTypes.Contains(grantType))
				{
					application.Permissions.Add(OpenIddictConstants.Permissions.Prefixes.GrantType + grantType);
				}
			}

			foreach (var scope in scopes)
			{
				application.Permissions.Add(scope);
			}

			if (!string.IsNullOrEmpty(redirectUri))
			{
				if (!Uri.TryCreate(redirectUri, UriKind.Absolute, out var uri) || !uri.IsWellFormedOriginalString())
				{
					throw new ArgumentException("InvalidRedirectUri");
				}

				if (application.RedirectUris.All(x => x != uri))
				{
					application.RedirectUris.Add(uri);
				}
			}

			if (!string.IsNullOrEmpty(postLogoutRedirectUri))
			{
				if (!Uri.TryCreate(postLogoutRedirectUri, UriKind.Absolute, out var uri) || !uri.IsWellFormedOriginalString())
				{
					throw new ArgumentException("InvalidPostLogoutRedirectUri");
				}

				if (application.PostLogoutRedirectUris.All(x => x != uri))
				{
					application.PostLogoutRedirectUris.Add(uri);
				}
			}


			await _applicationManager.CreateAsync(application);
		}
	}
}