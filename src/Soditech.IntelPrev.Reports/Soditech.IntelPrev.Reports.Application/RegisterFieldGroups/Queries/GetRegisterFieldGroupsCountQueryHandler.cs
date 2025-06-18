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
using Soditech.IntelPrev.Reports.Persistence.Models;
using Soditech.IntelPrev.Reports.Shared.RegisterFielGroups;

namespace Soditech.IntelPrev.Reports.Application.RegisterFieldGroups.Queries;

public class GetRegisterFieldGroupsCountQueryHandler(IServiceProvider serviceProvider) : IRequestHandler<GetRegisterFieldGroupsCountQuery, TResult<int>>
{
    private readonly IRepository<RegisterFieldGroup> _registerFieldGroupRepository = serviceProvider.GetRequiredService<IRepository<RegisterFieldGroup>>();
    private readonly ILogger<GetRegisterFieldGroupsCountQueryHandler> _logger = serviceProvider.GetRequiredService<ILogger<GetRegisterFieldGroupsCountQueryHandler>>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();

    public async Task<TResult<int>> Handle(GetRegisterFieldGroupsCountQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var registerFieldGroupsCount = await _registerFieldGroupRepository
                .GetAll
                .Where(registerFieldGroup => registerFieldGroup.TenantId == _session.TenantId)
                .CountAsync(cancellationToken);
                
            return Result.Success(registerFieldGroupsCount);
        }
        catch (Exception e)
        {
            _logger.LogError("Error while getting registerFieldGroups, {errors}", e.Message);
            
            return Result.Failure<int>(new Error("500", "Error while getting registerFieldGroups"));
        }
    }
}