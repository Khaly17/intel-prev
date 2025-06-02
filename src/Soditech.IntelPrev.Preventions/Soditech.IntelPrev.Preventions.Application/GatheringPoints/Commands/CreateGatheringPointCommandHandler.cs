using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Sensor6ty.Sessions;
using Soditech.IntelPrev.Preventions.Persistence.Models;
using Soditech.IntelPrev.Preventions.Shared.GatheringPoints;

namespace Soditech.IntelPrev.Preventions.Application.GatheringPoints.Commands;

public class CreateGatheringPointCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<CreateGatheringPointCommand, TResult<GatheringPointResult>>
{
    private readonly IRepository<GatheringPoint> _gatheringPointRepository = serviceProvider.GetRequiredService<IRepository<GatheringPoint>>();
    private readonly ILogger<CreateGatheringPointCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<CreateGatheringPointCommandHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();
    private readonly IPublisher _publisher = serviceProvider.GetRequiredService<IPublisher>();

    public async Task<TResult<GatheringPointResult>> Handle(CreateGatheringPointCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (_session.TenantId == Guid.Empty || _session.TenantId == null)
            {
                return Result.Failure<GatheringPointResult>(new Error("400", "cannot create gatheringPoint without a tenant"));
            }
            var gatheringPoint = _mapper.Map<GatheringPoint>(request);
            gatheringPoint.TenantId = _session.TenantId.Value;
            
            gatheringPoint.CreatorId = _session.UserId;
            gatheringPoint.CreatedAt = DateTimeOffset.UtcNow;

            await _gatheringPointRepository.AddAsync(gatheringPoint, cancellationToken);

            await _publisher.Publish(_mapper.Map<GatheringPointCreatedEvent>(gatheringPoint), cancellationToken);
            
            return Result.Success(_mapper.Map<GatheringPointResult>(gatheringPoint));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cannot create gatheringPoint");
        }
        
        return Result.Failure<GatheringPointResult>(new Error("500", "Error while creating gatheringPoint"));
    }   
}