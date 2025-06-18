using System;
using System.Collections.Generic;
using MediatR;
using Sensor6ty.Results;

namespace Soditech.IntelPrev.Reports.Shared.RegisterFields;

public record RegisterFieldResult
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
    
    public string Name { get; set; } = string.Empty;  // Field name (ex. Victime, Lieu)
    public string Description { get; set; } = string.Empty;  // Field description
    public string FieldType { get; set; } = string.Empty; // Field type (texte, date, booléen, etc.)
    public bool IsRequired { get; set; } // Required or not
    public int DisplayOrder { get; set; } // Display order
    
    public Guid RegisterTypeId { get; set; } // reference to the register type
    public string RegisterTypeName { get; set; } = default!;
    
    public Guid? RegisterFieldGroupId { get; set; } // reference to the register type
    public string? RegisterFieldGroupName { get; set; } = default!;

    
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

public record CreateRegisterFieldCommand : IRequest<TResult<RegisterFieldResult>>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string FieldType { get; set; } = string.Empty;
    public int DisplayOrder { get; set; } // Display order
    public bool IsRequired { get; set; }
    public Guid RegisterTypeId { get; set; }
    public Guid? RegisterFieldGroupId { get; set; } // reference to the register type
}

public record CreateReportFieldCommand : IRequest<TResult<RegisterFieldResult>>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string FieldType { get; set; } = string.Empty;
    public int DisplayOrder { get; set; } // Display order
    public bool IsRequired { get; set; }
    public Guid RegisterTypeId { get; set; }
    public Guid? RegisterFieldGroupId { get; set; } // reference to the register type
    public Guid RegisterFieldId { get; set; } // reference to the register field
    public object Value { get; set; } = default!;
    
    // Property for UI validation
    public bool IsRequiredAndNotFilled => IsRequired && (
        Value == null || 
        (Value is string strValue && string.IsNullOrWhiteSpace(strValue)) ||
        (Value is DateTime dateValue && dateValue == default)
    );
}

public record UpdateRegisterFieldCommand : IRequest<TResult<RegisterFieldResult>>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string FieldType { get; set; } = string.Empty;
    public int DisplayOrder { get; set; } // Display order
    public bool IsRequired { get; set; }
    public Guid RegisterTypeId { get; set; }
    public Guid? RegisterFieldGroupId { get; set; } // reference to the register type
}

public record DeleteRegisterFieldCommand : IRequest<Result>
{
    public Guid Id { get; set; }
}

public record GetRegisterFieldQuery(Guid Id) : IRequest<TResult<RegisterFieldResult>>;
public record GetRegisterFieldsCountQuery : IRequest<TResult<int>>;
public record GetRegisterFieldsQuery : IRequest<TResult<IEnumerable<RegisterFieldResult>>>;