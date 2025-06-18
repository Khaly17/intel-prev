using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Soditech.IntelPrev.Users.Shared.Roles;
using Soditech.IntelPrev.Users.Shared;
using Soditech.IntelPrev.Web.Models;
using Microsoft.Extensions.Logging;
using Soditech.IntelPrev.Prevensions.Shared;
using Soditech.IntelPrev.Prevensions.Shared.CommitteeMembers;

namespace Soditech.IntelPrev.Web.Pages.Administration.CommitteeMembers;

public partial class EditCommitteeMember : ComponentBase
{
    [Parameter]
    public string CommitteeMemberId { get; set; } = string.Empty;
    private IEnumerable<RoleModel> Roles { get; set; } = new List<RoleModel>();
    private List<Guid> SelectedRoles { get; set; } = new();
    public string Title { get; set; } = "Modifier le membre du comité";
    private CommitteeMemberResult CommitteeMember { get; set; } = new();
    public UpdateCommitteeMemberCommand NewCommitteeMember { get; set; } = new();

    private string? _successMessage;
    private string? _errorMessage;
    [Inject] private ILogger<EditCommitteeMember> Logger { get; set; } = default!;
    private const string CommitteeMembersCacheKey = "CommitteeMembers";
    private string GetCommitteeMemberCacheKey() => $"CommitteeMember_{CommitteeMemberId}";

    protected override async Task OnInitializedAsync()
    {
        await LoadCommitteeMemberFromApiAsync();
    }

    private async Task LoadCommitteeMembersAsync()
    {
        var cacheKey = GetCommitteeMemberCacheKey();
        var (exists, cachedValue) = CacheService.Get(cacheKey);

        if (exists)
        {
            CommitteeMember = (CommitteeMemberResult)cachedValue;
        }
        else
        {
            await LoadCommitteeMemberFromApiAsync();
            CacheService.Set(cacheKey, CommitteeMember);
        }
    }
    private async Task LoadCommitteeMemberFromApiAsync()
    {
        try
        {
            var result = await ProxyService.GetAsync<CommitteeMemberResult>(PreventionRoutes.CommitteeMembers.GetById.Replace("{id:guid}", CommitteeMemberId));

            if (result.IsSuccess)
            {
                CommitteeMember = result.Value;

                await LoadRoles();
            }
            else
            {
                _errorMessage = "Erreur de récupération du membre du comité.";
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
            _errorMessage = $"Error {ex.Message}";
        }
    }

    private async Task UpdateCommitteeMember()
    {
        if (CommitteeMember.Id == Guid.Empty)
        {
            _errorMessage = "L'ID du membre du comité est invalide.";
            return;
        }

        var updateResult = await ProxyService.PostAsync<CommitteeMemberResult>(PreventionRoutes.CommitteeMembers.Update.Replace("{id:guid}", CommitteeMember.Id.ToString()), CommitteeMember);
        if (updateResult.IsSuccess)
        {
            _successMessage = "Membre du comité mis à jour avec succès.";
            _errorMessage = null;
            CacheService.Set(GetCommitteeMemberCacheKey(), CommitteeMember);
            CacheService.Set(CommitteeMembersCacheKey, null);
            Navigation.NavigateTo("/committeemembers");
        }
        else
        {
            _errorMessage = "Erreur lors de la mise à jour du membre du comité.";
        }
    }
}