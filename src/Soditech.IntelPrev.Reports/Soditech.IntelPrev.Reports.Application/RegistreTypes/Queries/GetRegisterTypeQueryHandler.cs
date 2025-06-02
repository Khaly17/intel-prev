using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Soditech.IntelPrev.Reports.Persistence.Models;
using Soditech.IntelPrev.Reports.Shared.RegisterTypes;

namespace Soditech.IntelPrev.Reports.Application.RegisterTypes.Queries;

public class GetRegisterTypeQueryHandler(IServiceProvider serviceProvider)
    : IRequestHandler<GetRegisterTypeQuery, TResult<RegisterTypeResult>>
{
    private readonly IRepository<RegisterType> _registerTypeRepository = serviceProvider.GetRequiredService<IRepository<RegisterType>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    
    public async Task<TResult<RegisterTypeResult>> Handle(GetRegisterTypeQuery request, CancellationToken cancellationToken)
    {
        var registerType = await _registerTypeRepository.GetAsync(request.Id, cancellationToken);

        if (registerType == null)
        {
            return Result.Failure<RegisterTypeResult>(new Error("404", "RegisterType not found"));
        }

        var registerTypeResult = _mapper.Map<RegisterTypeResult>(registerType);

        return Result.Success(registerTypeResult);
    }
}