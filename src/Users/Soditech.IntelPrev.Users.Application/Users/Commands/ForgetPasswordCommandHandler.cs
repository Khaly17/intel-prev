using System;
using System.Linq;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Sensor6ty.Results;
using Soditech.IntelPrev.Emails.Helpers;
using Soditech.IntelPrev.Users.Application.Helpers.Models;
using Soditech.IntelPrev.Users.Persistence.Models;
using Soditech.IntelPrev.Users.Shared.Users;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Soditech.IntelPrev.Users.Application.Users.Commands;

public class ForgetPasswordCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<ForgetPasswordCommand, Result>
{
    private readonly UserManager<User> _userManager = serviceProvider.GetRequiredService<UserManager<User>>();
    private readonly ILogger<ForgetPasswordCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<ForgetPasswordCommandHandler>>();
    private readonly IEmailTemplateRenderer _emailTemplateRenderer = serviceProvider.GetRequiredService<IEmailTemplateRenderer>();
    private readonly IEmailSender _emailSender = serviceProvider.GetRequiredService<IEmailSender>();

    
    public async Task<Result> Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                _logger.LogWarning("Reset password failed: user with username {UserName} not found.", request.UserName);
                return Result.Failure(new Error("404", "Utilisateur introuvable."));
            }

            var newPassword = GenerateSecureCode();

            var removePasswordResult = await _userManager.RemovePasswordAsync(user);
            if (!removePasswordResult.Succeeded)
            {
                _logger.LogError("Failed to remove password for user {UserName}: {Errors}", request.UserName, removePasswordResult.Errors);
                return Result.Failure(new Error("500", "Échec de la réinitialisation du mot de passe."));
            }

            var addPasswordResult = await _userManager.AddPasswordAsync(user, newPassword);
            if (!addPasswordResult.Succeeded)
            {
                _logger.LogError("Failed to add new password for user {UserName}: {Errors}", request.UserName, addPasswordResult.Errors);
                return Result.Failure(new Error("500", "Échec de la réinitialisation du mot de passe."));
            }

            if (!string.IsNullOrEmpty(user.Email))
            {
                var welcomeTemplateMailModel = new ResetPasswordTemplateMailModel
                {
                    Password = newPassword,
                    UserName = user.UserName!,
                    FullName = user.FullName!,
                    Url = "https://intelprev.sensor6ty.com"
                };
                
                var htmlBody = await _emailTemplateRenderer.RenderTemplateAsync("Helpers/Templates/_ResetPasswordTemplateMail.html", welcomeTemplateMailModel);
                
                //if htmlBody is null or empty, send msg without htmlBody
                if (string.IsNullOrEmpty(htmlBody))
                {
                    htmlBody = $"Votre nouveau mot de passe : {newPassword}";
                    _logger.LogError("mail send without reset password `_ResetPasswordTemplate.cshtml` for template.");
                }
                
                await _emailSender.SendEmailAsync(user.Email, "IntelPrev Soditech réinitialisation de mot de passe", htmlBody);
             
                return Result.Success();
            }

            _logger.LogWarning("Reset password failed: user {UserName} has no email.", request.UserName);
            return Result.Failure(new Error("400", "Utilisateur sans adresse email."));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to reset password for user {UserName}.", request.UserName);
            return Result.Failure(new Error("500", "Une erreur interne est survenue."));
        }
    }


    private static string GenerateSecureCode(int length = 4)
    {
        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[length];
        rng.GetBytes(bytes);

        var digits = bytes.Select(b => (b % 10).ToString());
        return string.Concat(digits);
    }

}