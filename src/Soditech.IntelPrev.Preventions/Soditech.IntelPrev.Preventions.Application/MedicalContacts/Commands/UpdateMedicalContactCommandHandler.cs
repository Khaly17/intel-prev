using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Sensor6ty.Sessions;
using Soditech.IntelPrev.Preventions.Persistence.Models;
using Soditech.IntelPrev.Preventions.Shared.MedicalContacts;

namespace Soditech.IntelPrev.Preventions.Application.MedicalContacts.Commands;

public class UpdateMedicalContactCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<UpdateMedicalContactCommand, TResult<MedicalContactResult>>
{
    private readonly IRepository<MedicalContact> _medicalContactRepository = serviceProvider.GetRequiredService<IRepository<MedicalContact>>();
    private readonly ILogger<UpdateMedicalContactCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<UpdateMedicalContactCommandHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly IPublisher _publisher = serviceProvider.GetRequiredService<IPublisher>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();

    public async Task<TResult<MedicalContactResult>> Handle(UpdateMedicalContactCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var medicalContact = await _medicalContactRepository.GetAsync(request.Id, cancellationToken);
            if (medicalContact == null)
            {
                return Result.Failure<MedicalContactResult>(new Error("404", "MedicalContact not found"));
            }
            
            _mapper.Map(request, medicalContact);
            
            medicalContact.UpdaterId = _session.UserId;
            medicalContact.UpdatedAt = DateTimeOffset.UtcNow;
            
            await _medicalContactRepository.UpdateAsync(medicalContact, cancellationToken);
            await _publisher.Publish(_mapper.Map<MedicalContactUpdatedEvent>(medicalContact), cancellationToken);

            
            return Result.Success(_mapper.Map<MedicalContactResult>(medicalContact));
        }
        catch(Exception ex)
        {
            _logger.LogError(ex , "Error while updating medicalContact");

            return Result.Failure<MedicalContactResult>(new Error("500", "Error while updating medicalContact"));
        }
    }   
}