using Microsoft.AspNetCore.Components;
using Soditech.IntelPrev.Users.Shared;
using Soditech.IntelPrev.Users.Shared.Tenants;
using Soditech.IntelPrev.Users.Shared.Users;
using Soditech.IntelPrev.Web.Models;
using Soditech.IntelPrev.Web.Pages.Administration.Users;

namespace Soditech.IntelPrev.Web.Pages.Administration.Tenants;

public partial class AddTenant: ComponentBase
{

    public TenantResult NewTenant { get; set; } = new();

    public string title { get; set; } = "Ajouter une nouvelle structure";
    public string? errorMessage { get; set; }

    public string? successMessage { get; set; }

    [Inject] private ILogger<AddTenant> Logger { get; set; } = default!;

    private async Task CreateTenant()
    {
        errorMessage = null;
        successMessage = null;
        try
        {
            var result = await ProxyService.PostAsync<TenantResult>(UserRoutes.Tenants.Create, NewTenant);

            if (result.IsSuccess)
            {
                successMessage = "La structure a été ajouté avec succès !";
                var userId = result.Value.Id;

                Navigation.NavigateTo("/tenants");
            }
            else
            {
                errorMessage = result.Error?.Message ?? "Une erreur est survenue lors de la création de la structure.";
                Logger.LogError("{code} : {message}", result.Error?.Code, result.Error?.Message);
            }
        }
        catch (Exception ex)
        {
            errorMessage = $"Une erreur interne est survenue lors de la création de la structure.";
            Logger.LogError(ex, errorMessage);
        }
    }

}
