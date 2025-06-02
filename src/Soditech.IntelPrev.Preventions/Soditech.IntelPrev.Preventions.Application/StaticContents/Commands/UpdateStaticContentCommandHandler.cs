using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Sensor6ty.Sessions;
using Soditech.IntelPrev.Preventions.Persistence.Models;
using Soditech.IntelPrev.Preventions.Shared.StaticContents;

namespace Soditech.IntelPrev.Preventions.Application.StaticContents.Commands;

public class UpdateStaticContentCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<UpdateStaticContentCommand, TResult<StaticContentResult>>
{
    private readonly IRepository<StaticContent> _staticContentRepository = serviceProvider.GetRequiredService<IRepository<StaticContent>>();
    private readonly ILogger<UpdateStaticContentCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<UpdateStaticContentCommandHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly IPublisher _publisher = serviceProvider.GetRequiredService<IPublisher>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();

    public async Task<TResult<StaticContentResult>> Handle(UpdateStaticContentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var staticContent = await _staticContentRepository.GetAsync(request.Id, cancellationToken);
            if (staticContent == null)
            {
                return Result.Failure<StaticContentResult>(new Error("404", "StaticContent not found"));
            }
            
            _mapper.Map(request, staticContent);
            
            staticContent.UpdaterId = _session.UserId;
            staticContent.UpdatedAt = DateTimeOffset.UtcNow;
            
            await _staticContentRepository.UpdateAsync(staticContent, cancellationToken);
            await _publisher.Publish(_mapper.Map<StaticContentUpdatedEvent>(staticContent), cancellationToken);

            
            return Result.Success(_mapper.Map<StaticContentResult>(staticContent));
        }
        catch(Exception ex)
        {
            _logger.LogError(ex , "Error while updating staticContent");

            return Result.Failure<StaticContentResult>(new Error("500", "Error while updating staticContent"));
        }
    }   
}