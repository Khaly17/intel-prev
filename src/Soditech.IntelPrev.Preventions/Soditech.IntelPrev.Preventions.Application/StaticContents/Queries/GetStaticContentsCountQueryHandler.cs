using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Sensor6ty.Sessions;
using Soditech.IntelPrev.Preventions.Persistence.Models;
using Soditech.IntelPrev.Preventions.Shared.StaticContents;

namespace Soditech.IntelPrev.Preventions.Application.Equipments.Queries;

public class GetStaticContentsCountQueryHandler(IServiceProvider serviceProvider) : IRequestHandler<GetStaticContentsCountQuery, TResult<int>>
{
    private readonly IRepository<StaticContent> _staticContentRepository = serviceProvider.GetRequiredService<IRepository<StaticContent>>();
    private readonly ILogger<GetStaticContentsCountQueryHandler> _logger = serviceProvider.GetRequiredService<ILogger<GetStaticContentsCountQueryHandler>>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();

    public async Task<TResult<int>> Handle(GetStaticContentsCountQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var staticContentsCount = await _staticContentRepository.GetAll
                .Where(equipment => equipment.TenantId == _session.TenantId)
                .CountAsync(cancellationToken);
                
            return Result.Success(staticContentsCount);
        }
        catch (Exception e)
        {
            _logger.LogError("Error while getting staticContents, {errors}", e.Message);
            
            return Result.Failure<int>(new Error("500", "Error while getting staticContents"));
        }
    }
}