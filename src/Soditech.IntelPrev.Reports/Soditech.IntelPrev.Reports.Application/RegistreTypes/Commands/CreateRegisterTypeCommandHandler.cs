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
using Soditech.IntelPrev.Reports.Persistence.Models;
using Soditech.IntelPrev.Reports.Shared.RegisterTypes;

namespace Soditech.IntelPrev.Reports.Application.RegistreTypes.Commands;


public class CreateRegisterTypeCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<CreateRegisterTypeCommand, TResult<RegisterTypeResult>>
{
    private readonly IRepository<RegisterType> _registerTypeRepository = serviceProvider.GetRequiredService<IRepository<RegisterType>>();
    private readonly ILogger<CreateRegisterTypeCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<CreateRegisterTypeCommandHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();
    private readonly IPublisher _publisher = serviceProvider.GetRequiredService<IPublisher>();

    public async Task<TResult<RegisterTypeResult>> Handle(CreateRegisterTypeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (_session.TenantId == Guid.Empty || _session.TenantId == null)
            {
                return Result.Failure<RegisterTypeResult>(new Error("400", "cannot create registerType without a tenant"));
            }
            var registerType = _mapper.Map<RegisterType>(request);
            registerType.TenantId = _session.TenantId.Value;
            registerType.Id = Guid.NewGuid();

            registerType.CreatorId = _session.UserId;
            registerType.CreatedAt = DateTimeOffset.UtcNow;

            foreach (var registerField in registerType.RegisterFields)
            {
                registerField.CreatedAt = registerType.CreatedAt;
                registerField.CreatorId = registerType.CreatorId;
                registerField.TenantId = registerType.TenantId;
            }

            foreach (var registerFieldGroup in registerType.RegisterFieldGroups)
            {
                registerFieldGroup.CreatedAt = registerType.CreatedAt;
                registerFieldGroup.CreatorId = registerType.CreatorId;
                registerFieldGroup.TenantId = registerType.TenantId;

                foreach (var registerField in registerFieldGroup.RegisterFields)
                {
                    registerField.CreatedAt = registerType.CreatedAt;
                    registerField.CreatorId = registerType.CreatorId;
                    registerField.TenantId = registerType.TenantId;
                    registerField.RegisterTypeId = registerType.Id;
                }
            }

            await _registerTypeRepository.AddAsync(registerType, cancellationToken);

            await _publisher.Publish(_mapper.Map<RegisterTypeCreatedEvent>(registerType), cancellationToken);

            return Result.Success(_mapper.Map<RegisterTypeResult>(registerType));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cannot create registerType");
        }

        return Result.Failure<RegisterTypeResult>(new Error("500", "Error while creating registerType"));
    }
}
