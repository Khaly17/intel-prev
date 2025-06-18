using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using Soditech.IntelPrev.Users.Shared;
using Soditech.IntelPrev.Users.Shared.Roles;
using Soditech.IntelPrev.Users.Shared.Tenants;
using Soditech.IntelPrev.Users.Shared.Users;
using Soditech.IntelPrev.Web.Models;

namespace Soditech.IntelPrev.Web.Pages.Administration.Users;

public partial class AddUser: ComponentBase
{
    [Inject] public AuthenticationStateProvider AuthStateProvider { get; set; } = default!;

    public UserResult NewUser { get; set; } = new();

    public IEnumerable<RoleModel> Roles { get; set; } = new List<RoleModel>();

    public IEnumerable<TenantResult> Tenants { get; set; } = new List<TenantResult>();

    public string TenantIdCacheKey { get; set; } = "selectedTenant";

    public string? ErrorMessage { get; set; }

    public string? SuccessMessage { get; set; }

    private List<Guid> SelectedRoles { get; set; } = [];

    [Inject] private ILogger<AddUser> Logger { get; set; } = default!;


    protected override async Task OnInitializedAsync()
    {
        GetSelectedTenant();
        await SetTenantFromAuthUserAsync();
        await LoadRoles();
        await LoadTenants();
        //await LoadSitesAsync();
    }


    private async Task CreateUser()
    {
        ErrorMessage = null;
        SuccessMessage = null;
        try
        {
            var result = await ProxyService.PostAsync<UserResult>(UserRoutes.Users.Create, NewUser);

            if (result.IsSuccess)
            {
                SuccessMessage = "L'utilisateur a été ajouté avec succès !";
                var userId = result.Value.Id;

                await AssignRolesToUser(userId);
                CacheService.Set(TenantIdCacheKey, "");
                Navigation.NavigateTo("/users");
            }
            else
            {
                ErrorMessage = result.Error?.Message ?? "Une erreur est survenue lors de la création de l'utilisateur.";
                Logger.LogError("{code} : {message}", result.Error?.Code, result.Error?.Message);
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = "Error: Cannot create user";
            Logger.LogError(ex, ErrorMessage);
        }
    }
    private async Task LoadRoles()
    {
        try
        {
            var response = await ProxyService.GetAsync<IEnumerable<RoleResult>>(UserRoutes.Roles.GetAll);
            if (response.IsSuccess)
            {
                Roles = response.Value.Select(r => new RoleModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    IsSelected = false
                }).ToList();
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = " Error: Cannot load roles";
            Logger.LogError(ex, ErrorMessage);
        }
    }

    private async Task AssignRolesToUser(Guid userId)
    {
        SelectedRoles = Roles.Where(r => r.IsSelected).Select(r => r.Id).ToList();
        if (SelectedRoles.Any())
        {
            foreach (var roleId in SelectedRoles)
            {
                var assignRolesCommand = new AffectRoleToUserCommand
                {
                    UserId = userId,
                    RoleId = roleId
                };
                var result = await ProxyService.PostAsync<AffectRoleToUserCommand>(UserRoutes.Roles.AffectToUser, assignRolesCommand);

                if (!result.IsSuccess)
                {
                    ErrorMessage = "An error occurred while assigning the role with ID .";
                    Logger.LogError("{code} : {message}", result.Error?.Code, result.Error?.Message);
                    break;
                }
            }
            if (string.IsNullOrEmpty(ErrorMessage))
            {
                SuccessMessage = "Roles assigned successfully!";
            }
        }

    }


    private async Task LoadTenants()
    {
        try
        {
            var response = await ProxyService.GetAsync<IEnumerable<TenantResult>>(UserRoutes.Tenants.GetAll);
            if (response.IsSuccess)
            {
                Tenants = response.Value;
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = " Error: Cannot load tenants";
            Logger.LogError(ex, ErrorMessage);
        }
    }

    private void GetSelectedTenant()
    {

        var (exists, cachedValue) = CacheService.Get(TenantIdCacheKey);

        if (exists)
        {
            var tenant = (TenantResult)cachedValue;
            NewUser.TenantId = tenant.Id;
        }

    }
    private async Task SetTenantFromAuthUserAsync()
    {
        var authState = await AuthStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        var isSuperAdmin = user.IsInRole("Super Admin");

        if (!isSuperAdmin)
        {
            var tenantClaim = user.FindFirst("tenant_id")?.Value;
            if (Guid.TryParse(tenantClaim, out var tenantId))
            {
                NewUser.TenantId = tenantId;
            }
        }
    }

}