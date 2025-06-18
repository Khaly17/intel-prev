using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Soditech.IntelPrev.Prevensions.Shared.MedicalContacts;
using Soditech.IntelPrev.Preventions.Persistence.Models;

namespace Soditech.IntelPrev.Prevensions.Application.MedicalContacts.Commands;

public class DeleteMedicalContactCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<DeleteMedicalContactCommand, Result>
{
    private readonly IRepository<MedicalContact> _medicalContactRepository = serviceProvider.GetRequiredService<IRepository<MedicalContact>>();
    private readonly ILogger<DeleteMedicalContactCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<DeleteMedicalContactCommandHandler>>();
    private readonly IPublisher _publisher = serviceProvider.GetRequiredService<IPublisher>();

    public async Task<Result> Handle(DeleteMedicalContactCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var medicalContact = await _medicalContactRepository.GetAsync(request.Id, cancellationToken);
            if (medicalContact == null)
            {
                return Result.Failure<MedicalContactResult>(new Error("404", "MedicalContact not found"));
            }
            
            await _medicalContactRepository.DeleteAsync(medicalContact, cancellationToken);
            
            await _publisher.Publish(new MedicalContactDeletedEvent(request.Id), cancellationToken);

            return Result.Success();
        }
        catch(Exception ex)
        {
            _logger.LogError(ex , "Error while deleting medicalContact");

            return Result.Failure(new Error("500", "Error while deleting medicalContact"));
        }
    }   
}