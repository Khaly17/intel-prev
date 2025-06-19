using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Syncfusion.Maui.Scheduler;
using Soditech.IntelPrev.Mobile.ViewModels.Base;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;

namespace Soditech.IntelPrev.Mobile.ViewModels.Preventions.Events;

public class EventDetailViewModel : MauiViewModel, IQueryAttributable
{
	private SchedulerAppointment _event;

	public SchedulerAppointment Event
	{
		get => _event;
		set
		{
			SetProperty(ref _event, value);
			OnPropertyChanged(nameof(HasLocation));
			OnPropertyChanged(nameof(HasNotes));
		}
	}

	// Added properties to safely check for null values
	public bool HasLocation => !string.IsNullOrWhiteSpace(Event?.Location);
	public bool HasNotes => !string.IsNullOrWhiteSpace(Event?.Notes);

	public ICommand CloseCommand { get; }

	public EventDetailViewModel()
	{
		CloseCommand = new AsyncRelayCommand(OnClose);
	}

	private async Task OnClose()
	{
		await Shell.Current.GoToAsync("..");
	}

	public void ApplyQueryAttributesAsync(IDictionary<string, object> query)
	{
		if (query.TryGetValue("Appointment", out var appointment) && appointment is SchedulerAppointment schedulerAppointment)
		{
			Event = schedulerAppointment;
		}
	}
}