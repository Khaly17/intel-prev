using System;
using System.Collections.Generic;
using MediatR;
using Sensor6ty.Results;
using Soditech.IntelPrev.Prevensions.Shared.Equipments;
using Soditech.IntelPrev.Prevensions.Shared.Floors;

namespace Soditech.IntelPrev.Prevensions.Shared.Buildings;

public record BuildingResult
{
	public Guid Id { get; set; }

	public bool IsDeleted { get; set; }

	public string Name { get; set; } = string.Empty;

	public string Address { get; set; } = string.Empty;

	public string Description { get; set; } = string.Empty;

	public int NumberOfFloors { get; set; }

	public bool HasDAE { get; set; }

	public bool HasFirstAidKits { get; set; }

	public List<FloorResult> Floors { get; set; } = [];

	public List<EquipmentResult> Equipments { get; set; } = [];

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

public class CreateBuildingCommand : IRequest<TResult<BuildingResult>>
{
	public string Name { get; set; } = string.Empty;
	public string Address { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public int NumberOfFloors { get; set; }
	public bool HasDAE { get; set; }
	public bool HasFirstAidKits { get; set; }

	public List<CreateFloorCommand> Floors { get; set; } = [];

	public List<CreateEquipmentCommand> Equipments { get; set; } = [];

}

public class UpdateBuildingCommand : IRequest<TResult<BuildingResult>>
{
	public Guid Id { get; set; }
	public bool IsDeleted { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Address { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public int NumberOfFloors { get; set; }
	public bool HasDAE { get; set; }
	public bool HasFirstAidKits { get; set; }

	//public List<UpdateFloorCommand> Floors { get; set; } = [];

	//public List<UpdateEquipmentCommand> Equipments { get; set; } = [];

}
public class UpdateBuildingLocationCommand : IRequest<Result>
{
	public Guid Id { get; set; }
	public double Latitude { get; set; }
	public double Longitude { get; set; }
	public double? Altitude { get; set; }

}


public class DeleteBuildingCommand : IRequest<Result>
{
	public Guid Id { get; set; }
}

public class GetBuildingQuery : IRequest<TResult<BuildingResult>>
{
	public Guid Id { get; set; }
}

public class GetBuildingsQuery : IRequest<TResult<IEnumerable<BuildingResult>>>;

public class GetBuildingsCountQuery : IRequest<TResult<int>>;