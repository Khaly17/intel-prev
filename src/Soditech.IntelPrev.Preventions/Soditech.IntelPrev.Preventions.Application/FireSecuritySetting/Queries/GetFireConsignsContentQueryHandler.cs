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

public class GetFireConsignsContentQueryHandler(IServiceProvider serviceProvider) : IRequestHandler<FireConsignsContentQuery,TResult<FireSecuritySettingContentResult>>
{
    private readonly IRepository<FireSecuritySettings> _proPrevSettingsRepository =
        serviceProvider.GetRequiredService<IRepository<FireSecuritySettings>>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();
    private readonly ILogger<GetFireConsignsContentQueryHandler> _logger =
        serviceProvider.GetRequiredService<ILogger<GetFireConsignsContentQueryHandler>>();

    /// <inheritdoc />
    public async Task<TResult<FireSecuritySettingContentResult>> Handle(FireConsignsContentQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var proPrevSettings = await _proPrevSettingsRepository.GetAll
                .FirstOrDefaultAsync(x => x.TenantId == _session.TenantId, cancellationToken: cancellationToken);
            
            if (proPrevSettings == null)
            {
                return Result.Failure<FireSecuritySettingContentResult>(new Error("404","Fire Consigns Content not found"));
            }
            
            var result = new FireSecuritySettingContentResult
            {
                Content = proPrevSettings.FireConsignsContent,
                CreatedAt = proPrevSettings.CreatedAt,
                CreatorId = proPrevSettings.CreatorId
            };
        
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting Fire Consigns Content");
            return Result.Failure<FireSecuritySettingContentResult>(new Error("500","Error while getting Fire Consigns Content"));
        }
    }
}