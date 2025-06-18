using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Soditech.IntelPrev.Prevensions.Shared;
using Soditech.IntelPrev.Prevensions.Shared.Events;

namespace Soditech.IntelPrev.Web.Pages.Administration.Events;

public partial class EditEvent
{
    [Parameter]
    public string EventId { get; set; } = string.Empty;
    public string Title { get; set; } = "Mod l'événement";
    private EventResult EventItem { get; set; } = new EventResult();

    private string? _successMessage;
    private string? _errorMessage;
    [Inject] private ILogger<EditEvent> Logger { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await GetEventAsync();
    }

    private async Task GetEventAsync()
    {
        try
        {
            var result = await ProxyService.GetAsync<EventResult>(PreventionRoutes.Events.GetById.Replace("{id:guid}", EventId));

            if (result.IsSuccess)
            {
                EventItem = result.Value;
            }
            else
            {
                _errorMessage = "Erreur de récupération des informations de l'événement.";
            }
        }
        catch (Exception ex)
        {
            _errorMessage = $"Erreur: {ex.Message}";
        }
    }

    private async Task UpdateEvent()
    {
        if (EventItem.Id == Guid.Empty)
        {
            _errorMessage = "L'ID de l'événement est invalide.";
            return;
        }

        var updateResult = await ProxyService.PostAsync<EventResult>(PreventionRoutes.Events.Update.Replace("{id:guid}", EventItem.Id.ToString()), EventItem);
        if (updateResult.IsSuccess)
        {
            _successMessage = "Événement mis à jour avec succès.";
            _errorMessage = null;
            Navigation.NavigateTo("/events");
        }
        else
        {
            _errorMessage = "Erreur lors de la mise à jour des informations de l'événement.";
        }
    }
}