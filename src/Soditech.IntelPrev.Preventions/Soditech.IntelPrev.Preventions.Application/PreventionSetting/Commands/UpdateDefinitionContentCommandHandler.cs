using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Sensor6ty.Sessions;
using Soditech.IntelPrev.Prevensions.Shared.PreventionSetting;
using Soditech.IntelPrev.Preventions.Persistence.Models;

namespace Soditech.IntelPrev.Prevensions.Application.PreventionSetting.Commands;

public class UpdateDefinitionContentCommandHandler(IServiceProvider serviceProvider): IRequestHandler<UpdateDefinitionContentCommand, Result>
{
    private readonly IRepository<PreventionSettings> _proPrevSettingsRepository =
        serviceProvider.GetRequiredService<IRepository<PreventionSettings>>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();
    private readonly ILogger<UpdateDefinitionContentCommandHandler> _logger =
        serviceProvider.GetRequiredService<ILogger<UpdateDefinitionContentCommandHandler>>();
    
    /// <inheritdoc />
    public async Task<Result> Handle(UpdateDefinitionContentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var proPrevSettings = await _proPrevSettingsRepository.GetAll
                .FirstOrDefaultAsync(x => x.TenantId == _session.TenantId, cancellationToken: cancellationToken);
            
            if (proPrevSettings == null)
            {
                proPrevSettings = new PreventionSettings
                {
                    TenantId = _session.TenantId,
                    CreatorId = _session.UserId,
                    CreatedAt = DateTimeOffset.Now,
                    IsDefault = _session.TenantId == null
                };
                
                await _proPrevSettingsRepository.AddAsync(proPrevSettings, cancellationToken);
            }
            
            proPrevSettings.DefinitionContent = request.Content;
            proPrevSettings.UpdaterId = _session.UserId;
            proPrevSettings.UpdatedAt = DateTimeOffset.Now;
        
            await _proPrevSettingsRepository.UpdateAsync(proPrevSettings, cancellationToken);
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while updating Prevention Definition Content");
            return Result.Failure(new Error("500","Error while updating Prevention Definition Content"));
        }
    }
}