using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Soditech.IntelPrev.Reports.Persistence.Models;
using Soditech.IntelPrev.Reports.Shared.RegisterFielGroups;

namespace Soditech.IntelPrev.Reports.Application.RegisterFieldGroups.Queries;

public class GetRegisterFieldGroupQueryHandler(IServiceProvider serviceProvider)
    : IRequestHandler<GetRegisterFieldGroupQuery, TResult<RegisterFieldGroupResult>>
{
    private readonly IRepository<RegisterFieldGroup> _registerFieldGroupRepository = serviceProvider.GetRequiredService<IRepository<RegisterFieldGroup>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    
    public async Task<TResult<RegisterFieldGroupResult>> Handle(GetRegisterFieldGroupQuery request, CancellationToken cancellationToken)
    {
        var registerFieldGroup = await _registerFieldGroupRepository.GetAsync(request.Id, cancellationToken);

        if (registerFieldGroup == null)
        {
            return Result.Failure<RegisterFieldGroupResult>(new Error("404", "RegisterFieldGroup not found"));
        }

        var registerFieldGroupResult = _mapper.Map<RegisterFieldGroupResult>(registerFieldGroup);

        return Result.Success(registerFieldGroupResult);
    }
}