using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Sensor6ty.Sessions;
using Soditech.IntelPrev.Reports.Persistence.Models;
using Soditech.IntelPrev.Reports.Shared.RegisterFields;

namespace Soditech.IntelPrev.Reports.Application.RegisterFields.Queries;

public class GetRegisterFieldsCountQueryHandler(IServiceProvider serviceProvider) : IRequestHandler<GetRegisterFieldsCountQuery, TResult<int>>
{
    private readonly IRepository<RegisterField> _registerFieldRepository = serviceProvider.GetRequiredService<IRepository<RegisterField>>();
    private readonly ILogger<GetRegisterFieldsCountQueryHandler> _logger = serviceProvider.GetRequiredService<ILogger<GetRegisterFieldsCountQueryHandler>>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();

    public async Task<TResult<int>> Handle(GetRegisterFieldsCountQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var registerFieldsCount = await _registerFieldRepository
                .GetAll
                .Where(registerField => registerField.TenantId == _session.TenantId)
                .CountAsync(cancellationToken);
                
            return Result.Success(registerFieldsCount);
        }
        catch (Exception e)
        {
            _logger.LogError("Error while getting registerFields, {errors}", e.Message);
            
            return Result.Failure<int>(new Error("500", "Error while getting registerFields"));
        }
    }
}