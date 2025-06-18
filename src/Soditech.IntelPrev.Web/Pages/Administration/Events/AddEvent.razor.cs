using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Soditech.IntelPrev.Prevensions.Shared;
using Soditech.IntelPrev.Prevensions.Shared.CommitteeMembers;
using Soditech.IntelPrev.Prevensions.Shared.Events;

namespace Soditech.IntelPrev.Web.Pages.Administration.Events;

public partial class AddEvent
{
    public EventResult NewEvent { get; set; } = new();
    public IList<CommitteeMemberResult> CommitteeMembers { get; set; } = [];
    public string Title { get; set; } = "Ajouter un événement";
    public string? ErrorMessage { get; set; }
    public string? SuccessMessage { get; set; }
    [Inject] private ILogger<AddEvent> Logger { get; set; } = default!;
    private bool IsLoading { get; set; }

    private async Task CreateEvent()
    {
        ErrorMessage = null;
        SuccessMessage = null;
        try
        {
            var result = await ProxyService.PostAsync<EventResult>(PreventionRoutes.Events.Create, NewEvent);

            if (result.IsSuccess)
            {
                SuccessMessage = "L'événement a été ajouté avec succès !";
                Navigation.NavigateTo("/events");
            }
            else
            {
                ErrorMessage = result.Error?.Message ?? "Une erreur est survenue lors de la création de l'événement";
                Logger.LogError("{code} : {message}", result.Error?.Code, result.Error?.Message);
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = "Une erreur interne est survenue lors de la création de l'événement.";
            Logger.LogError(ex, ErrorMessage);
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