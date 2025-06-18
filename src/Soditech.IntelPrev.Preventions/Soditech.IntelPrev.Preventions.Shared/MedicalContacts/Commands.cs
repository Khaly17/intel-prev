using System;
using System.Collections.Generic;
using MediatR;
using Sensor6ty.Results;

namespace Soditech.IntelPrev.Prevensions.Shared.MedicalContacts;

public record MedicalContactResult
{
    public Guid Id { get; init; }

    public bool IsDeleted { get; init; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string Position { get; set; } = string.Empty;

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

public record CreateMedicalContactCommand : IRequest<TResult<MedicalContactResult>>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
}

public record UpdateMedicalContactCommand : IRequest<TResult<MedicalContactResult>>
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
}

public record DeleteMedicalContactCommand : IRequest<Result>
{
    public Guid Id { get; set; }
}

public record GetMedicalContactQuery : IRequest<TResult<MedicalContactResult>>
{
    public Guid Id { get; set; }
}

public record GetMedicalContactsQuery : IRequest<TResult<IEnumerable<MedicalContactResult>>>;

public record GetMedicalContactsCountQuery : IRequest<TResult<int>>;