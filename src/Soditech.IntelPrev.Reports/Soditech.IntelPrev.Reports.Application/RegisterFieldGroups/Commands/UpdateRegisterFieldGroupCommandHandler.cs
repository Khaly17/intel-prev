using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Sensor6ty.Sessions;
using Soditech.IntelPrev.Reports.Persistence.Models;
using Soditech.IntelPrev.Reports.Shared.RegisterFieldGroups;

namespace Soditech.IntelPrev.Reports.Application.RegisterFieldGroups.Commands;

public class UpdateRegisterFieldGroupCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<UpdateRegisterFieldGroupCommand, TResult<RegisterFieldGroupResult>>
{
    private readonly IRepository<RegisterFieldGroup> _registerFieldGroupRepository = serviceProvider.GetRequiredService<IRepository<RegisterFieldGroup>>();
    private readonly ILogger<UpdateRegisterFieldGroupCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<UpdateRegisterFieldGroupCommandHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly IPublisher _publisher = serviceProvider.GetRequiredService<IPublisher>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();

    public async Task<TResult<RegisterFieldGroupResult>> Handle(UpdateRegisterFieldGroupCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var registerFieldGroup = await _registerFieldGroupRepository.GetAsync(request.Id, cancellationToken);
            if (registerFieldGroup == null)
            {
                return Result.Failure<RegisterFieldGroupResult>(new Error("404", "RegisterFieldGroup not found"));
            }
            
            _mapper.Map(request, registerFieldGroup);
            
            registerFieldGroup.UpdaterId = _session.UserId;
            registerFieldGroup.UpdatedAt = DateTimeOffset.UtcNow;
            
            await _registerFieldGroupRepository.UpdateAsync(registerFieldGroup, cancellationToken);
            await _publisher.Publish(_mapper.Map<RegisterFieldGroupUpdatedEvent>(registerFieldGroup), cancellationToken);

            
            return Result.Success(_mapper.Map<RegisterFieldGroupResult>(registerFieldGroup));
        }
        catch(Exception ex)
        {
            _logger.LogError(ex , "Error while updating registerFieldGroup");

            return Result.Failure<RegisterFieldGroupResult>(new Error("500", "Error while updating registerFieldGroup"));
        }
    }   
}