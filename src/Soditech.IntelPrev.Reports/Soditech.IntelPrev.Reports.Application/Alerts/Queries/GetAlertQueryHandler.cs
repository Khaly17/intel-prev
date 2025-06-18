using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Soditech.IntelPrev.Reports.Persistence.Models;
using Soditech.IntelPrev.Reports.Shared.Alerts;

namespace Soditech.IntelPrev.Reports.Application.Alerts.Queries;

public class GetAlertQueryHandler(IServiceProvider serviceProvider)
    : IRequestHandler<GetAlertQuery, TResult<AlertResult>>
{
    private readonly IRepository<Alert> _alertRepository = serviceProvider.GetRequiredService<IRepository<Alert>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    
    public async Task<TResult<AlertResult>> Handle(GetAlertQuery request, CancellationToken cancellationToken)
    {
        var alert = await _alertRepository.GetAsync(request.Id, cancellationToken);

        if (alert == null)
        {
            return Result.Failure<AlertResult>(new Error("404", "Alert not found"));
        }

        var alertResult = _mapper.Map<AlertResult>(alert);

        return Result.Success(alertResult);
    }
}