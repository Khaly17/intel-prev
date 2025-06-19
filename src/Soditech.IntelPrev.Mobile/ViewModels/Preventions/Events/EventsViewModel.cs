using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls;
using Soditech.IntelPrev.Mobile.Core.Dependency;
using Soditech.IntelPrev.Mobile.ViewModels.Base;
using Soditech.IntelPrev.Prevensions.Shared;
using Soditech.IntelPrev.Prevensions.Shared.Events;
using Soditech.IntelPrev.Proxy;
using Syncfusion.Maui.Scheduler;

namespace Soditech.IntelPrev.Mobile.ViewModels.Preventions.Events;

public class EventsViewModel : MauiViewModel
{
	private bool _isRefreshing;


	public bool IsRefreshing
	{
		get => _isRefreshing;
		set => SetProperty(ref _isRefreshing, value);
	}

	public readonly ILogger<EventsViewModel> _logger;

	private IEnumerable<EventResult> _events;
	public IEnumerable<EventResult> Events
	{
		get => _events;
		set => SetProperty(ref _events, value);
	}
	private EventResult? _selectedEvent;
	public EventResult? SelectedEvent
	{
		get => _selectedEvent;
		set => SetProperty(ref _selectedEvent, value);
	}


	private readonly IProxyService _proxyClientService = DependencyResolver.GetRequiredService<IProxyService>();

	public ICommand PageAppearingCommand => new AsyncRelayCommand(async () => await InitializeAsync());

	public ICommand EventSelectedCommand => new RelayCommand<EventResult>(eventItem =>
	{
		if (eventItem != null)
		{
			SelectedEvent = eventItem;
			// Handle event selection if needed
			// Example: Navigate to event details or show more info
		}
	});

	// appointment list
	public IEnumerable<SchedulerAppointment> _appointments;
	public IEnumerable<SchedulerAppointment> Appointments
	{
		get => _appointments;
		set => SetProperty(ref _appointments, value);
	}


	public ICommand AppointmentTappedCommand => new RelayCommand<object>(appointmentInfo =>
	{
		if (appointmentInfo is EventResult eventItem)
		{
			SelectedEvent = eventItem;
			// Handle event selection
			// You could show details in a popup or navigate to a details page
		}
	});

	public IAsyncRelayCommand RefreshCommand => new AsyncRelayCommand(async () => await InitializeAsync());

	public ICommand EventTappedCommand { get; set; }

	public EventsViewModel(ILogger<EventsViewModel> logger)
	{
		_logger = logger;
		EventTappedCommand = new AsyncRelayCommand<SchedulerAppointment>(OnEventTapped);
	}

	private async Task OnEventTapped(SchedulerAppointment? appointment)
	{
		if (appointment != null)
		{
			try
			{
				// Ensure appointment has all required properties before navigation
				var navigationParameter = new Dictionary<string, object>
				{
					{ "Appointment", appointment }
				};
				await Shell.Current.GoToAsync(AppRoutes.EventDetailPage, navigationParameter);
			}
			catch (Exception ex)
			{
				// Add diagnostic logging
				_logger.LogError($"Error navigating to event details: {ex.Message}");
				// You might want to show an alert to the user
			}
		}
	}

	/// <inheritdoc />
	public override async Task InitializeAsync()
	{
		// get upcoming events from the server
		IsBusy = true;
		var eventsResult = await _proxyClientService.GetAsync<IEnumerable<EventResult>>(PreventionRoutes.Events.GetUpComing);
		if (eventsResult.IsSuccess)
		{
			// convert to appointments
			Appointments = new List<SchedulerAppointment>(eventsResult.Value.Select(e => new SchedulerAppointment
			{
				Id = e.Id.ToString(),
				StartTime = e.StartDate.DateTime,
				EndTime = e.EndDate.DateTime,
				Subject = e.Name,
				Location = e.Location,

			}).ToList());
		}
		else
		{
			//TODO: handle error

		}

		IsBusy = false;
	}
	private void Scheduler_AppointmentTapped(object sender, AppointmentDragOverEventArgs e)
	{
		if (e.Appointment != null)
		{
			AppointmentTappedCommand.Execute(e.Appointment);
		}
	}
}