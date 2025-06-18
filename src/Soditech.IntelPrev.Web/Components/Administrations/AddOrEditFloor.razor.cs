using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Soditech.IntelPrev.Prevensions.Shared.Floors;

namespace Soditech.IntelPrev.Web.Components.Administrations;

public partial class AddOrEditFloor: ComponentBase
{
    [Parameter]
    public FloorResult NewFloor { get; set; } = default!;

    [Parameter]
    public string Title { get; set; } = "Ajouter un etage";
    [Parameter]
    public string? ErrorMessage { get; set; }

    [Parameter]
    public string? SuccessMessage { get; set; }

    [Parameter]
    public EventCallback OnFloorCreated { get; set; }
    public async Task CreateFloor()
    {
        await OnFloorCreated.InvokeAsync(null);
    }
}
