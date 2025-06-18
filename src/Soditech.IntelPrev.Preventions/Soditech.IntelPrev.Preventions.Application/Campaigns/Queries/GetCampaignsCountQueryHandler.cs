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
using Soditech.IntelPrev.Prevensions.Shared.Campaigns;
using Soditech.IntelPrev.Preventions.Persistence.Models;

namespace Soditech.IntelPrev.Prevensions.Application.Campaigns.Queries;

public class GetCampaignsCountQueryHandler(IServiceProvider serviceProvider) : IRequestHandler<GetCampaignsCountQuery, TResult<int>>
{
    private readonly IRepository<Campaign> _campaignRepository = serviceProvider.GetRequiredService<IRepository<Campaign>>();
    private readonly ILogger<GetCampaignsCountQueryHandler> _logger = serviceProvider.GetRequiredService<ILogger<GetCampaignsCountQueryHandler>>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();

    public async Task<TResult<int>> Handle(GetCampaignsCountQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var campaignsCount = await _campaignRepository
                .GetAll
                .Where(campaign => campaign.TenantId == _session.TenantId)
                .CountAsync(cancellationToken);
                
            return Result.Success(campaignsCount);
        }
        catch (Exception e)
        {
            _logger.LogError("Error while getting campaigns, {errors}", e.Message);
            
            return Result.Failure<int>(new Error("500", "Error while getting campaigns"));
        }
    }
}