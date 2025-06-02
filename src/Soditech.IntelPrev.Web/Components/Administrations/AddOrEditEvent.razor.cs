using Microsoft.AspNetCore.Components;
using Soditech.IntelPrev.Preventions.Shared.CommitteeMembers;
using Soditech.IntelPrev.Preventions.Shared.Events;

namespace Soditech.IntelPrev.Web.Components.Administrations;

public partial class AddOrEditEvent
{
    [Parameter]
    public EventResult NewEvent { get; set; } = default!;
    [Parameter]
    public IList<CommitteeMemberResult> CommitteeMemberResults { get; set; } = [];

    [Parameter]
    public string Title { get; set; } = "Ajouter un agenda";
    [Parameter]
    public string? ErrorMessage { get; set; }

    [Parameter]
    public string? SuccessMessage { get; set; }

    [Parameter]
    public EventCallback OnEventCreated { get; set; }
    public async Task CreateEvent()
    {
        await OnEventCreated.InvokeAsync(null);
    }
}