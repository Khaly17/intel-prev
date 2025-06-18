using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Soditech.IntelPrev.Prevensions.Shared;
using Soditech.IntelPrev.Prevensions.Shared.CommitteeMembers;
using Soditech.IntelPrev.Users.Shared;
using Soditech.IntelPrev.Users.Shared.Roles;
using Soditech.IntelPrev.Web.Models;

namespace Soditech.IntelPrev.Web.Pages.Administration.CommitteeMembers;

public partial class AddCommitteeMember : ComponentBase
{
    public CommitteeMemberResult NewCommitteeMember { get; set; } = new();

    public IEnumerable<RoleModel> Roles { get; set; } = new List<RoleModel>();

    public string? ErrorMessage { get; set; }

    public string? SuccessMessage { get; set; }

    private List<Guid> SelectedRoles { get; set; } = [];

    [Inject] private ILogger<AddCommitteeMember> Logger { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await LoadRoles();
    }

    private async Task CreateCommitteeMember()
    {
        ErrorMessage = null;
        SuccessMessage = null;
        try
        {
            var result = await ProxyService.PostAsync<CommitteeMemberResult>(PreventionRoutes.CommitteeMembers.Create, NewCommitteeMember);

            if (result.IsSuccess)
            {
                SuccessMessage = "Le membre du comité a été ajouté avec succès !";
                Navigation.NavigateTo("/committeemembers");
            }
            else
            {
                ErrorMessage = result.Error?.Message ?? "Une erreur est survenue lors de la création du membre du comité.";
                Logger.LogError("{code} : {message}", result.Error?.Code, result.Error?.Message);
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = "Error: Cannot create committee member";
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

}