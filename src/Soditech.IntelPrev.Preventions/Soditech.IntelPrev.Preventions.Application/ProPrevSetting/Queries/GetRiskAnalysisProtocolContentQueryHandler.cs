using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Sensor6ty.Sessions;
using Soditech.IntelPrev.Preventions.Persistence.Models;
using Soditech.IntelPrev.Preventions.Shared.ProPrevSetting;

namespace Soditech.IntelPrev.Preventions.Application.ProPrevSetting.Queries;


public class GetRiskAnalysisProtocolContentQueryHandler(IServiceProvider serviceProvider): IRequestHandler<RiskAnalysisProtocolContentQuery,TResult<ProPrevContentResult>>
{
    private readonly IRepository<ProPrevSettings> _proPrevSettingsRepository =
        serviceProvider.GetRequiredService<IRepository<ProPrevSettings>>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();
    private readonly ILogger<GetRiskAnalysisProtocolContentQueryHandler> _logger =
        serviceProvider.GetRequiredService<ILogger<GetRiskAnalysisProtocolContentQueryHandler>>();
    
    /// <inheritdoc />
    public async Task<TResult<ProPrevContentResult>> Handle(RiskAnalysisProtocolContentQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var proPrevSettings = await _proPrevSettingsRepository.GetAll
                .FirstOrDefaultAsync(x => x.TenantId == _session.TenantId, cancellationToken: cancellationToken);

            if (proPrevSettings == null)
            {
                return Result.Failure<ProPrevContentResult>(new Error("404","Risk Analysis Protocol Content not found"));
            }
            
            var result = new ProPrevContentResult
            {
                Content = proPrevSettings.RiskAnalysisProtocolContent,
                CreatedAt = proPrevSettings.CreatedAt,
                CreatorId = proPrevSettings.CreatorId
            };
        
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while updating Risk Analysis Protocol Content");
            return Result.Failure<ProPrevContentResult>(new Error("500","Error while updating Risk Analysis Protocol Content"));
        }
    }
}