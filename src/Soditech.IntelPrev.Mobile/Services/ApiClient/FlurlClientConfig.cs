using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using Flurl.Http;
using Flurl.Http.Configuration;
using Soditech.IntelPrev.Mobile.Core.Dependency;
using Soditech.IntelPrev.Mobile.Helpers;
using Soditech.IntelPrev.Mobile.Services.Storage;

namespace Soditech.IntelPrev.Mobile.Services.ApiClient;

public static class FlurlClientConfig
{
    public static void ConfigureFlurlHttp()
    {
        FlurlHttp.Clients.WithDefaults( builder =>
        {
            builder.ConfigureHttpClient(conf =>
            {
                conf.BaseAddress = new Uri(ApiUrlConfig.BaseUrl);
                    
                conf.DefaultRequestHeaders.Add("Accept", "application/json");
                
                // to set timeout uncomment the line below
                // conf.Timeout = TimeSpan.FromSeconds(30);
               
            });

            builder.Settings.JsonSerializer = new DefaultJsonSerializer(new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            });

            var dataStorageService = DependencyResolver.GetRequiredService<IDataStorageService>();
            var authenticateResult = dataStorageService.RetrieveAuthenticateResult();
            if (authenticateResult?.AccessToken != null && !string.IsNullOrEmpty(authenticateResult.AccessToken))
            {
                builder.WithOAuthBearerToken(authenticateResult.AccessToken);
            }

            // add middleware to handle unauthorized response
            builder.AddMiddleware(() => new ReTryWithNewCredentialHandler());


#if DEBUG
            builder.ConfigureInnerHandler(conf =>
            {
                conf.ServerCertificateCustomValidationCallback = new Func<HttpRequestMessage, X509Certificate2?, X509Chain?, SslPolicyErrors, bool>((message, cert, chain, error)=> true);
            });
#endif
        });
    }

}