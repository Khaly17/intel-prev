using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Soditech.IntelPrev.Reports.Persistence.Models;
using Soditech.IntelPrev.Reports.Shared.RegisterFieldGroups;

namespace Soditech.IntelPrev.Reports.Application.RegisterFieldGroups.Commands;

public class DeleteRegisterFieldGroupCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<DeleteRegisterFieldGroupCommand, Result>
{
    private readonly IRepository<RegisterFieldGroup> _registerFieldGroupRepository = serviceProvider.GetRequiredService<IRepository<RegisterFieldGroup>>();
    private readonly ILogger<DeleteRegisterFieldGroupCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<DeleteRegisterFieldGroupCommandHandler>>();
    private readonly IPublisher _publisher = serviceProvider.GetRequiredService<IPublisher>();

    public async Task<Result> Handle(DeleteRegisterFieldGroupCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var registerFieldGroup = await _registerFieldGroupRepository.GetAsync(request.Id, cancellationToken);
            if (registerFieldGroup == null)
            {
                return Result.Failure<RegisterFieldGroupResult>(new Error("404", "RegisterFieldGroup not found"));
            }
            
            await _registerFieldGroupRepository.DeleteAsync(registerFieldGroup, cancellationToken);
            
            await _publisher.Publish(new RegisterFieldGroupDeletedEvent(request.Id), cancellationToken);

            return Result.Success();
        }
        catch(Exception ex)
        {
            _logger.LogError(ex , "Error while deleting registerFieldGroup");

            return Result.Failure(new Error("500", "Error while deleting registerFieldGroup"));
        }
    }   
}