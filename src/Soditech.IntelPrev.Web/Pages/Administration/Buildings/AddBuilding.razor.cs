using Microsoft.AspNetCore.Components;
using Soditech.IntelPrev.Preventions.Shared.Buildings;
using Soditech.IntelPrev.Users.Shared.Tenants;
using Soditech.IntelPrev.Users.Shared;
using Soditech.IntelPrev.Preventions.Shared;
using Soditech.IntelPrev.Preventions.Shared.Floors;

namespace Soditech.IntelPrev.Web.Pages.Administration.Buildings;

public partial class AddBuilding
{
    public BuildingResult NewBuilding { get; set; } = new();
    public string title { get; set; } = "Ajouter un batiment";
    public string? errorMessage { get; set; }
    public string? successMessage { get; set; }
    [Inject] private ILogger<AddBuilding> Logger { get; set; } = default!;
    private List<FloorResult> NewFloors { get; set; } = new List<FloorResult>();
    private FloorResult NewFloor { get; set; } = new FloorResult();

    private async Task CreateBuilding()
    {
        errorMessage = null;
        successMessage = null;

        try
        {
            var result = await ProxyService.PostAsync<BuildingResult>(PreventionRoutes.Buildings.Create, NewBuilding);

            if (result.IsSuccess)
            {
                successMessage = "Le batiment a été ajouté avec succès !";
                Navigation.NavigateTo("/buildings");
            }
            else
            {
                errorMessage = result.Error?.Message ?? "Une erreur est survenue lors de la création du bâtiment";
                Logger.LogError("{code} : {message}", result.Error?.Code, result.Error?.Message);
            }
        }
        catch (Exception ex)
        {
            errorMessage = "Une erreur interne est survenue lors de la création du bâtiment.";
            Logger.LogError(ex, errorMessage);
        }
    }

}
