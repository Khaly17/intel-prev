using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Soditech.IntelPrev.Reports.Persistence.Models;
using Soditech.IntelPrev.Reports.Shared.RegisterTypes;

namespace Soditech.IntelPrev.Reports.Application.RegistreTypes.Commands;

public class DeleteRegisterTypeCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<DeleteRegisterTypeCommand, Result>
{
    private readonly IRepository<RegisterType> _registerTypeRepository = serviceProvider.GetRequiredService<IRepository<RegisterType>>();
    private readonly ILogger<DeleteRegisterTypeCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<DeleteRegisterTypeCommandHandler>>();
    private readonly IPublisher _publisher = serviceProvider.GetRequiredService<IPublisher>();

    public async Task<Result> Handle(DeleteRegisterTypeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var registerType = await _registerTypeRepository.GetAsync(request.Id, cancellationToken);
            if (registerType == null)
            {
                return Result.Failure<RegisterTypeResult>(new Error("404", "RegisterType not found"));
            }
            
            await _registerTypeRepository.DeleteAsync(registerType, cancellationToken);
            
            await _publisher.Publish(new RegisterTypeDeletedEvent(request.Id), cancellationToken);

            return Result.Success();
        }
        catch(Exception ex)
        {
            _logger.LogError(ex , "Error while deleting registerType");

            return Result.Failure(new Error("500", "Error while deleting registerType"));
        }
    }   
}