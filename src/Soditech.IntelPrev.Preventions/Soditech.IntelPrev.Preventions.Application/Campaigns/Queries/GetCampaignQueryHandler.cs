using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Soditech.IntelPrev.Preventions.Persistence.Models;
using Soditech.IntelPrev.Preventions.Shared.Campaigns;

namespace Soditech.IntelPrev.Preventions.Application.Campaigns.Queries;

public class GetCampaignQueryHandler(IServiceProvider serviceProvider)
    : IRequestHandler<GetCampaignQuery, TResult<CampaignResult>>
{
    private readonly IRepository<Campaign> _campaignRepository = serviceProvider.GetRequiredService<IRepository<Campaign>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    
    public async Task<TResult<CampaignResult>> Handle(GetCampaignQuery request, CancellationToken cancellationToken)
    {
        var campaign = await _campaignRepository.GetAsync(request.Id, cancellationToken);

        if (campaign == null)
        {
            return Result.Failure<CampaignResult>(new Error("404", "Campaign not found"));
        }

        var campaignResult = _mapper.Map<CampaignResult>(campaign);

        return Result.Success(campaignResult);
    }
}