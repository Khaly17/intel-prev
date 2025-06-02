using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Sensor6ty.Sessions;
using Soditech.IntelPrev.Preventions.Persistence.Models;
using Soditech.IntelPrev.Preventions.Shared.ProPrevSetting;

namespace Soditech.IntelPrev.Preventions.Application.ProPrevSetting.Commands;

public class UpdateAnalysisToolsContentCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<UpdateAnalysisToolsContentCommand, Result>
{
    private readonly IRepository<ProPrevSettings> _proPrevSettingsRepository =
        serviceProvider.GetRequiredService<IRepository<ProPrevSettings>>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();
    private readonly ILogger<UpdateRiskAnalysisProtocolContentCommandHandler> _logger =
        serviceProvider.GetRequiredService<ILogger<UpdateRiskAnalysisProtocolContentCommandHandler>>();

    /// <inheritdoc />
    public async Task<Result> Handle(UpdateAnalysisToolsContentCommand request, CancellationToken cancellationToken)
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


            proPrevSettings.AnalysisToolsContent = request.Content;
            proPrevSettings.UpdaterId = _session.UserId;
            proPrevSettings.UpdatedAt = DateTimeOffset.Now;
        
            await _proPrevSettingsRepository.UpdateAsync(proPrevSettings, cancellationToken);
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while updating Risk Analysis Protocol Content");
            return Result.Failure(new Error("500","Error while updating Risk Analysis Protocol Content"));
        }
    }
}