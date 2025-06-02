using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Sensor6ty.Sessions;
using Soditech.IntelPrev.Reports.Persistence.Models;
using Soditech.IntelPrev.Reports.Shared.RegisterFields;

namespace Soditech.IntelPrev.Reports.Application.RegisterFields.Commands;

public class UpdateRegisterFieldCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<UpdateRegisterFieldCommand, TResult<RegisterFieldResult>>
{
    private readonly IRepository<RegisterField> _registerFieldRepository = serviceProvider.GetRequiredService<IRepository<RegisterField>>();
    private readonly ILogger<UpdateRegisterFieldCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<UpdateRegisterFieldCommandHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly IPublisher _publisher = serviceProvider.GetRequiredService<IPublisher>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();

    public async Task<TResult<RegisterFieldResult>> Handle(UpdateRegisterFieldCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var registerField = await _registerFieldRepository.GetAsync(request.Id, cancellationToken);
            if (registerField == null)
            {
                return Result.Failure<RegisterFieldResult>(new Error("404", "RegisterField not found"));
            }
            
            _mapper.Map(request, registerField);
            
            registerField.UpdaterId = _session.UserId;
            registerField.UpdatedAt = DateTimeOffset.UtcNow;
            
            await _registerFieldRepository.UpdateAsync(registerField, cancellationToken);
            await _publisher.Publish(_mapper.Map<RegisterFieldUpdatedEvent>(registerField), cancellationToken);

            
            return Result.Success(_mapper.Map<RegisterFieldResult>(registerField));
        }
        catch(Exception ex)
        {
            _logger.LogError(ex , "Error while updating registerField");

            return Result.Failure<RegisterFieldResult>(new Error("500", "Error while updating registerField"));
        }
    }   
}