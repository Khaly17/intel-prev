using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Sensor6ty.Sessions;
using Soditech.IntelPrev.Prevensions.Shared.GatheringPoints;
using Soditech.IntelPrev.Preventions.Persistence.Models;

namespace Soditech.IntelPrev.Prevensions.Application.GatheringPoints.Commands;

public class UpdateGatheringPointCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<UpdateGatheringPointCommand, TResult<GatheringPointResult>>
{
    private readonly IRepository<GatheringPoint> _gatheringPointRepository = serviceProvider.GetRequiredService<IRepository<GatheringPoint>>();
    private readonly ILogger<UpdateGatheringPointCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<UpdateGatheringPointCommandHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly IPublisher _publisher = serviceProvider.GetRequiredService<IPublisher>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();

    public async Task<TResult<GatheringPointResult>> Handle(UpdateGatheringPointCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var gatheringPoint = await _gatheringPointRepository.GetAsync(request.Id, cancellationToken);
            if (gatheringPoint == null)
            {
                return Result.Failure<GatheringPointResult>(new Error("404", "GatheringPoint not found"));
            }
            
            _mapper.Map(request, gatheringPoint);
            
            gatheringPoint.UpdaterId = _session.UserId;
            gatheringPoint.UpdatedAt = DateTimeOffset.UtcNow;
            
            await _gatheringPointRepository.UpdateAsync(gatheringPoint, cancellationToken);
            await _publisher.Publish(_mapper.Map<GatheringPointUpdatedEvent>(gatheringPoint), cancellationToken);

            
            return Result.Success(_mapper.Map<GatheringPointResult>(gatheringPoint));
        }
        catch(Exception ex)
        {
            _logger.LogError(ex , "Error while updating gatheringPoint");

            return Result.Failure<GatheringPointResult>(new Error("500", "Error while updating gatheringPoint"));
        }
    }   
}