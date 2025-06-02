using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Sensor6ty.Sessions;
using Soditech.IntelPrev.Preventions.Persistence.Models;
using Soditech.IntelPrev.Preventions.Shared.Equipments;

namespace Soditech.IntelPrev.Preventions.Application.Equipments.Queries;

public class GetEquipmentsQueryHandler(IServiceProvider serviceProvider) : IRequestHandler<GetEquipmentsQuery, TResult<IEnumerable<EquipmentResult>>>
{
    private readonly IRepository<Equipment> _equipmentRepository = serviceProvider.GetRequiredService<IRepository<Equipment>>();
    private readonly ILogger<GetEquipmentsQueryHandler> _logger = serviceProvider.GetRequiredService<ILogger<GetEquipmentsQueryHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();


    public async Task<TResult<IEnumerable<EquipmentResult>>> Handle(GetEquipmentsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var equipments = await _equipmentRepository
                .GetAll
                .Where(equipment => equipment.TenantId == _session.TenantId)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var equipmentResults = _mapper.Map<List<EquipmentResult>>(equipments);

            return Result.Success<IEnumerable<EquipmentResult>>(equipmentResults);
        }
        catch (Exception e)
        {
            _logger.LogError("Error while getting equipments, {errors}", e.Message);
            
            return Result.Failure<IEnumerable<EquipmentResult>>(new Error("500", "Error while getting equipments"));
        }
    }
}