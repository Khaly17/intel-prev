using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Sensor6ty.Sessions;
using Soditech.IntelPrev.Prevensions.Shared.Enums;
using Soditech.IntelPrev.Prevensions.Shared.GeoLocations;
using Soditech.IntelPrev.Preventions.Persistence.Extensions.EfCore;
using Soditech.IntelPrev.Preventions.Persistence.Models;

namespace Soditech.IntelPrev.Prevensions.Application.GeoLocations.Queries;

public class GetLocationsByTypeQueryHandler(IServiceProvider serviceProvider)
    : IRequestHandler<GetLocationsByTypeQuery, TResult<IEnumerable<GeoLocationResult>>>
{
    private readonly IRepository<GeoLocation> _geoLocationRepository = serviceProvider.GetRequiredService<IRepository<GeoLocation>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();
    
    public async Task<TResult<IEnumerable<GeoLocationResult>>> Handle(GetLocationsByTypeQuery request, CancellationToken cancellationToken)
    {
        var isTypeValid = Enum.TryParse<GeoLocationType>(request.Type, out var type);
        if (!isTypeValid)
        {
            return Result.Failure<IEnumerable<GeoLocationResult>>(new Error("400", "Invalid type"));
        }
        
        var geoLocations = await _geoLocationRepository
            .GetAll
            .Where(geo => geo.TenantId == _session.TenantId)
            .Where(geo => geo.Type == type)
            .WhereIf(request.BuildingId != null, geo => geo.BuildingId == request.BuildingId)
            .WhereIf(request.FloorId != null, geo => geo.FloorId == request.FloorId)
            .ToListAsync(cancellationToken);
        

        var equipmentResult = _mapper.Map<IEnumerable<GeoLocationResult>>(geoLocations);

        return Result.Success(equipmentResult);
    }
}