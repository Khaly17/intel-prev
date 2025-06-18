using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Syncfusion.Blazor.Schedule;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Soditech.IntelPrev.Prevensions.Shared;
using Soditech.IntelPrev.Prevensions.Shared.CommitteeMembers;
using Soditech.IntelPrev.Prevensions.Shared.Events;

namespace Soditech.IntelPrev.Web.Pages.Administration.Calendars;

public partial class Calendar
{
    [Inject] private ILogger<Calendar> Logger { get; set; } = default!;

    DateTime _currentDate = new(2025, 2, 14);
    List<EventResult> _dataSource = new();
    public IList<CommitteeMemberResult> Organizers { get; set; } = new List<CommitteeMemberResult>();
    public string? ErrorMessage { get; set; }
    public string? SuccessMessage { get; set; }
    private bool IsLoading { get; set; }

    ValidationRules _validationRules = new() { Required = true };

    protected override async Task OnInitializedAsync()
    {
        await GetEvents();
        await GetCommitteeMembers();
    }

    public async Task GetCommitteeMembers()
    {
        IsLoading = true;
        var committeeMembersResult = await ProxyService.GetAsync<IList<CommitteeMemberResult>>(PreventionRoutes.CommitteeMembers.GetAll);

        if (committeeMembersResult.IsSuccess)
        {
            Organizers = committeeMembersResult.Value;
        }
        IsLoading = false;
    }

    private async Task GetEvents()
    {
        IsLoading = true;
        var eventsResult = await ProxyService.GetAsync<List<EventResult>>(PreventionRoutes.Events.GetAll);

        if (eventsResult.IsSuccess)
        {
            _dataSource = eventsResult.Value;
            foreach (var d in _dataSource)
            {
                d.StartTime = d.StartDate.DateTime; 
                d.EndTime = d.EndDate.DateTime;    
            }
        }

        IsLoading = false;
    }

    public void OnActionCompleted(ActionEventArgs<EventResult> args)
    {
        if (args.ActionType == ActionType.EventCreate)
        {
            if (args.AddedRecords != null)
            {
                foreach (var newEvent in args.AddedRecords)
                {
                    _ = CreateEvent(newEvent);
                }
            }
        }
        else if (args.ActionType == ActionType.EventChange)
        {
            if (args.ChangedRecords != null)
            {
                foreach (var updatedEvent in args.ChangedRecords)
                {
                    var existingEvent = _dataSource.FirstOrDefault(e => e.Id == updatedEvent.Id);
                    if (existingEvent != null)
                    {
                        existingEvent.Name = updatedEvent.Name;
                        existingEvent.StartDate = new DateTimeOffset(updatedEvent.StartTime); 
                        existingEvent.EndDate = new DateTimeOffset(updatedEvent.EndTime);   
                    }
                    _ = UpdateEvent(updatedEvent);
                }
            }
        }
        else if (args.ActionType == ActionType.EventRemove) 
        {
            if (args.DeletedRecords != null)
            {
                foreach (var deletedEvent in args.DeletedRecords)
                {
                    _ = DeleteEvent(deletedEvent);
                }
            }
        }
    }

    private async Task CreateEvent(EventResult newEvent)
    {
        ErrorMessage = null;
        SuccessMessage = null;
        try
        {
            newEvent.StartDate = new DateTimeOffset(newEvent.StartTime);
            newEvent.EndDate = new DateTimeOffset(newEvent.EndTime);     
            var result = await ProxyService.PostAsync<EventResult>(PreventionRoutes.Events.Create, newEvent);

            if (result.IsSuccess)
            {
                SuccessMessage = "L'événement a été ajouté avec succès !";
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

    private async Task UpdateEvent(EventResult updatedEvent)
    {

        var updateResult = await ProxyService.PostAsync<EventResult>(PreventionRoutes.Events.Update.Replace("{id:guid}", updatedEvent.Id.ToString()), updatedEvent);
        if (updateResult.IsSuccess)
        {
            SuccessMessage = "Événement mis à jour avec succès.";
            ErrorMessage = null;
        }
        else
        {
            ErrorMessage = "Erreur lors de la mise à jour des informations de l'événement.";
        }
    }

    private async Task DeleteEvent(EventResult deletedEvent)
    {
        try
        {
            var result = await ProxyService.DeleteAsync(PreventionRoutes.Events.Delete.Replace("{id:guid}", deletedEvent.Id.ToString()));

            if (result.IsSuccess)
            {
                SuccessMessage = "Event deleted successfully.";
            }
            else
            {
                ErrorMessage = "Failed to delete event.";
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error deleting event: {Message}", ex.Message);
            ErrorMessage = "An error occurred while deleting the event. Please try again.";
        }
    }
}