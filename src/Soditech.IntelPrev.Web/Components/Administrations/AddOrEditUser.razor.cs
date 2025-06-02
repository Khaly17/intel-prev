using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Soditech.IntelPrev.Users.Shared.Tenants;
using Soditech.IntelPrev.Users.Shared.Users;
using Soditech.IntelPrev.Web.Models;

namespace Soditech.IntelPrev.Web.Components.Administrations;

public partial class AddOrEditUser : ComponentBase
{
    
    
    [Inject] public AuthenticationStateProvider AuthStateProvider { get; set; } = default!;
    [Parameter]
    public UserResult NewUser { get; set; } = default!;

    [Parameter]
    public IEnumerable<RoleModel> Roles { get; set; } = new List<RoleModel>();

    [Parameter]
    public string Title { get; set; } = "Ajouter un utilisateur";
    [Parameter]
    public string? ErrorMessage { get; set; }

    [Parameter]
    public string? SuccessMessage { get; set; }

    [Parameter]
    public IEnumerable<TenantResult> Tenants { get; set; } = new List<TenantResult>();

    [Parameter]
    public EventCallback OnUserCreated { get; set; }

    public bool IsSuperAdmin { get; set; }

    public async Task CreateUser()
    {
        await OnUserCreated.InvokeAsync(null);
    }

    protected override async Task OnInitializedAsync()
    {
        var userState = await AuthStateProvider.GetAuthenticationStateAsync();
        var user = userState.User;

        IsSuperAdmin = user.IsInRole("Super Admin");
    }
}