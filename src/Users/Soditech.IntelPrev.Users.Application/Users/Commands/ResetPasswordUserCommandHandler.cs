using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Results;
using Soditech.IntelPrev.Emails.Helpers;
using Soditech.IntelPrev.Users.Application.Helpers.Models;
using Soditech.IntelPrev.Users.Persistence.Models;
using Soditech.IntelPrev.Users.Shared.Users;
using System.Security.Cryptography;

namespace Soditech.IntelPrev.Users.Application.Users.Commands;

public class ResetPasswordUserCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<ResetPasswordCommand, Result>
{
    private readonly UserManager<User> _userManager = serviceProvider.GetRequiredService<UserManager<User>>();
    private readonly ILogger<ResetPasswordUserCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<ResetPasswordUserCommandHandler>>();
    private readonly IEmailTemplateRenderer _emailTemplateRenderer = serviceProvider.GetRequiredService<IEmailTemplateRenderer>();
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
    private readonly IEmailSender _emailSender = serviceProvider.GetRequiredService<IEmailSender>();
    
    public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        try
        {
            //Check if the user already exists
            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            if (user == null) 
                return Result.Failure(new Error("500", "User password cannot be reset"));


            if (!string.IsNullOrEmpty(user.Email))
            {
                var newPassword = GenerateSecureCode();

                // update user password
                var result = await _userManager.RemovePasswordAsync(user);
                if (!result.Succeeded)
                {
                    _logger.LogError("User password cannot be reset: {error}", result.Errors);
                    return Result.Failure(new Error("500", "User password cannot be reset"));
                }

                result = await _userManager.AddPasswordAsync(user, newPassword);
                if (!result.Succeeded)
                {
                    _logger.LogError("User password cannot be reset: {errors}", result.Errors);
                    return Result.Failure(new Error("500", "User password cannot be reset"));
                }
                
                #region send email to the user

                var resetPasswordTemplateMailModel = new ResetPasswordTemplateMailModel
                {
                    Password = newPassword,
                    UserName = user.UserName!,
                    FullName = user.FullName!,
                    Url = "https://intelprev.sensor6ty.com"
                };
                
                var htmlBody = await _emailTemplateRenderer.RenderTemplateAsync("Helpers/Templates/_ResetPasswordTemplateMail.html", resetPasswordTemplateMailModel);
                
                //if htmlBody is null or empty, send msg without htmlBody
                if (string.IsNullOrEmpty(htmlBody))
                {
                    htmlBody = $"Votre nouveau mot de passe : {newPassword}";
                    _logger.LogError("mail send without reset password `_ResetPasswordTemplateMail.cshtml` for template.");
                }

                
                await _emailSender.SendEmailAsync(user.Email, "IntelPrev Soditech réinitialisation de mot de passe", htmlBody);

                #endregion
                 
                return Result.Success();
            }

            //TODO: tenant or host admin to inform him that the user has no email
            await _emailSender.SendEmailAsync("mg.univ@sensor6ty.com",
                "IntelPrev Soditech réinitialisation de mot de passe",
                $"The user {user.FullName} identified by {user.UserName} has no email.");

            // return failure
            _logger.LogWarning("User password cannot be reset: user has not a email address");
            return Result.Failure(new Error("400", "User password cannot be reset"));

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "User password cannot be reset");
            return Result.Failure(new Error("500", "User password cannot be reset"));
        }

    }   

    private string GenerateSecureCode(int length = 4)
    {
        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[length];
        rng.GetBytes(bytes);

        var digits = bytes.Select(b => (b % 10).ToString());
        return string.Concat(digits);
    }

}