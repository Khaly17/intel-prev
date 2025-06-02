using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Sensor6ty.Sessions;
using Soditech.IntelPrev.Preventions.Persistence.Models;
using Soditech.IntelPrev.Preventions.Shared.FireSecuritySetting;

namespace Soditech.IntelPrev.Preventions.Application.FireSecuritySetting.Queries;

public class GetEvacuationCaseContentQueryHandler(IServiceProvider serviceProvider) : IRequestHandler<EvacuationCaseContentQuery,TResult<FireSecuritySettingContentResult>>
{
    private readonly IRepository<FireSecuritySettings> _proPrevSettingsRepository =
        serviceProvider.GetRequiredService<IRepository<FireSecuritySettings>>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();
    private readonly ILogger<GetEvacuationCaseContentQueryHandler> _logger =
        serviceProvider.GetRequiredService<ILogger<GetEvacuationCaseContentQueryHandler>>();

    /// <inheritdoc />
    public async Task<TResult<FireSecuritySettingContentResult>> Handle(EvacuationCaseContentQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var proPrevSettings = await _proPrevSettingsRepository.GetAll
                .FirstOrDefaultAsync(x => x.TenantId == _session.TenantId, cancellationToken: cancellationToken);

            if (proPrevSettings == null)
            {
                return Result.Failure<FireSecuritySettingContentResult>(new Error("404","Evacuation Case Content not found"));
            }
            
            var result = new FireSecuritySettingContentResult
            {
                Content = proPrevSettings.EvacuationCaseContent,
                CreatedAt = proPrevSettings.CreatedAt,
                CreatorId = proPrevSettings.CreatorId
            };
        
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting Evacuation Case Content");
            return Result.Failure<FireSecuritySettingContentResult>(new Error("500","Error while getting Evacuation Case Content"));
        }
    }
}