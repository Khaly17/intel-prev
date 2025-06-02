using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Sensor6ty.Sessions;
using Soditech.IntelPrev.Reports.Shared.RegisterFieldGroups;
using Soditech.IntelPrev.Reports.Persistence.Models;

namespace Soditech.IntelPrev.Reports.Application.RegisterFieldGroups.Commands;


public class CreateRegisterFieldGroupCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<CreateRegisterFieldGroupCommand, TResult<RegisterFieldGroupResult>>
{
    private readonly IRepository<RegisterFieldGroup> _registerFieldGroupRepository = serviceProvider.GetRequiredService<IRepository<RegisterFieldGroup>>();
    private readonly ILogger<CreateRegisterFieldGroupCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<CreateRegisterFieldGroupCommandHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();
    private readonly IPublisher _publisher = serviceProvider.GetRequiredService<IPublisher>();

    public async Task<TResult<RegisterFieldGroupResult>> Handle(CreateRegisterFieldGroupCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (_session.TenantId == Guid.Empty || _session.TenantId == null)
            {
                return Result.Failure<RegisterFieldGroupResult>(new Error("400", "cannot create registerFieldGroup without a tenant"));
            }
            var registerFieldGroup = _mapper.Map<RegisterFieldGroup>(request);
            registerFieldGroup.TenantId = _session.TenantId.Value;
            
            registerFieldGroup.CreatorId = _session.UserId;
            registerFieldGroup.CreatedAt = DateTimeOffset.UtcNow;

            await _registerFieldGroupRepository.AddAsync(registerFieldGroup, cancellationToken);

            await _publisher.Publish(_mapper.Map<RegisterFieldGroupCreatedEvent>(registerFieldGroup), cancellationToken);
            
            return Result.Success(_mapper.Map<RegisterFieldGroupResult>(registerFieldGroup));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cannot create registerFieldGroup");
        }
        
        return Result.Failure<RegisterFieldGroupResult>(new Error("500", "Error while creating registerFieldGroup"));
    }   
}