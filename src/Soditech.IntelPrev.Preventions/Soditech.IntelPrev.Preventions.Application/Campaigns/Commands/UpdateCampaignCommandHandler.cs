using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Sensor6ty.Sessions;
using Soditech.IntelPrev.Preventions.Persistence.Models;
using Soditech.IntelPrev.Preventions.Shared.Campaigns;

namespace Soditech.IntelPrev.Preventions.Application.Campaigns.Commands;

public class UpdateCampaignCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<UpdateCampaignCommand, TResult<CampaignResult>>
{
    private readonly IRepository<Campaign> _campaignRepository = serviceProvider.GetRequiredService<IRepository<Campaign>>();
    private readonly ILogger<UpdateCampaignCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<UpdateCampaignCommandHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly IPublisher _publisher = serviceProvider.GetRequiredService<IPublisher>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();

    public async Task<TResult<CampaignResult>> Handle(UpdateCampaignCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var campaign = await _campaignRepository.GetAsync(request.Id, cancellationToken);
            if (campaign == null)
            {
                return Result.Failure<CampaignResult>(new Error("404", "Campaign not found"));
            }
            
            _mapper.Map(request, campaign);
            
            campaign.UpdaterId = _session.UserId;
            campaign.UpdatedAt = DateTimeOffset.UtcNow;
            
            await _campaignRepository.UpdateAsync(campaign, cancellationToken);
            await _publisher.Publish(_mapper.Map<CampaignUpdatedEvent>(campaign), cancellationToken);

            
            return Result.Success(_mapper.Map<CampaignResult>(campaign));
        }
        catch(Exception ex)
        {
            _logger.LogError(ex , "Error while updating campaign");

            return Result.Failure<CampaignResult>(new Error("500", "Error while updating campaign"));
        }
    }   
}