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

namespace Soditech.IntelPrev.Prevensions.Application.FireSecuritySetting.Queries;


public class GetDefinitionContentQueryHandler(IServiceProvider serviceProvider): IRequestHandler<DefinitionContentQuery,TResult<FireSecuritySettingContentResult>>
{
    private readonly IRepository<FireSecuritySettings> _proPrevSettingsRepository =
        serviceProvider.GetRequiredService<IRepository<FireSecuritySettings>>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();
    private readonly ILogger<GetDefinitionContentQueryHandler> _logger =
        serviceProvider.GetRequiredService<ILogger<GetDefinitionContentQueryHandler>>();
    
    /// <inheritdoc />
    public async Task<TResult<FireSecuritySettingContentResult>> Handle(DefinitionContentQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var proPrevSettings = await _proPrevSettingsRepository.GetAll
                .FirstOrDefaultAsync(x => x.TenantId == _session.TenantId, cancellationToken: cancellationToken);

            if (proPrevSettings == null)
            {
                return Result.Failure<FireSecuritySettingContentResult>(new Error("404","Risk Analysis Protocol Content not found"));
            }
            
            var result = new FireSecuritySettingContentResult
            {
                Content = proPrevSettings.DefinitionContent,
                CreatedAt = proPrevSettings.CreatedAt,
                CreatorId = proPrevSettings.CreatorId
            };
        
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while updating Risk Analysis Protocol Content");
            return Result.Failure<FireSecuritySettingContentResult>(new Error("500","Error while updating Risk Analysis Protocol Content"));
        }
    }
}