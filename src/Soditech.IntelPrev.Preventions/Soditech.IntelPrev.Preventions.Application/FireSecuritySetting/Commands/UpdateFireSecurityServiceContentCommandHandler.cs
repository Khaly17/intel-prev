using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Sensor6ty.Sessions;
using Soditech.IntelPrev.Preventions.Persistence.Models;
using Soditech.IntelPrev.Preventions.Shared.FireSecuritySetting;

namespace Soditech.IntelPrev.Preventions.Application.FireSecuritySetting.Commands;

public class UpdateFireSecurityServiceContentCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<UpdateFireSecurityServiceContentCommand, Result>
{
    private readonly IRepository<FireSecuritySettings> _proPrevSettingsRepository =
        serviceProvider.GetRequiredService<IRepository<FireSecuritySettings>>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();
    private readonly ILogger<UpdateFireSecurityServiceContentCommandHandler> _logger =
        serviceProvider.GetRequiredService<ILogger<UpdateFireSecurityServiceContentCommandHandler>>();

    /// <inheritdoc />
    public async Task<Result> Handle(UpdateFireSecurityServiceContentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var proPrevSettings = await _proPrevSettingsRepository.GetAll
                .FirstOrDefaultAsync(x => x.TenantId == _session.TenantId, cancellationToken: cancellationToken);
            
            if (proPrevSettings == null)
            {
                proPrevSettings = new FireSecuritySettings
                {
                    TenantId = _session.TenantId,
                    CreatorId = _session.UserId,
                    CreatedAt = DateTimeOffset.Now,
                    IsDefault = _session.TenantId == null
                };
                
                await _proPrevSettingsRepository.AddAsync(proPrevSettings, cancellationToken);
            }
            
            proPrevSettings.FireSecurityServiceContent = request.Content;
            proPrevSettings.UpdaterId = _session.UserId;
            proPrevSettings.UpdatedAt = DateTimeOffset.Now;
        
            await _proPrevSettingsRepository.UpdateAsync(proPrevSettings, cancellationToken);
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while updating Actions Organizer Content");
            return Result.Failure(new Error("500","Error while updating Actions Organizer Content"));
        }
    }
}