using Microsoft.AspNetCore.Components;
using Soditech.IntelPrev.Users.Shared.Roles;
using Soditech.IntelPrev.Users.Shared;
using Soditech.IntelPrev.Users.Shared.Users;
using Soditech.IntelPrev.Web.Models;
using Soditech.IntelPrev.Users.Shared.Tenants;
using Microsoft.Extensions.Logging;

namespace Soditech.IntelPrev.Web.Pages.Administration.Users;

public partial class EditUser: ComponentBase
{

    [Parameter]
    public string UserId { get; set; } = string.Empty;
    private IEnumerable<RoleModel> _roles { get; set; } = new List<RoleModel>();
    private List<Guid> SelectedRoles { get; set; } = new List<Guid>();
    public string title { get; set; } = "Modifier l'utilisateur";
    private UserResult user { get; set; } = new UserResult();
    public UpdateUserCommand NewUser { get; set; } = new UpdateUserCommand();
    public IEnumerable<TenantResult> _tenants { get; set; } = new List<TenantResult>();
    public TenantResult _selectedTenant { get; set; } = default!;

    public string TenantIdCacheKey { get; set; } = "selectedTenant";

    private string SelectedTenantId { get; set; } = string.Empty;

    private string? successMessage;
    private string? errorMessage;
    [Inject] private ILogger<EditUser> Logger { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await GetUserAsync();
        await LoadTenants();
        //await LoadSitesAsync();
    }

    private async Task GetUserAsync()
    {
        try
        {
            var result = await ProxyService.GetAsync<UserResult>(UserRoutes.Users.GetById.Replace("{id:guid}", UserId));

            if (result.IsSuccess)
            {
                user = result.Value;

                await LoadRoles();

                var assignedRoleIds = user.Roles.Select(role => role.Id).ToList();

                _roles = _roles.Select(r =>
                {
                    r.IsSelected = assignedRoleIds.Contains(r.Id);
                    return r;
                }).ToList();

            }
            else
            {
                errorMessage = $"Erreur de récupération d'utilisateur.";
            }
        }
        catch (Exception ex)
        {
            errorMessage = $"Erreur: {ex.Message}";
        }
    }

    private async Task LoadRoles()
    {
        try
        {
            // Récupérer tous les rôles disponibles
            var response = await ProxyService.GetAsync<IEnumerable<RoleResult>>(UserRoutes.Roles.GetAll);
            if (response.IsSuccess)
            {
                _roles = response.Value.Select(r => new RoleModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    IsSelected = false
                    //IsSelected = SelectedRoles.Contains(r.Id) // Cocher les rôles déjà assignés
                }).ToList();
            }
        }
        catch (Exception ex)
        {
            errorMessage = $"Error {ex.Message}";
        }
    }

    private async Task UpdateUser()
    {
        // Mettre à jour l'utilisateur
        if (user.Id == Guid.Empty)
        {
            errorMessage = "L'ID de l'utilisateur est invalide.";
            return;
        }

        var updateResult = await ProxyService.PostAsync<UserResult>(UserRoutes.Users.Update.Replace("{id:guid}", user.Id.ToString()), user);
        if (updateResult.IsSuccess)
        {
            successMessage = "Utilisateur mis à jour avec succès.";
            errorMessage = null;
            await AssignRolesToUser(user.Id);
            Navigation.NavigateTo("/users");
        }
        else
        {
            errorMessage = "Erreur lors de la mise à jour de l'utilisateur.";
        }
    }

    private async Task AssignRolesToUser(Guid userId)
    {
        SelectedRoles = _roles.Where(r => r.IsSelected).Select(r => r.Id).ToList();

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
                errorMessage = $"An error occurred while assigning the role with ID: {roleId}.";
                break;
            }
        }

        if (string.IsNullOrEmpty(errorMessage))
        {
            successMessage = "Roles assigned successfully!";
        }
    }


    private async Task LoadTenants()
    {
        try
        {
            var response = await ProxyService.GetAsync<IEnumerable<TenantResult>>(UserRoutes.Tenants.GetAll);
            if (response.IsSuccess)
            {
                _tenants = response.Value;
            }
        }
        catch (Exception ex)
        {
            errorMessage = $" Error: Cannot load tenants";
            Logger.LogError(ex, errorMessage);
        }
    }

}
