using System;
using System.Collections.Generic;
using MediatR;
using Sensor6ty.Results;

namespace Soditech.IntelPrev.Prevensions.Shared.Statistics;

public record StatisticResult
{
    public Guid Id { get; init; }
    public bool IsDeleted { get; init; }
    
    public string Category { get; set; } = string.Empty;
    public float Value { get; set; }
    public string Notes { get; set; } = string.Empty;
    public DateTimeOffset Date { get; set; }
    
    public Guid CampaignId { get; set; }
    public string CampaignName { get; set; } = default!;

    
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

public record CreateStatisticCommand : IRequest<TResult<StatisticResult>>
{
    public string Category { get; set; } = string.Empty;
    public float Value { get; set; }
    public string Notes { get; set; } = string.Empty;
    public DateTimeOffset Date { get; set; }
    public Guid CampaignId { get; set; }
}

public record UpdateStatisticCommand : IRequest<TResult<StatisticResult>>
{
    public Guid Id { get; set; }
    public string Category { get; set; } = string.Empty;
    public float Value { get; set; }
    public string Notes { get; set; } = string.Empty;
    public DateTimeOffset Date { get; set; }
    public Guid CampaignId { get; set; }
}

public record DeleteStatisticCommand : IRequest<Result>
{
    public Guid Id { get; set; }
}


public record GetStatisticQuery(Guid Id) : IRequest<TResult<StatisticResult>>;

public record GetStatisticsCountQuery : IRequest<TResult<int>>;

public record GetStatisticsQuery : IRequest<TResult<IEnumerable<StatisticResult>>>;

