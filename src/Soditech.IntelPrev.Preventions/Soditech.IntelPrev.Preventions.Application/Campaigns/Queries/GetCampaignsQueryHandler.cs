using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Sensor6ty.Sessions;
using Soditech.IntelPrev.Preventions.Persistence.Models;
using Soditech.IntelPrev.Preventions.Shared.Campaigns;

namespace Soditech.IntelPrev.Preventions.Application.Campaigns.Queries;

public class GetCampaignsQueryHandler(IServiceProvider serviceProvider) : IRequestHandler<GetCampaignsQuery, TResult<IEnumerable<CampaignResult>>>
{
    private readonly IRepository<Campaign> _campaignRepository = serviceProvider.GetRequiredService<IRepository<Campaign>>();
    private readonly ILogger<GetCampaignsQueryHandler> _logger = serviceProvider.GetRequiredService<ILogger<GetCampaignsQueryHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();


    public async Task<TResult<IEnumerable<CampaignResult>>> Handle(GetCampaignsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var campaigns = await _campaignRepository
                .GetAll
                .Where(campaign => campaign.TenantId == _session.TenantId)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var campaignResults = _mapper.Map<List<CampaignResult>>(campaigns);

            return Result.Success<IEnumerable<CampaignResult>>(campaignResults);
        }
        catch (Exception e)
        {
            _logger.LogError("Error while getting campaigns, {errors}", e.Message);
            
            return Result.Failure<IEnumerable<CampaignResult>>(new Error("500", "Error while getting campaigns"));
        }
    }
}