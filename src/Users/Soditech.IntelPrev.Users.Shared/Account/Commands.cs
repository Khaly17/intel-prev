using MediatR;
using Sensor6ty.Results;
using System.Text.Json.Serialization;

namespace Soditech.IntelPrev.Users.Shared.Account;

public record WebLoginCommand: IRequest<TResult<WebLoginCommandResult>>
{
    //TODO: review the properties
    [JsonPropertyName("email")]
    public string Email { get; set; } = default!;
    [JsonPropertyName("password")]
    public string Password { get; set; } = default!;
    
    // [JsonPropertyName("tenantName")]
    // public string Tenant { get; set; } = default!;
}

public record WebLoginCommandResult
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = string.Empty;
    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; set; } = string.Empty;
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }
}