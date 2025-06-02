namespace Soditech.IntelPrev.Emails.Services.Options;

public class EwsOptions
{
    public required string TenantId { get; set; }
    public required string ClientId { get; set; }
    public required string ClientSecret { get; set; }
    public required string Username { get; set; }
}