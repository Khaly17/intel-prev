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
using Soditech.IntelPrev.Prevensions.Shared.GeoLocations;
using Soditech.IntelPrev.Preventions.Persistence.Models;

namespace Soditech.IntelPrev.Prevensions.Application.GeoLocations.Commands;

public class UpdateGeoLocationCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<UpdateGeoLocationCommand, Result>
{
    private readonly IRepository<GeoLocation> _geoLocationRepository = serviceProvider.GetRequiredService<IRepository<GeoLocation>>();
    private readonly ILogger<UpdateGeoLocationCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<UpdateGeoLocationCommandHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();

    public async Task<Result> Handle(UpdateGeoLocationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (_session.TenantId == Guid.Empty || _session.TenantId == null)
            {
                return Result.Failure(new Error("400", "cannot update geoLocation without a tenant"));
            }
            var geoLocation = _mapper.Map<GeoLocation>(request);
            geoLocation.TenantId = _session.TenantId.Value;
            
            geoLocation.CreatorId = _session.UserId;
            geoLocation.CreatedAt = DateTimeOffset.UtcNow;

            await _geoLocationRepository.UpdateAsync(geoLocation, cancellationToken);
            
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cannot update geoLocation");
        }
        
        return Result.Failure(new Error("500", "Error while updating geoLocation"));
    }   
}