using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Sensor6ty.Sessions;
using Soditech.IntelPrev.Reports.Persistence.Models;
using Soditech.IntelPrev.Reports.Shared.RegisterTypes;

namespace Soditech.IntelPrev.Reports.Application.RegisterTypes.Queries;

public class GetRegisterTypesQueryHandler(IServiceProvider serviceProvider) : IRequestHandler<GetRegisterTypesQuery, TResult<IEnumerable<RegisterTypeResult>>>
{
    private readonly IRepository<RegisterType> _registerTypeRepository = serviceProvider.GetRequiredService<IRepository<RegisterType>>();
    private readonly ILogger<GetRegisterTypesQueryHandler> _logger = serviceProvider.GetRequiredService<ILogger<GetRegisterTypesQueryHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();


    public async Task<TResult<IEnumerable<RegisterTypeResult>>> Handle(GetRegisterTypesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var registerTypes = await _registerTypeRepository
                .GetAll
                .Where(registerType => registerType.TenantId == _session.TenantId)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var registerTypeResults = _mapper.Map<IEnumerable<RegisterTypeResult>>(registerTypes);

            return Result.Success(registerTypeResults);
        }
        catch (Exception e)
        {
            _logger.LogError("Error while getting registerTypes, {errors}", e.Message);
            
            return Result.Failure<IEnumerable<RegisterTypeResult>>(new Error("500", "Error while getting registerTypes"));
        }
    }
}