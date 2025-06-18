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

public class CreateEquipmentCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<CreateEquipmentCommand, TResult<EquipmentResult>>
{
    private readonly IRepository<Equipment> _equipmentRepository = serviceProvider.GetRequiredService<IRepository<Equipment>>();
    private readonly ILogger<CreateEquipmentCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<CreateEquipmentCommandHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();
    private readonly IPublisher _publisher = serviceProvider.GetRequiredService<IPublisher>();

    public async Task<TResult<EquipmentResult>> Handle(CreateEquipmentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (_session.TenantId == Guid.Empty || _session.TenantId == null)
            {
                return Result.Failure<EquipmentResult>(new Error("400", "cannot create equipment without a tenant"));
            }
            var equipment = _mapper.Map<Equipment>(request);
            equipment.TenantId = _session.TenantId.Value;
            
            equipment.CreatorId = _session.UserId;
            equipment.CreatedAt = DateTimeOffset.UtcNow;

            await _equipmentRepository.AddAsync(equipment, cancellationToken);

            await _publisher.Publish(_mapper.Map<EquipmentCreatedEvent>(equipment), cancellationToken);
            
            return Result.Success(_mapper.Map<EquipmentResult>(equipment));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cannot create equipment");
        }
        
        return Result.Failure<EquipmentResult>(new Error("500", "Error while creating equipment"));
    }   
}