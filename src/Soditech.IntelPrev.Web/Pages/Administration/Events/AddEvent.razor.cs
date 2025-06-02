using Microsoft.AspNetCore.Components;
using Soditech.IntelPrev.Preventions.Shared.Events;
using Soditech.IntelPrev.Users.Shared.Tenants;
using Soditech.IntelPrev.Users.Shared;
using Soditech.IntelPrev.Preventions.Shared;
using Soditech.IntelPrev.Preventions.Shared.CommitteeMembers;

namespace Soditech.IntelPrev.Web.Pages.Administration.Events;

public partial class AddEvent
{
    public EventResult NewEvent { get; set; } = new EventResult();
    public IList<CommitteeMemberResult> CommitteeMembers { get; set; } = [];
    public string title { get; set; } = "Ajouter un événement";
    public string? errorMessage { get; set; }
    public string? successMessage { get; set; }
    [Inject] private ILogger<AddEvent> Logger { get; set; } = default!;
    private bool IsLoading { get; set; }

    private async Task CreateEvent()
    {
        errorMessage = null;
        successMessage = null;
        try
        {
            var result = await ProxyService.PostAsync<EventResult>(PreventionRoutes.Events.Create, NewEvent);

            if (result.IsSuccess)
            {
                successMessage = "L'événement a été ajouté avec succès !";
                var eventId = result.Value.Id;

                Navigation.NavigateTo("/events");
            }
            else
            {
                errorMessage = result.Error?.Message ?? "Une erreur est survenue lors de la création de l'événement";
                Logger.LogError("{code} : {message}", result.Error?.Code, result.Error?.Message);
            }
        }
        catch (Exception ex)
        {
            errorMessage = $"Une erreur interne est survenue lors de la création de l'événement.";
            Logger.LogError(ex, errorMessage);
        }
    }
    protected override async Task OnInitializedAsync()
    {
        await GetCommitteeMembers();
    }

    public async Task GetCommitteeMembers()
    {
        IsLoading = true;
        var committeeMembersResult = await ProxyService.GetAsync<IList<CommitteeMemberResult>>(PreventionRoutes.CommitteeMembers.GetAll);

        if (committeeMembersResult.IsSuccess)
        {
            CommitteeMembers = committeeMembersResult.Value;
        }
        IsLoading = false;
    }
}