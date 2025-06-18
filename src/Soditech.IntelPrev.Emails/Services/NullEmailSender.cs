using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Soditech.IntelPrev.Emails.Services;

/// <summary>
/// Used to send emails in development or testing environments where actual email sending is not required.
/// </summary>
public class NullEmailSender : IEmailSender
{
    /// <inheritdoc />
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        Debug.WriteLine($"Subject: {subject}\nTo:{email}\nBody:{htmlMessage}");
        return Task.CompletedTask;
    }
}