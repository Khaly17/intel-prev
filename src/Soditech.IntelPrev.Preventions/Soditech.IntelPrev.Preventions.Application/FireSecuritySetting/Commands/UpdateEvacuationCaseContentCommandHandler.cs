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
using Soditech.IntelPrev.Prevensions.Shared.FireSecuritySetting;
using Soditech.IntelPrev.Preventions.Persistence.Models;

namespace Soditech.IntelPrev.Prevensions.Application.FireSecuritySetting.Commands;

public class UpdateEvacuationCaseContentCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<UpdateEvacuationCaseContentCommand, Result>
{
    private readonly IRepository<FireSecuritySettings> _proPrevSettingsRepository =
        serviceProvider.GetRequiredService<IRepository<FireSecuritySettings>>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();
    private readonly ILogger<UpdateEvacuationCaseContentCommandHandler> _logger =
        serviceProvider.GetRequiredService<ILogger<UpdateEvacuationCaseContentCommandHandler>>();

    /// <inheritdoc />
    public async Task<Result> Handle(UpdateEvacuationCaseContentCommand request, CancellationToken cancellationToken)
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
            
            proPrevSettings.EvacuationCaseContent = request.Content;
            proPrevSettings.UpdaterId = _session.UserId;
            proPrevSettings.UpdatedAt = DateTimeOffset.Now;
        
            await _proPrevSettingsRepository.UpdateAsync(proPrevSettings, cancellationToken);
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while updating Epi Control Content");
            return Result.Failure(new Error("500","Error while updating Epi Control Content"));
        }
    }
}