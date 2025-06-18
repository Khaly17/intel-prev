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

public class UpdateRegisterTypeCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<UpdateRegisterTypeCommand, TResult<RegisterTypeResult>>
{
    private readonly IRepository<RegisterType> _registerTypeRepository = serviceProvider.GetRequiredService<IRepository<RegisterType>>();
    private readonly ILogger<UpdateRegisterTypeCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<UpdateRegisterTypeCommandHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly IPublisher _publisher = serviceProvider.GetRequiredService<IPublisher>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();

    public async Task<TResult<RegisterTypeResult>> Handle(UpdateRegisterTypeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var registerType = await _registerTypeRepository.GetAsync(request.Id, cancellationToken);
            if (registerType == null)
            {
                return Result.Failure<RegisterTypeResult>(new Error("404", "RegisterType not found"));
            }
            
            _mapper.Map(request, registerType);

            registerType.UpdaterId = _session.UserId;
            registerType.UpdatedAt = DateTimeOffset.UtcNow;

            foreach (var registerField in registerType.RegisterFields)
            {
                registerField.UpdatedAt = registerType.UpdatedAt;
                registerField.UpdaterId = registerType.UpdaterId;
                registerField.TenantId = registerType.TenantId;
            }

            foreach (var registerFieldGroup in registerType.RegisterFieldGroups)
            {
                registerFieldGroup.UpdatedAt = registerType.UpdatedAt;
                registerFieldGroup.UpdaterId = registerType.UpdaterId;
                registerFieldGroup.TenantId = registerType.TenantId;

                if(registerFieldGroup.RegisterFields != null)
                {
                    foreach (var registerField in registerFieldGroup.RegisterFields)
                    {
                        registerField.UpdatedAt = registerType.UpdatedAt;
                        registerField.UpdaterId = registerType.UpdaterId;
                        registerField.TenantId = registerType.TenantId;
                    }

                }
            }


            await _registerTypeRepository.UpdateAsync(registerType, cancellationToken);
            await _publisher.Publish(_mapper.Map<RegisterTypeUpdatedEvent>(registerType), cancellationToken);

            
            return Result.Success(_mapper.Map<RegisterTypeResult>(registerType));
        }
        catch(Exception ex)
        {
            _logger.LogError(ex , "Error while updating registerType");

            return Result.Failure<RegisterTypeResult>(new Error("500", "Error while updating registerType"));
        }
    }   
}