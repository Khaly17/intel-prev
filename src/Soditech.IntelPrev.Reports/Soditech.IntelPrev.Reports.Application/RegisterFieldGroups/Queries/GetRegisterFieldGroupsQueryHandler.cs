using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Sensor6ty.Sessions;
using Soditech.IntelPrev.Reports.Persistence.Models;
using Soditech.IntelPrev.Reports.Shared.RegisterFieldGroups;

namespace Soditech.IntelPrev.Reports.Application.RegisterFieldGroups.Queries;

public class GetRegisterFieldGroupsQueryHandler(IServiceProvider serviceProvider) : IRequestHandler<GetRegisterFieldGroupsQuery, TResult<IEnumerable<RegisterFieldGroupResult>>>
{
    private readonly IRepository<RegisterFieldGroup> _registerFieldGroupRepository = serviceProvider.GetRequiredService<IRepository<RegisterFieldGroup>>();
    private readonly ILogger<GetRegisterFieldGroupsQueryHandler> _logger = serviceProvider.GetRequiredService<ILogger<GetRegisterFieldGroupsQueryHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();


    public async Task<TResult<IEnumerable<RegisterFieldGroupResult>>> Handle(GetRegisterFieldGroupsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var registerFieldGroups = await _registerFieldGroupRepository
                .GetAll
                .Where(registerFieldGroup => registerFieldGroup.TenantId == _session.TenantId)
                .ToListAsync(cancellationToken);

            var registerFieldGroupResults = _mapper.Map<IEnumerable<RegisterFieldGroupResult>>(registerFieldGroups);

            return Result.Success(registerFieldGroupResults);
        }
        catch (Exception e)
        {
            _logger.LogError("Error while getting registerFieldGroups, {errors}", e.Message);
            
            return Result.Failure<IEnumerable<RegisterFieldGroupResult>>(new Error("500", "Error while getting registerFieldGroups"));
        }
    }
}