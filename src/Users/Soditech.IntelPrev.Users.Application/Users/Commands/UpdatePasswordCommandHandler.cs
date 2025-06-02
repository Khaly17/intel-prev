using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Results;
using Soditech.IntelPrev.Users.Persistence.Models;
using Soditech.IntelPrev.Users.Shared.Users;

namespace Soditech.IntelPrev.Users.Application.Users.Commands;

public class UpdatePasswordCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<UpdatePasswordCommand, Result>
{
    private readonly UserManager<User> _userManager = serviceProvider.GetRequiredService<UserManager<User>>();
    private readonly ILogger<UpdatePasswordCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<UpdatePasswordCommandHandler>>();
    
    public async Task<Result> Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
            {
                return Result.Failure(new Error("404", "User not found"));
            }

            var passwordCheck = await _userManager.CheckPasswordAsync(user, request.OldPassword);
            if (!passwordCheck)
            {
                return Result.Failure(new Error("400", "Old password is incorrect"));
            }

            var result = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
            if (result.Succeeded)
            {
                return Result.Success();
            }

            _logger.LogError("Error while updating password: {errors}", result.Errors);
            return Result.Failure(new Error("500", "Error while updating password"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception while updating password");
            return Result.Failure(new Error("500", "Exception occurred"));
        }
    }
}