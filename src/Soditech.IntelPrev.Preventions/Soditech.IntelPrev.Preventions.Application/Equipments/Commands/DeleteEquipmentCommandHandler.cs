using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Soditech.IntelPrev.Prevensions.Shared.Equipments;
using Soditech.IntelPrev.Preventions.Persistence.Models;

namespace Soditech.IntelPrev.Prevensions.Application.Equipments.Commands;

public class DeleteEquipmentCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<DeleteEquipmentCommand, Result>
{
    private readonly IRepository<Equipment> _equipmentRepository = serviceProvider.GetRequiredService<IRepository<Equipment>>();
    private readonly ILogger<DeleteEquipmentCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<DeleteEquipmentCommandHandler>>();
    private readonly IPublisher _publisher = serviceProvider.GetRequiredService<IPublisher>();

    public async Task<Result> Handle(DeleteEquipmentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var equipment = await _equipmentRepository.GetAsync(request.Id, cancellationToken);
            if (equipment == null)
            {
                return Result.Failure<EquipmentResult>(new Error("404", "Equipment not found"));
            }
            
            await _equipmentRepository.DeleteAsync(equipment, cancellationToken);
            
            await _publisher.Publish(new EquipmentDeletedEvent(request.Id), cancellationToken);

            return Result.Success();
        }
        catch(Exception ex)
        {
            _logger.LogError(ex , "Error while deleting equipment");

            return Result.Failure(new Error("500", "Error while deleting equipment"));
        }
    }   
}