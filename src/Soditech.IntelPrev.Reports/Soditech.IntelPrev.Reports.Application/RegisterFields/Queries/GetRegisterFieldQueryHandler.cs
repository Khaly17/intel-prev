using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Soditech.IntelPrev.Reports.Persistence.Models;
using Soditech.IntelPrev.Reports.Shared.RegisterFields;

namespace Soditech.IntelPrev.Reports.Application.RegisterFields.Queries;

public class GetRegisterFieldQueryHandler(IServiceProvider serviceProvider)
    : IRequestHandler<GetRegisterFieldQuery, TResult<RegisterFieldResult>>
{
    private readonly IRepository<RegisterField> _registerFieldRepository = serviceProvider.GetRequiredService<IRepository<RegisterField>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    
    public async Task<TResult<RegisterFieldResult>> Handle(GetRegisterFieldQuery request, CancellationToken cancellationToken)
    {
        var registerField = await _registerFieldRepository.GetAsync(request.Id, cancellationToken);

        if (registerField == null)
        {
            return Result.Failure<RegisterFieldResult>(new Error("404", "RegisterField not found"));
        }

        var registerFieldResult = _mapper.Map<RegisterFieldResult>(registerField);

        return Result.Success(registerFieldResult);
    }
}