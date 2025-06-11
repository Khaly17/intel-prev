namespace Soditech.IntelPrev.Emails.Services.Options;

/// <summary>
/// In order to use EWS, you need to create an Azure AD application with the following permissions:
/// mail.send, and user.read.
/// In the appsettings.json file, you need to add the following properties:
/// `EwsOptions:TenantId`, `EwsOptions:ClientId`, `EwsOptions:ClientSecret`, and `EwsOptions:Username`.
/// </summary>
public class EwsOptions
{
    public required string TenantId { get; set; }
    public required string ClientId { get; set; }
    public required string ClientSecret { get; set; }
    public required string Username { get; set; }
}