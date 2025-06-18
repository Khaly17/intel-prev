using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Soditech.IntelPrev.Prevensions.Shared.Equipments;
using Soditech.IntelPrev.Preventions.Persistence.Models;

namespace Soditech.IntelPrev.Prevensions.Application.Equipments.Queries;

public class GetEquipmentQueryHandler(IServiceProvider serviceProvider)
    : IRequestHandler<GetEquipmentQuery, TResult<EquipmentResult>>
{
    private readonly IRepository<Equipment> _equipmentRepository = serviceProvider.GetRequiredService<IRepository<Equipment>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    
    public async Task<TResult<EquipmentResult>> Handle(GetEquipmentQuery request, CancellationToken cancellationToken)
    {
        var equipment = await _equipmentRepository.GetAsync(request.Id, cancellationToken);

        if (equipment == null)
        {
            return Result.Failure<EquipmentResult>(new Error("404", "Equipment not found"));
        }

        var equipmentResult = _mapper.Map<EquipmentResult>(equipment);

        return Result.Success(equipmentResult);
    }
}