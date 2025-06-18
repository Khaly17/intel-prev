using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Soditech.IntelPrev.Users.Shared;
using Soditech.IntelPrev.Users.Shared.Tenants;

namespace Soditech.IntelPrev.Web.Pages.Administration.Tenants;

public partial class EditTenant: ComponentBase
{
    [Parameter]
    public string TenantId { get; set; } = string.Empty;
    public string Title { get; set; } = "Mo la structure";
    private TenantResult Tenant { get; set; } = new TenantResult();

    private string? _successMessage;

    private string? _errorMessage;
    [Inject] private ILogger<EditTenant> Logger { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await GetTenantAsync();
    }
    private async Task GetTenantAsync()
    {
        try
        {
            var result = await ProxyService.GetAsync<TenantResult>(UserRoutes.Tenants.GetById.Replace("{id:guid}", TenantId));

            if (result.IsSuccess)
            {
                Tenant = result.Value;

            }
            else
            {
                _errorMessage = "Erreur de récupération de la structure.";
            }
        }
        catch (Exception ex)
        {
            _errorMessage = $"Erreur: {ex.Message}";
        }
    }

    private async Task UpdateTenant()
    {
        if (Tenant.Id == Guid.Empty)
        {
            _errorMessage = "L'ID de la structure est invalide.";
            return;
        }

        var updateResult = await ProxyService.PostAsync<TenantResult>(UserRoutes.Tenants.Update.Replace("{id:guid}", Tenant.Id.ToString()), Tenant);
        if (updateResult.IsSuccess)
        {
            _successMessage = "Structure mis à jour avec succès.";
            _errorMessage = null;
            Navigation.NavigateTo("/tenants");
        }
        else
        {
            _errorMessage = "Erreur lors de la mise à jour de la structure.";
        }
    }

}
