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

public class GetActionsOrganizerContentQueryHandler(IServiceProvider serviceProvider) : IRequestHandler<ActionsOrganizerContentQuery,TResult<ProPrevContentResult>>
{
    private readonly IRepository<ProPrevSettings> _proPrevSettingsRepository =
        serviceProvider.GetRequiredService<IRepository<ProPrevSettings>>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();
    private readonly ILogger<GetActionsOrganizerContentQueryHandler> _logger =
        serviceProvider.GetRequiredService<ILogger<GetActionsOrganizerContentQueryHandler>>();

    /// <inheritdoc />
    public async Task<TResult<ProPrevContentResult>> Handle(ActionsOrganizerContentQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var proPrevSettings = await _proPrevSettingsRepository.GetAll
                .FirstOrDefaultAsync(x => x.TenantId == _session.TenantId, cancellationToken: cancellationToken);

            if (proPrevSettings == null)
            {
                return Result.Failure<ProPrevContentResult>(new Error("404","Actions Organizer Content not found"));
            }
            
            var result = new ProPrevContentResult
            {
                Content = proPrevSettings.ActionsOrganizerContent,
                CreatedAt = proPrevSettings.CreatedAt,
                CreatorId = proPrevSettings.CreatorId
            };
        
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting Actions Organizer Content");
            return Result.Failure<ProPrevContentResult>(new Error("500","Error while getting Actions Organizer Content"));
        }
    }
}