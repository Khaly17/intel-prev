using Microsoft.AspNetCore.Components;
using Soditech.IntelPrev.Preventions.Shared.Events;
using Soditech.IntelPrev.Preventions.Shared;

namespace Soditech.IntelPrev.Web.Pages.Administration.Events;

public partial class EditEvent
{
    [Parameter]
    public string eventId { get; set; } = string.Empty;
    public string title { get; set; } = "Modification de l'événement";
    private EventResult eventItem { get; set; } = new EventResult();

    private string? successMessage;
    private string? errorMessage;
    [Inject] private ILogger<EditEvent> Logger { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await GetEventAsync();
    }

    private async Task GetEventAsync()
    {
        try
        {
            var result = await ProxyService.GetAsync<EventResult>(PreventionRoutes.Events.GetById.Replace("{id:guid}", eventId));

            if (result.IsSuccess)
            {
                eventItem = result.Value;
            }
            else
            {
                errorMessage = "Erreur de récupération des informations de l'événement.";
            }
        }
        catch (Exception ex)
        {
            errorMessage = $"Erreur: {ex.Message}";
        }
    }

    private async Task UpdateEvent()
    {
        if (eventItem.Id == Guid.Empty)
        {
            errorMessage = "L'ID de l'événement est invalide.";
            return;
        }

        var updateResult = await ProxyService.PostAsync<EventResult>(PreventionRoutes.Events.Update.Replace("{id:guid}", eventItem.Id.ToString()), eventItem);
        if (updateResult.IsSuccess)
        {
            successMessage = "Événement mis à jour avec succès.";
            errorMessage = null;
            Navigation.NavigateTo("/events");
        }
        else
        {
            errorMessage = "Erreur lors de la mise à jour des informations de l'événement.";
        }
    }
}