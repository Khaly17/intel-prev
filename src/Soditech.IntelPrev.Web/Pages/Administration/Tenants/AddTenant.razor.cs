using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Soditech.IntelPrev.Users.Shared;
using Soditech.IntelPrev.Users.Shared.Tenants;

namespace Soditech.IntelPrev.Web.Pages.Administration.Tenants;

public partial class AddTenant: ComponentBase
{

    public TenantResult NewTenant { get; set; } = new();

    public string Title { get; set; } = "Ajouter une nouvelle structure";
    public string? ErrorMessage { get; set; }

    public string? SuccessMessage { get; set; }

    [Inject] private ILogger<AddTenant> Logger { get; set; } = default!;

    private async Task CreateTenant()
    {
        ErrorMessage = null;
        SuccessMessage = null;
        try
        {
            var result = await ProxyService.PostAsync<TenantResult>(UserRoutes.Tenants.Create, NewTenant);

            if (result.IsSuccess)
            {
                SuccessMessage = "La structure a été ajouté avec succès !";

                Navigation.NavigateTo("/tenants");
            }
            else
            {
                ErrorMessage = result.Error?.Message ?? "Une erreur est survenue lors de la création de la structure.";
                Logger.LogError("{code} : {message}", result.Error?.Code, result.Error?.Message);
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = "Une erreur interne est survenue lors de la création de la structure.";
            Logger.LogError(ex, ErrorMessage);
        }
    }

}
