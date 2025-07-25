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

namespace Soditech.IntelPrev.Prevensions.Application.PreventionSetting.Queries;

public class GetSensibilisationContentQueryHandler(IServiceProvider serviceProvider) : IRequestHandler<SensibilisationContentQuery,TResult<PreventionContentResult>>
{
    private readonly IRepository<PreventionSettings> _proPrevSettingsRepository =
        serviceProvider.GetRequiredService<IRepository<PreventionSettings>>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();
    private readonly ILogger<GetSensibilisationContentQueryHandler> _logger =
        serviceProvider.GetRequiredService<ILogger<GetSensibilisationContentQueryHandler>>();

    /// <inheritdoc />
    public async Task<TResult<PreventionContentResult>> Handle(SensibilisationContentQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var proPrevSettings = await _proPrevSettingsRepository.GetAll
                .FirstOrDefaultAsync(x => x.TenantId == _session.TenantId, cancellationToken: cancellationToken);
            
            if (proPrevSettings == null)
            {
                return Result.Failure<PreventionContentResult>(new Error("404","Sensibilisation Content not found"));
            }
            
            var result = new PreventionContentResult
            {
                Content = proPrevSettings.SensibilisationContent,
                CreatedAt = proPrevSettings.CreatedAt,
                CreatorId = proPrevSettings.CreatorId
            };
        
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting Sensibilisation Content");
            return Result.Failure<PreventionContentResult>(new Error("500","Error while getting Sensibilisation Content"));
        }
    }
}