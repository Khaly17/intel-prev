using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Soditech.IntelPrev.Prevensions.Shared.MedicalContacts;
using Soditech.IntelPrev.Preventions.Persistence.Models;

namespace Soditech.IntelPrev.Prevensions.Application.MedicalContacts.Queries;

public class GetMedicalContactQueryHandler(IServiceProvider serviceProvider)
    : IRequestHandler<GetMedicalContactQuery, TResult<MedicalContactResult>>
{
    private readonly IRepository<MedicalContact> _medicalContactRepository = serviceProvider.GetRequiredService<IRepository<MedicalContact>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();

    
    public async Task<TResult<MedicalContactResult>> Handle(GetMedicalContactQuery request, CancellationToken cancellationToken)
    {
        var medicalContact = await _medicalContactRepository.GetAsync(request.Id, cancellationToken);

        if (medicalContact == null)
        {
            return Result.Failure<MedicalContactResult>(new Error("404", "MedicalContact not found"));
        }

        var medicalContactResult = _mapper.Map<MedicalContactResult>(medicalContact);

        return Result.Success(medicalContactResult);
    }
}