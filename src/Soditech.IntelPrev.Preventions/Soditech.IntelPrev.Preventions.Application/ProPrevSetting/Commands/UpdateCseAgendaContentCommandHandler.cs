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
using Soditech.IntelPrev.Prevensions.Shared.ProPrevSetting;
using Soditech.IntelPrev.Preventions.Persistence.Models;

namespace Soditech.IntelPrev.Prevensions.Application.ProPrevSetting.Commands;

public class UpdateCseAgendaContentCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<UpdateCseAgendaContentCommand, Result>
{
    private readonly IRepository<ProPrevSettings> _proPrevSettingsRepository =
        serviceProvider.GetRequiredService<IRepository<ProPrevSettings>>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();
    private readonly ILogger<UpdateCseAgendaContentCommandHandler> _logger =
        serviceProvider.GetRequiredService<ILogger<UpdateCseAgendaContentCommandHandler>>();

    /// <inheritdoc />
    public async Task<Result> Handle(UpdateCseAgendaContentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var proPrevSettings = await _proPrevSettingsRepository.GetAll
                .FirstOrDefaultAsync(x => x.TenantId == _session.TenantId, cancellationToken: cancellationToken);
            
             
            if (proPrevSettings == null)
            {
                proPrevSettings = new ProPrevSettings
                {
                    TenantId = _session.TenantId,
                    CreatorId = _session.UserId,
                    CreatedAt = DateTimeOffset.Now,
                    IsDefault = _session.TenantId == null
                };
                
                await _proPrevSettingsRepository.AddAsync(proPrevSettings, cancellationToken);
            }


            proPrevSettings.CseAgendaContent = request.Content;
            proPrevSettings.UpdaterId = _session.UserId;
            proPrevSettings.UpdatedAt = DateTimeOffset.Now;
        
            await _proPrevSettingsRepository.UpdateAsync(proPrevSettings, cancellationToken);
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while updating Cse Agenda Content");
            return Result.Failure(new Error("500","Error while updating Cse Agenda Content"));
        }
    }
}