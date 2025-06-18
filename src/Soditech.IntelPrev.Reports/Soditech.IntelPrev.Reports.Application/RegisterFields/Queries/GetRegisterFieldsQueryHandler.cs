using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
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

public class GetRegisterFieldsQueryHandler(IServiceProvider serviceProvider) : IRequestHandler<GetRegisterFieldsQuery, TResult<IEnumerable<RegisterFieldResult>>>
{
    private readonly IRepository<RegisterField> _registerFieldRepository = serviceProvider.GetRequiredService<IRepository<RegisterField>>();
    private readonly ILogger<GetRegisterFieldsQueryHandler> _logger = serviceProvider.GetRequiredService<ILogger<GetRegisterFieldsQueryHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();


    public async Task<TResult<IEnumerable<RegisterFieldResult>>> Handle(GetRegisterFieldsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var registerFields = await _registerFieldRepository
                .GetAll
                .Where(registerField => registerField.TenantId == _session.TenantId)
                .ToListAsync(cancellationToken);

            var registerFieldResults = _mapper.Map<IEnumerable<RegisterFieldResult>>(registerFields);

            return Result.Success(registerFieldResults);
        }
        catch (Exception e)
        {
            _logger.LogError("Error while getting registerFields, {errors}", e.Message);
            
            return Result.Failure<IEnumerable<RegisterFieldResult>>(new Error("500", "Error while getting registerFields"));
        }
    }
}