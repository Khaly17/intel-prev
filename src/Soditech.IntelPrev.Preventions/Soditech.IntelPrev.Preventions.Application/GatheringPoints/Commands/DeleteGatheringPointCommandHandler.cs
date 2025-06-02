using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Soditech.IntelPrev.Preventions.Persistence.Models;
using Soditech.IntelPrev.Preventions.Shared.GatheringPoints;

namespace Soditech.IntelPrev.Preventions.Application.GatheringPoints.Commands;

public class DeleteGatheringPointCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<DeleteGatheringPointCommand, Result>
{
    private readonly IRepository<GatheringPoint> _gatheringPointRepository = serviceProvider.GetRequiredService<IRepository<GatheringPoint>>();
    private readonly ILogger<DeleteGatheringPointCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<DeleteGatheringPointCommandHandler>>();
    private readonly IPublisher _publisher = serviceProvider.GetRequiredService<IPublisher>();

    public async Task<Result> Handle(DeleteGatheringPointCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var gatheringPoint = await _gatheringPointRepository.GetAsync(request.Id, cancellationToken);
            if (gatheringPoint == null)
            {
                return Result.Failure<GatheringPointResult>(new Error("404", "GatheringPoint not found"));
            }
            
            await _gatheringPointRepository.DeleteAsync(gatheringPoint, cancellationToken);
            
            await _publisher.Publish(new GatheringPointDeletedEvent(request.Id), cancellationToken);

            return Result.Success();
        }
        catch(Exception ex)
        {
            _logger.LogError(ex , "Error while deleting gatheringPoint");

            return Result.Failure(new Error("500", "Error while deleting gatheringPoint"));
        }
    }   
}