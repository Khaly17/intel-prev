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
using Soditech.IntelPrev.Prevensions.Shared.MedicalContacts;
using Soditech.IntelPrev.Preventions.Persistence.Models;

namespace Soditech.IntelPrev.Prevensions.Application.MedicalContacts.Commands;

public class CreateMedicalContactCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<CreateMedicalContactCommand, TResult<MedicalContactResult>>
{
    private readonly IRepository<MedicalContact> _medicalContactRepository = serviceProvider.GetRequiredService<IRepository<MedicalContact>>();
    private readonly ILogger<CreateMedicalContactCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<CreateMedicalContactCommandHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();
    private readonly IPublisher _publisher = serviceProvider.GetRequiredService<IPublisher>();

    public async Task<TResult<MedicalContactResult>> Handle(CreateMedicalContactCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (_session.TenantId == Guid.Empty || _session.TenantId == null)
            {
                return Result.Failure<MedicalContactResult>(new Error("400", "cannot create medicalContact without a tenant"));
            }
            var medicalContact = _mapper.Map<MedicalContact>(request);
            medicalContact.TenantId = _session.TenantId.Value;
            
            medicalContact.CreatorId = _session.UserId;
            medicalContact.CreatedAt = DateTimeOffset.UtcNow;

            await _medicalContactRepository.AddAsync(medicalContact, cancellationToken);

            await _publisher.Publish(_mapper.Map<MedicalContactCreatedEvent>(medicalContact), cancellationToken);
            
            return Result.Success(_mapper.Map<MedicalContactResult>(medicalContact));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cannot create medicalContact");
        }
        
        return Result.Failure<MedicalContactResult>(new Error("500", "Error while creating medicalContact"));
    }   
}