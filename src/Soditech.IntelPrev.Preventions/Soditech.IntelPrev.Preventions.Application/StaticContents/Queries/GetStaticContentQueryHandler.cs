using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Soditech.IntelPrev.Prevensions.Shared.StaticContents;
using Soditech.IntelPrev.Preventions.Persistence.Models;

namespace Soditech.IntelPrev.Prevensions.Application.StaticContents.Queries;

public class GetStaticContentQueryHandler(IServiceProvider serviceProvider)
    : IRequestHandler<GetStaticContentQuery, TResult<StaticContentResult>>
{
    private readonly IRepository<StaticContent> _staticContentRepository = serviceProvider.GetRequiredService<IRepository<StaticContent>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();

    
    public async Task<TResult<StaticContentResult>> Handle(GetStaticContentQuery request, CancellationToken cancellationToken)
    {
        var staticContent = await _staticContentRepository.GetAsync(request.Id, cancellationToken);

        if (staticContent == null)
        {
            return Result.Failure<StaticContentResult>(new Error("404", "StaticContent not found"));
        }

        var staticContentResult = _mapper.Map<StaticContentResult>(staticContent);

        return Result.Success(staticContentResult);
    }
}