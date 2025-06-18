using System;
using System.Collections.Generic;
using MediatR;
using Sensor6ty.Results;

namespace Soditech.IntelPrev.Prevensions.Shared.Campaigns;

public record CampaignResult
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateTimeOffset StartDate { get; set; }

    public DateTimeOffset EndDate { get; set; }

    public string? CreatorFullName { get; set; }

    public Guid? CreatorId { get; set; }

    public DateTimeOffset? CreatedAt { get; set; }

    public string? DeleterFullName { get; set; }

    public Guid? DeleterId { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public string? UpdaterFullName { get; set; }

    public Guid? UpdaterId { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }

}

public record CreateCampaignCommand : IRequest<TResult<CampaignResult>>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
}

public record UpdateCampaignCommand : IRequest<TResult<CampaignResult>>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
}

public record DeleteCampaignCommand : IRequest<Result>
{
    public Guid Id { get; set; }
}

public record GetCampaignQuery : IRequest<TResult<CampaignResult>>
{
    public Guid Id { get; set; }
}

public record GetCampaignsQuery : IRequest<TResult<IEnumerable<CampaignResult>>>;

public record GetCampaignsCountQuery : IRequest<TResult<int>>;

