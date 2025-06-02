using Microsoft.AspNetCore.Components;
using Soditech.IntelPrev.Users.Shared;
using Soditech.IntelPrev.Users.Shared.Tenants;
using Soditech.IntelPrev.Users.Shared.Users;
using Soditech.IntelPrev.Web.Models;
using Soditech.IntelPrev.Web.Pages.Administration.Users;

namespace Soditech.IntelPrev.Web.Pages.Administration.Tenants;

public partial class EditTenant: ComponentBase
{
    [Parameter]
    public string tenantId { get; set; } = string.Empty;
    public string title { get; set; } = "Modification de la structure";
    private TenantResult tenant { get; set; } = new TenantResult();

    private string? successMessage;

    private string? errorMessage;
    [Inject] private ILogger<EditTenant> Logger { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await GetTenantAsync();
    }
    private async Task GetTenantAsync()
    {
        try
        {
            var result = await ProxyService.GetAsync<TenantResult>(UserRoutes.Tenants.GetById.Replace("{id:guid}", tenantId));

            if (result.IsSuccess)
            {
                tenant = result.Value;

            }
            else
            {
                errorMessage = $"Erreur de récupération de la structure.";
            }
        }
        catch (Exception ex)
        {
            errorMessage = $"Erreur: {ex.Message}";
        }
    }

    private async Task UpdateTenant()
    {
        if (tenant.Id == Guid.Empty)
        {
            errorMessage = "L'ID de la structure est invalide.";
            return;
        }

        var updateResult = await ProxyService.PostAsync<TenantResult>(UserRoutes.Tenants.Update.Replace("{id:guid}", tenant.Id.ToString()), tenant);
        if (updateResult.IsSuccess)
        {
            successMessage = "Structure mis à jour avec succès.";
            errorMessage = null;
            Navigation.NavigateTo("/tenants");
        }
        else
        {
            errorMessage = "Erreur lors de la mise à jour de la structure.";
        }
    }

}
