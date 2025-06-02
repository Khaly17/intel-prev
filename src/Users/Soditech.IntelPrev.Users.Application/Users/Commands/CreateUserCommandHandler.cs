using System.Reflection;
using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Results;
using Sensor6ty.System.Reflection;
using Soditech.IntelPrev.Emails.Helpers;
using Soditech.IntelPrev.Users.Application.Helpers.Models;
using Soditech.IntelPrev.Users.Persistence.Models;
using Soditech.IntelPrev.Users.Shared.Users;
using Soditech.IntelPrev.Users.Shared.Users.Events;

namespace Soditech.IntelPrev.Users.Application.Users.Commands;

public class CreateUserCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<CreateUserCommand, TResult<UserResult>>
{
    private readonly UserManager<User> _userManager = serviceProvider.GetRequiredService<UserManager<User>>();
    private readonly ILogger<CreateUserCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<CreateUserCommandHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly IPublisher _publisher = serviceProvider.GetRequiredService<IPublisher>();
    private readonly IEmailSender _emailSender = serviceProvider.GetRequiredService<IEmailSender>();
    private readonly IEmailTemplateRenderer _emailTemplateRenderer = serviceProvider.GetRequiredService<IEmailTemplateRenderer>();
    private readonly IHttpContextAccessor _httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();

    public async Task<TResult<UserResult>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            //Check if the user already exists
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user != null)
            {
                _logger.LogWarning("User with username {UserName} already exists", request.UserName);
                return Result.Success(_mapper.Map<UserResult>(user));
            }
        
            user = _mapper.Map<User>(request);
            var currentUser = _httpContextAccessor.HttpContext?.User;

            if (currentUser?.Identity?.IsAuthenticated == true)
            {
                var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var roles = currentUser.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();

                if (!roles.Contains("Super Admin"))
                {
                    var creatorUser = await _userManager.FindByIdAsync(currentUserId);
                    if (creatorUser?.TenantId != null)
                    {
                        user.TenantId = creatorUser.TenantId;
                    }
                }
            }
            var password = new Random().Next(1000, 10000).ToString();

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await _publisher.Publish(_mapper.Map<UserCreatedEvent>(user), cancellationToken);
                
                //TODO: Send a notification to the tenant or host admin

                #region send email to the user
                
                if (!string.IsNullOrEmpty(user.Email))
                {
                    var welcomeTemplateMailModel = new WelcomeTemplateMailModel
                    {
                        Password = password,
                        UserName = user.UserName!,
                        FullName = user.FullName,
                        Url = "https://intelprev.sensor6ty.com"
                    };
                    
                    var htmlBody = await _emailTemplateRenderer.RenderTemplateAsync("_WelcomeTemplateMail", welcomeTemplateMailModel);
                
                    //if htmlBody is null or empty, send msg without htmlBody
                    if (string.IsNullOrEmpty(htmlBody))
                    {
                        htmlBody = $"Votre nouveau mot de passe : {password}";
                        _logger.LogError("mail send without reset password `_WelcomeTemplate.cshtml` for template.");
                    }
                
                    await _emailSender.SendEmailAsync(user.Email, "Welcome to IntelPrev Soditech platform", htmlBody);
                }

                #endregion

                return Result.Success(_mapper.Map<UserResult>(user));
            }

            _logger.LogError("Error while creating user, {errors}", result.Errors);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cannot create user");
        }
        
        return Result.Failure<UserResult>(new Error("500", "Error while creating user"));
    }   
}