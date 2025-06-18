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
using Soditech.IntelPrev.Prevensions.Shared.Equipments;
using Soditech.IntelPrev.Preventions.Persistence.Models;

namespace Soditech.IntelPrev.Prevensions.Application.Equipments.Commands;

public class UpdateEquipmentCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<UpdateEquipmentCommand, TResult<EquipmentResult>>
{
    private readonly IRepository<Equipment> _equipmentRepository = serviceProvider.GetRequiredService<IRepository<Equipment>>();
    private readonly ILogger<UpdateEquipmentCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<UpdateEquipmentCommandHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly IPublisher _publisher = serviceProvider.GetRequiredService<IPublisher>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();

    public async Task<TResult<EquipmentResult>> Handle(UpdateEquipmentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var equipment = await _equipmentRepository.GetAsync(request.Id, cancellationToken);
            if (equipment == null)
            {
                return Result.Failure<EquipmentResult>(new Error("404", "Equipment not found"));
            }
            
            _mapper.Map(request, equipment);
            
            equipment.UpdaterId = _session.UserId;
            equipment.UpdatedAt = DateTimeOffset.UtcNow;
            
            await _equipmentRepository.UpdateAsync(equipment, cancellationToken);
            await _publisher.Publish(_mapper.Map<EquipmentUpdatedEvent>(equipment), cancellationToken);

            
            return Result.Success(_mapper.Map<EquipmentResult>(equipment));
        }
        catch(Exception ex)
        {
            _logger.LogError(ex , "Error while updating equipment");

            return Result.Failure<EquipmentResult>(new Error("500", "Error while updating equipment"));
        }
    }   
}