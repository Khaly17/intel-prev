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
using Soditech.IntelPrev.Reports.Shared.RegisterTypes;

namespace Soditech.IntelPrev.Reports.Application.RegistreTypes.Queries;

public class GetRegisterTypesCountQueryHandler(IServiceProvider serviceProvider) : IRequestHandler<GetRegisterTypesCountQuery, TResult<int>>
{
    private readonly IRepository<RegisterType> _registerTypeRepository = serviceProvider.GetRequiredService<IRepository<RegisterType>>();
    private readonly ILogger<GetRegisterTypesCountQueryHandler> _logger = serviceProvider.GetRequiredService<ILogger<GetRegisterTypesCountQueryHandler>>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();

    public async Task<TResult<int>> Handle(GetRegisterTypesCountQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var registerTypesCount = await _registerTypeRepository
                .GetAll
                .Where(registerType => registerType.TenantId == _session.TenantId)
                .CountAsync(cancellationToken);
                
            return Result.Success(registerTypesCount);
        }
        catch (Exception e)
        {
            _logger.LogError("Error while getting registerTypes, {errors}", e.Message);
            
            return Result.Failure<int>(new Error("500", "Error while getting registerTypes"));
        }
    }
}