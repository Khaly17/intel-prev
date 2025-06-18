using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Sensor6ty.Sessions;
using Soditech.IntelPrev.Prevensions.Shared.StaticContents;
using Soditech.IntelPrev.Preventions.Persistence.Models;

namespace Soditech.IntelPrev.Prevensions.Application.StaticContents.Commands;

public class CreateStaticContentCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<CreateStaticContentCommand, TResult<StaticContentResult>>
{
    private readonly IRepository<StaticContent> _staticContentRepository = serviceProvider.GetRequiredService<IRepository<StaticContent>>();
    private readonly ILogger<CreateStaticContentCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<CreateStaticContentCommandHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();
    private readonly IPublisher _publisher = serviceProvider.GetRequiredService<IPublisher>();

    public async Task<TResult<StaticContentResult>> Handle(CreateStaticContentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (_session.TenantId == Guid.Empty || _session.TenantId == null)
            {
                return Result.Failure<StaticContentResult>(new Error("400", "cannot create staticContent without a tenant"));
            }
            var staticContent = _mapper.Map<StaticContent>(request);
            staticContent.TenantId = _session.TenantId.Value;
            
            staticContent.CreatorId = _session.UserId;
            staticContent.CreatedAt = DateTimeOffset.UtcNow;

            await _staticContentRepository.AddAsync(staticContent, cancellationToken);

            await _publisher.Publish(_mapper.Map<StaticContentCreatedEvent>(staticContent), cancellationToken);
            
            return Result.Success(_mapper.Map<StaticContentResult>(staticContent));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cannot create staticContent");
        }
        
        return Result.Failure<StaticContentResult>(new Error("500", "Error while creating staticContent"));
    }   
}