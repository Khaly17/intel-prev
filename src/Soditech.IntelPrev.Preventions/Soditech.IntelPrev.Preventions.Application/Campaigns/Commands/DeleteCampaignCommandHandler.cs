using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sensor6ty.Repositories;
using Sensor6ty.Results;
using Soditech.IntelPrev.Prevensions.Shared.Campaigns;
using Soditech.IntelPrev.Preventions.Persistence.Models;

namespace Soditech.IntelPrev.Prevensions.Application.Campaigns.Commands;

public class DeleteCampaignCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<DeleteCampaignCommand, Result>
{
    private readonly IRepository<Campaign> _campaignRepository = serviceProvider.GetRequiredService<IRepository<Campaign>>();
    private readonly ILogger<DeleteCampaignCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<DeleteCampaignCommandHandler>>();
    private readonly IPublisher _publisher = serviceProvider.GetRequiredService<IPublisher>();

    public async Task<Result> Handle(DeleteCampaignCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var campaign = await _campaignRepository.GetAsync(request.Id, cancellationToken);
            if (campaign == null)
            {
                return Result.Failure<CampaignResult>(new Error("404", "Campaign not found"));
            }
            
            await _campaignRepository.DeleteAsync(campaign, cancellationToken);
            
            await _publisher.Publish(new CampaignDeletedEvent(request.Id), cancellationToken);

            return Result.Success();
        }
        catch(Exception ex)
        {
            _logger.LogError(ex , "Error while deleting campaign");

            return Result.Failure(new Error("500", "Error while deleting campaign"));
        }
    }   
}