using MediatR;
using Sensor6ty.Results;

namespace Soditech.IntelPrev.Notifications.Shared;

public record SendEmailCommand : IRequest<Result>
{
    public required string To { get; init; }
    public required string Subject { get; init; }
    public required string Body { get; init; }
    
    public string From { get; init; } = string.Empty;
}