using Microsoft.AspNetCore.Components;
using Soditech.IntelPrev.Users.Shared.Roles;
using Soditech.IntelPrev.Users.Shared;
using Soditech.IntelPrev.Web.Models;
using Microsoft.Extensions.Logging;
using Soditech.IntelPrev.Preventions.Shared.CommitteeMembers;
using Soditech.IntelPrev.Preventions.Shared;
using Soditech.IntelPrev.Preventions.Shared.Campaigns;
using Soditech.IntelPrev.Web.Services.Cache;

namespace Soditech.IntelPrev.Web.Pages.Administration.CommitteeMembers;

public partial class EditCommitteeMember : ComponentBase
{
    [Parameter]
    public string CommitteeMemberId { get; set; } = string.Empty;
    private IEnumerable<RoleModel> _roles { get; set; } = new List<RoleModel>();
    private List<Guid> SelectedRoles { get; set; } = new List<Guid>();
    public string title { get; set; } = "Modifier le membre du comité";
    private CommitteeMemberResult committeeMember { get; set; } = new CommitteeMemberResult();
    public UpdateCommitteeMemberCommand NewCommitteeMember { get; set; } = new UpdateCommitteeMemberCommand();

    private string? successMessage;
    private string? errorMessage;
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
            committeeMember = (CommitteeMemberResult)cachedValue;
        }
        else
        {
            await LoadCommitteeMemberFromApiAsync();
            CacheService.Set(cacheKey, committeeMember);
        }
    }
    private async Task LoadCommitteeMemberFromApiAsync()
    {
        try
        {
            var result = await ProxyService.GetAsync<CommitteeMemberResult>(PreventionRoutes.CommitteeMembers.GetById.Replace("{id:guid}", CommitteeMemberId));

            if (result.IsSuccess)
            {
                committeeMember = result.Value;

                await LoadRoles();
            }
            else
            {
                errorMessage = $"Erreur de récupération du membre du comité.";
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
            var response = await ProxyService.GetAsync<IEnumerable<RoleResult>>(UserRoutes.Roles.GetAll);
            if (response.IsSuccess)
            {
                _roles = response.Value.Select(r => new RoleModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    IsSelected = false
                }).ToList();
            }
        }
        catch (Exception ex)
        {
            errorMessage = $"Error {ex.Message}";
        }
    }

    private async Task UpdateCommitteeMember()
    {
        if (committeeMember.Id == Guid.Empty)
        {
            errorMessage = "L'ID du membre du comité est invalide.";
            return;
        }

        var updateResult = await ProxyService.PostAsync<CommitteeMemberResult>(PreventionRoutes.CommitteeMembers.Update.Replace("{id:guid}", committeeMember.Id.ToString()), committeeMember);
        if (updateResult.IsSuccess)
        {
            successMessage = "Membre du comité mis à jour avec succès.";
            errorMessage = null;
            CacheService.Set(GetCommitteeMemberCacheKey(), committeeMember);
            CacheService.Set(CommitteeMembersCacheKey, null);
            Navigation.NavigateTo("/committeemembers");
        }
        else
        {
            errorMessage = "Erreur lors de la mise à jour du membre du comité.";
        }
    }
}