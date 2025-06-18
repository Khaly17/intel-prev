using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Sensor6ty.Sessions;
using Soditech.IntelPrev.Prevensions.Shared.MedicalContacts;
using Soditech.IntelPrev.Preventions.Persistence.Models;

namespace Soditech.IntelPrev.Prevensions.Application.MedicalContacts.Queries;

public class GetMedicalContactsQueryHandler(IServiceProvider serviceProvider) : IRequestHandler<GetMedicalContactsQuery, TResult<IEnumerable<MedicalContactResult>>>
{
    private readonly IRepository<MedicalContact> _medicalContactRepository = serviceProvider.GetRequiredService<IRepository<MedicalContact>>();
    private readonly ILogger<GetMedicalContactsQueryHandler> _logger = serviceProvider.GetRequiredService<ILogger<GetMedicalContactsQueryHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();


    public async Task<TResult<IEnumerable<MedicalContactResult>>> Handle(GetMedicalContactsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var medicalContacts = await _medicalContactRepository
                .GetAll
                .Where(equipment => equipment.TenantId == _session.TenantId)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var medicalContactResults = _mapper.Map<List<MedicalContactResult>>(medicalContacts);

            return Result.Success<IEnumerable<MedicalContactResult>>(medicalContactResults);
        }
        catch (Exception e)
        {
            _logger.LogError("Error while getting medicalContacts, {errors}", e.Message);
            
            return Result.Failure<IEnumerable<MedicalContactResult>>(new Error("500", "Error while getting medicalContacts"));
        }
    }
}