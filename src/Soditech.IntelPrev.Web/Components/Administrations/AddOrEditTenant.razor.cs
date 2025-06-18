using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Soditech.IntelPrev.Users.Shared.Tenants;

namespace Soditech.IntelPrev.Web.Components.Administrations;

public partial class AddOrEditTenant: ComponentBase
{
    [Parameter]
    public TenantResult NewTenant { get; set; } = default!;

    [Parameter]
    public string Title { get; set; } = "Ajouter un nouveau";

    [Parameter]
    public string? ErrorMessage { get; set; }

    [Parameter]
    public string? SuccessMessage { get; set; }

    [Parameter]
    public EventCallback OnTenantCreated { get; set; }

    public async Task CreateTenant()
    {
        await OnTenantCreated.InvokeAsync(null);
    }

}
