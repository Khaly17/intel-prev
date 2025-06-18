using System;
using System.Text.Json.Serialization;

namespace Soditech.IntelPrev.Mobile.Services.Account.Models;

public class AuthenticateResultModel
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }

    public string EncryptedAccessToken { get; set; }

    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; set; }
    
    
    [JsonPropertyName("expires_in")]
    public int ExpireInSeconds { get; set; }

    public bool ShouldResetPassword { get; set; }

    public Guid UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    
    public DateTime RefreshTokenExpireDate { get; set; }
}

public class AuthenticateModel
{
    public string Email { get; set; }

    public string Password { get; set; }

    public string AppVersion { get; set; } = string.Empty;

}

public class UserInfoModel
{
    public string UserId { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}