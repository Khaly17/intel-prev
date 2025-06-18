using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Sensor6ty.Sessions;
using Soditech.IntelPrev.Prevensions.Shared.Equipments;
using Soditech.IntelPrev.Preventions.Persistence.Models;

namespace Soditech.IntelPrev.Prevensions.Application.Equipments.Queries;

public class GetEquipmentsCountQueryHandler(IServiceProvider serviceProvider) : IRequestHandler<GetEquipmentsCountQuery, TResult<int>>
{
    private readonly IRepository<Equipment> _equipmentRepository = serviceProvider.GetRequiredService<IRepository<Equipment>>();
    private readonly ILogger<GetEquipmentsCountQueryHandler> _logger = serviceProvider.GetRequiredService<ILogger<GetEquipmentsCountQueryHandler>>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();

    public async Task<TResult<int>> Handle(GetEquipmentsCountQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var equipmentsCount = await _equipmentRepository
                .GetAll
                .Where(equipment => equipment.TenantId == _session.TenantId)
                .CountAsync(cancellationToken);
                
            return Result.Success(equipmentsCount);
        }
        catch (Exception e)
        {
            _logger.LogError("Error while getting equipments, {errors}", e.Message);
            
            return Result.Failure<int>(new Error("500", "Error while getting equipments"));
        }
    }
}