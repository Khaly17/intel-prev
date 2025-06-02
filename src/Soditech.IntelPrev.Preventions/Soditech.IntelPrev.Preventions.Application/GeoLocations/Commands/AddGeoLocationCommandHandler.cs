using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Sensor6ty.Sessions;
using Soditech.IntelPrev.Prevensions.Shared.GeoLocations;
using Soditech.IntelPrev.Preventions.Persistence.Models;

namespace Soditech.IntelPrev.Preventions.Application.GeoLocations.Commands;

public class AddGeoLocationCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<AddGeoLocationCommand, Result>
{
    private readonly IRepository<GeoLocation> _geoLocationRepository = serviceProvider.GetRequiredService<IRepository<GeoLocation>>();
    private readonly ILogger<AddGeoLocationCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<AddGeoLocationCommandHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();

    public async Task<Result> Handle(AddGeoLocationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (_session.TenantId == Guid.Empty || _session.TenantId == null)
            {
                return Result.Failure(new Error("400", "cannot create geoLocation without a tenant"));
            }
            var geoLocation = _mapper.Map<GeoLocation>(request);
            geoLocation.TenantId = _session.TenantId.Value;
            
            geoLocation.CreatorId = _session.UserId;
            geoLocation.CreatedAt = DateTimeOffset.UtcNow;

            await _geoLocationRepository.AddAsync(geoLocation, cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cannot create geoLocation");
        }
        
        return Result.Failure(new Error("500", "Error while creating geoLocation"));
    }   
}