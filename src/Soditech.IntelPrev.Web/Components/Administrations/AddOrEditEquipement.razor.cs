using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Soditech.IntelPrev.Prevensions.Shared.Equipments;

namespace Soditech.IntelPrev.Web.Components.Administrations;

public partial class AddOrEditEquipment: ComponentBase
{
    [Parameter]
    public EquipmentResult NewEquipment { get; set; } = default!;

    [Parameter]
    public string Title { get; set; } = "Ajouter un équipement";
    [Parameter]
    public string? ErrorMessage { get; set; }

    [Parameter]
    public string? SuccessMessage { get; set; }

    [Parameter]
    public EventCallback OnEquipmentCreated { get; set; }
    public async Task CreateEquipment()
    {
        await OnEquipmentCreated.InvokeAsync(null);
    }
}