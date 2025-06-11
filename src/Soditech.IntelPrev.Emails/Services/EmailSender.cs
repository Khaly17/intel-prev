using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Soditech.IntelPrev.Emails.Services.Options;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Soditech.IntelPrev.Emails.Services;


public class EmailSender : IEmailSender
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<EmailSender> _logger;
    private readonly EwsOptions _ewsOptions;

    public EmailSender(ILogger<EmailSender> logger, IOptions<EwsOptions> ewsOptions)
    {
        _logger = logger;
        _httpClient = new HttpClient();
        _ewsOptions = ewsOptions.Value;

        var token = GetAccessToken(_ewsOptions.TenantId, _ewsOptions.ClientId, _ewsOptions.ClientSecret).Result;

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
    

    private static async Task<string> GetAccessToken(string tenantId, string clientId, string clientSecret)
    {
        var app = ConfidentialClientApplicationBuilder.Create(clientId)
            .WithClientSecret(clientSecret)
            .WithTenantId(tenantId)
            .WithAuthority(new Uri($"https://login.microsoftonline.com/{tenantId}"))
            .Build();

        var scopes = new[] { "https://graph.microsoft.com/.default" };
        var result = await app.AcquireTokenForClient(scopes).ExecuteAsync();

        return result.AccessToken;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {

        var emailObj = new
        {
            Message = new
            {
                Subject = subject,
                Body = new
                {
                    ContentType = "HTML",
                    Content = htmlMessage
                },
                ToRecipients = new[]
                {
                    new { EmailAddress = new { Address = email } }
                },
            },
            SaveToSentItems = "false"
        };


        try
        {
            var jsonContent = JsonSerializer.Serialize(emailObj);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"https://graph.microsoft.com/v1.0/users/{_ewsOptions.Username}/sendMail", content);

            if (response.StatusCode != HttpStatusCode.Accepted)
            {
                _logger.LogError("Cannot Send mail Status Code :{status}", response.StatusCode);
                var responseBody = await response.Content.ReadAsStringAsync();
                _logger.LogError("Cannot Send mail Response :{response}", responseBody);
            }

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while sending email");
        }
    }
}