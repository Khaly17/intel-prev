using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Soditech.IntelPrev.Prevensions.Shared.StaticContents;
using Soditech.IntelPrev.Preventions.Persistence.Models;

namespace Soditech.IntelPrev.Prevensions.Application.StaticContents.Commands;

public class DeleteStaticContentCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<DeleteStaticContentCommand, Result>
{
    private readonly IRepository<StaticContent> _staticContentRepository = serviceProvider.GetRequiredService<IRepository<StaticContent>>();
    private readonly ILogger<DeleteStaticContentCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<DeleteStaticContentCommandHandler>>();
    private readonly IPublisher _publisher = serviceProvider.GetRequiredService<IPublisher>();

    public async Task<Result> Handle(DeleteStaticContentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var staticContent = await _staticContentRepository.GetAsync(request.Id, cancellationToken);
            if (staticContent == null)
            {
                return Result.Failure<StaticContentResult>(new Error("404", "StaticContent not found"));
            }
            
            await _staticContentRepository.DeleteAsync(staticContent, cancellationToken);
            
            await _publisher.Publish(new StaticContentDeletedEvent(request.Id), cancellationToken);

            return Result.Success();
        }
        catch(Exception ex)
        {
            _logger.LogError(ex , "Error while deleting staticContent");

            return Result.Failure(new Error("500", "Error while deleting staticContent"));
        }
    }   
}