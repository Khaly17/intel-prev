using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Sensor6ty.Sessions;
using Soditech.IntelPrev.Reports.Shared.RegisterFields;
using Soditech.IntelPrev.Reports.Persistence.Models;

namespace Soditech.IntelPrev.Reports.Application.RegisterFields.Commands;


public class CreateRegisterFieldCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<CreateRegisterFieldCommand, TResult<RegisterFieldResult>>
{
    private readonly IRepository<RegisterField> _registerFieldRepository = serviceProvider.GetRequiredService<IRepository<RegisterField>>();
    private readonly ILogger<CreateRegisterFieldCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<CreateRegisterFieldCommandHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();
    private readonly IPublisher _publisher = serviceProvider.GetRequiredService<IPublisher>();

    public async Task<TResult<RegisterFieldResult>> Handle(CreateRegisterFieldCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (_session.TenantId == Guid.Empty || _session.TenantId == null)
            {
                return Result.Failure<RegisterFieldResult>(new Error("400", "cannot create registerField without a tenant"));
            }
            var registerField = _mapper.Map<RegisterField>(request);
            registerField.TenantId = _session.TenantId.Value;
            
            registerField.CreatorId = _session.UserId;
            registerField.CreatedAt = DateTimeOffset.UtcNow;

            await _registerFieldRepository.AddAsync(registerField, cancellationToken);

            await _publisher.Publish(_mapper.Map<RegisterFieldCreatedEvent>(registerField), cancellationToken);
            
            return Result.Success(_mapper.Map<RegisterFieldResult>(registerField));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cannot create registerField");
        }
        
        return Result.Failure<RegisterFieldResult>(new Error("500", "Error while creating registerField"));
    }   
}