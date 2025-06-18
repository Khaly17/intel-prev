using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Soditech.IntelPrev.Prevensions.Shared.CommitteeMembers;
using Soditech.IntelPrev.Web.Models;

namespace Soditech.IntelPrev.Web.Components.Administrations;

public partial class AddOrEditCommitteeMember : ComponentBase
{
    [Inject] public AuthenticationStateProvider AuthStateProvider { get; set; } = default!;
    [Parameter]
    public CommitteeMemberResult NewCommitteeMember { get; set; } = default!;

    [Parameter]
    public IEnumerable<RoleModel> Roles { get; set; } = new List<RoleModel>();

    [Parameter]
    public string Title { get; set; } = "Ajouter un membre du comité";
    [Parameter]
    public string? ErrorMessage { get; set; }

    [Parameter]
    public string? SuccessMessage { get; set; }

    [Parameter]
    public EventCallback OnCommitteeMemberCreated { get; set; }

    public bool IsSuperAdmin { get; set; }

    public async Task CreateCommitteeMember()
    {
        await OnCommitteeMemberCreated.InvokeAsync(null);
    }

    protected override async Task OnInitializedAsync()
    {
        var userState = await AuthStateProvider.GetAuthenticationStateAsync();

        if (userState.User.IsInRole("Super Admin"))
        {
            IsSuperAdmin = true;
        }
    }
}