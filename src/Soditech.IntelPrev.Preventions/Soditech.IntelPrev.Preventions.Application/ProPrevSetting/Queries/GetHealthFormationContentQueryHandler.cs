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

namespace Soditech.IntelPrev.Prevensions.Application.ProPrevSetting.Queries;

public class GetHealthFormationContentQueryHandler(IServiceProvider serviceProvider) : IRequestHandler<HealthFormationContentQuery,TResult<ProPrevContentResult>>
{
    private readonly IRepository<ProPrevSettings> _proPrevSettingsRepository =
        serviceProvider.GetRequiredService<IRepository<ProPrevSettings>>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();
    private readonly ILogger<GetHealthFormationContentQueryHandler> _logger =
        serviceProvider.GetRequiredService<ILogger<GetHealthFormationContentQueryHandler>>();

    /// <inheritdoc />
    public async Task<TResult<ProPrevContentResult>> Handle(HealthFormationContentQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var proPrevSettings = await _proPrevSettingsRepository.GetAll
                .FirstOrDefaultAsync(x => x.TenantId == _session.TenantId, cancellationToken: cancellationToken);

            if (proPrevSettings == null)
            {
                return Result.Failure<ProPrevContentResult>(new Error("404","Health Formation Content not found"));
            }
            
            var result = new ProPrevContentResult
            {
                Content = proPrevSettings.HealthFormationContent,
                CreatedAt = proPrevSettings.CreatedAt,
                CreatorId = proPrevSettings.CreatorId
            };
        
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting Health Formation Content");
            return Result.Failure<ProPrevContentResult>(new Error("500","Error while getting Health Formation Content"));
        }
    }
}