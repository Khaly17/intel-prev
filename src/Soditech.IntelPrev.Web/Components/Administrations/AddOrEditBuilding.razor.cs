using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Soditech.IntelPrev.Prevensions.Shared.Buildings;
using Soditech.IntelPrev.Prevensions.Shared.Equipments;
using Soditech.IntelPrev.Prevensions.Shared.Floors;

namespace Soditech.IntelPrev.Web.Components.Administrations;

public partial class AddOrEditBuilding: ComponentBase
{
    [Parameter]
    public BuildingResult NewBuilding { get; set; } = default!;

    [Parameter]
    public string Title { get; set; } = "Ajouter un bâtiment";
    [Parameter]
    public string? ErrorMessage { get; set; }

    [Parameter]
    public string? SuccessMessage { get; set; }

    [Parameter]
    public EventCallback OnBuildingCreated { get; set; }
    public async Task CreateBuilding()
    {
        await OnBuildingCreated.InvokeAsync(null);
    }

    private void AddFloor()
    {
        NewBuilding.Floors ??= [];
        NewBuilding.Floors.Add(new FloorResult()
        {
            Equipments = []
        });
        NewBuilding.NumberOfFloors++;
    }

    private void RemoveFloor(FloorResult floor)
    {
        NewBuilding.Floors.Remove(floor);
        NewBuilding.NumberOfFloors--;
    }

    private void AddEquipmentOnBuilding()
    {
        NewBuilding.Equipments ??= [];
        NewBuilding.Equipments.Add(new EquipmentResult()
        {
            LastInspectionDate = DateTimeOffset.UtcNow,
            NextInspectionDate = DateTimeOffset.UtcNow.AddDays(1),
        });
    }
    private void RemoveEquipment(EquipmentResult equipment)
    {
        NewBuilding.Equipments.Remove(equipment);
    }

    private static void AddEquipmentOnFloor(FloorResult floor)
    {
        floor.Equipments ??= [];
        floor.Equipments.Add(new EquipmentResult());
    }
    private static void RemoveEquipment(FloorResult floor, EquipmentResult equipment)
    {
        floor.Equipments.Remove(equipment);
    }

}
