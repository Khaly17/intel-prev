using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    private IEnumerable<RoleModel> Roles { get; set; } = new List<RoleModel>();
    private List<Guid> SelectedRoles { get; set; } = [];
    private string Title { get; set; } = "Modifier l'utilisateur";
    private UserResult User { get; set; } = new();
    public IEnumerable<TenantResult> Tenants { get; set; } = new List<TenantResult>();

    private string? _successMessage;
    private string? _errorMessage;
    [Inject] private ILogger<EditUser> Logger { get; set; } = null!;

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
                User = result.Value;

                await LoadRoles();

                var assignedRoleIds = User.Roles.Select(role => role.Id).ToList();

                Roles = Roles.Select(r =>
                {
                    r.IsSelected = assignedRoleIds.Contains(r.Id);
                    return r;
                }).ToList();

            }
            else
            {
                _errorMessage = "Erreur de récupération d'utilisateur.";
            }
        }
        catch (Exception ex)
        {
            _errorMessage = $"Erreur: {ex.Message}";
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
                Roles = response.Value.Select(r => new RoleModel
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
            _errorMessage = $"Error {ex.Message}";
        }
    }

    private async Task UpdateUser()
    {
        // Mettre à jour l'utilisateur
        if (User.Id == Guid.Empty)
        {
            _errorMessage = "L'ID de l'utilisateur est invalide.";
            return;
        }

        var updateResult = await ProxyService.PostAsync<UserResult>(UserRoutes.Users.Update.Replace("{id:guid}", User.Id.ToString()), User);
        if (updateResult.IsSuccess)
        {
            _successMessage = "Utilisateur mis à jour avec succès.";
            _errorMessage = null;
            await AssignRolesToUser(User.Id);
            Navigation.NavigateTo("/users");
        }
        else
        {
            _errorMessage = "Erreur lors de la mise à jour de l'utilisateur.";
        }
    }

    private async Task AssignRolesToUser(Guid userId)
    {
        SelectedRoles = Roles.Where(r => r.IsSelected).Select(r => r.Id).ToList();

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
                _errorMessage = $"An error occurred while assigning the role with ID: {roleId}.";
                break;
            }
        }

        if (string.IsNullOrEmpty(_errorMessage))
        {
            _successMessage = "Roles assigned successfully!";
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
            _errorMessage = "Error: Cannot load tenants";
            Logger.LogError(ex, _errorMessage);
        }
    }

}
