using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Soditech.IntelPrev.Reports.Persistence.Models;
using Soditech.IntelPrev.Reports.Shared.RegisterFields;

namespace Soditech.IntelPrev.Reports.Application.RegisterFields.Commands;

public class DeleteRegisterFieldCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<DeleteRegisterFieldCommand, Result>
{
    private readonly IRepository<RegisterField> _registerFieldRepository = serviceProvider.GetRequiredService<IRepository<RegisterField>>();
    private readonly ILogger<DeleteRegisterFieldCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<DeleteRegisterFieldCommandHandler>>();
    private readonly IPublisher _publisher = serviceProvider.GetRequiredService<IPublisher>();

    public async Task<Result> Handle(DeleteRegisterFieldCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var registerField = await _registerFieldRepository.GetAsync(request.Id, cancellationToken);
            if (registerField == null)
            {
                return Result.Failure<RegisterFieldResult>(new Error("404", "RegisterField not found"));
            }
            
            await _registerFieldRepository.DeleteAsync(registerField, cancellationToken);
            
            await _publisher.Publish(new RegisterFieldDeletedEvent(request.Id), cancellationToken);

            return Result.Success();
        }
        catch(Exception ex)
        {
            _logger.LogError(ex , "Error while deleting registerField");

            return Result.Failure(new Error("500", "Error while deleting registerField"));
        }
    }   
}