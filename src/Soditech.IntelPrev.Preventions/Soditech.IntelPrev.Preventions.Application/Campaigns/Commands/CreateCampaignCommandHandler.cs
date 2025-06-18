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
using Soditech.IntelPrev.Prevensions.Shared.Campaigns;
using Soditech.IntelPrev.Preventions.Persistence.Models;

namespace Soditech.IntelPrev.Prevensions.Application.Campaigns.Commands;

public class CreateCampaignCommandHandler(IServiceProvider serviceProvider) : IRequestHandler<CreateCampaignCommand, TResult<CampaignResult>>
{
    private readonly IRepository<Campaign> _campaignRepository = serviceProvider.GetRequiredService<IRepository<Campaign>>();
    private readonly ILogger<CreateCampaignCommandHandler> _logger = serviceProvider.GetRequiredService<ILogger<CreateCampaignCommandHandler>>();
    private readonly IMapper _mapper = serviceProvider.GetRequiredService<IMapper>();
    private readonly ISensor6tySession _session = serviceProvider.GetRequiredService<ISensor6tySession>();
    private readonly IPublisher _publisher = serviceProvider.GetRequiredService<IPublisher>();

    public async Task<TResult<CampaignResult>> Handle(CreateCampaignCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (_session.TenantId == Guid.Empty || _session.TenantId == null)
            {
                return Result.Failure<CampaignResult>(new Error("400", "cannot create campaign without a tenant"));
            }
            var campaign = _mapper.Map<Campaign>(request);
            campaign.TenantId = _session.TenantId.Value;
            
            campaign.CreatorId = _session.UserId;
            campaign.CreatedAt = DateTimeOffset.UtcNow;

            await _campaignRepository.AddAsync(campaign, cancellationToken);

            await _publisher.Publish(_mapper.Map<CampaignCreatedEvent>(campaign), cancellationToken);
            
            return Result.Success(_mapper.Map<CampaignResult>(campaign));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cannot create campaign");
        }
        
        return Result.Failure<CampaignResult>(new Error("500", "Error while creating campaign"));
    }   
}