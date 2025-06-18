using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

public class GetMedicalContactsCountQueryHandler(IServiceProvider serviceProvider) : IRequestHandler<GetMedicalContactsCountQuery, TResult<int>>
{
    private readonly IRepository<MedicalContact> _medicalContactRepository = serviceProvider.GetRequiredService<IRepository<MedicalContact>>();
    private readonly ILogger<GetMedicalContactsCountQueryHandler> _logger = serviceProvider.GetRequiredService<ILogger<GetMedicalContactsCountQueryHandler>>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();

    public async Task<TResult<int>> Handle(GetMedicalContactsCountQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var medicalContactsCount = await _medicalContactRepository.GetAll
                .Where(equipment => equipment.TenantId == _session.TenantId)
                .CountAsync(cancellationToken);
                
            return Result.Success(medicalContactsCount);
        }
        catch (Exception e)
        {
            _logger.LogError("Error while getting medicalContacts, {errors}", e.Message);
            
            return Result.Failure<int>(new Error("500", "Error while getting medicalContacts"));
        }
    }
}