using Microsoft.Maui.Controls;
using Syncfusion.Maui.Scheduler;
using Soditech.IntelPrev.Mobile.ViewModels.Preventions.Events;

namespace Soditech.IntelPrev.Mobile.Views.Preventions.Events;

public partial class EventsView : ContentPage
{
	public EventsView()
	{
		InitializeComponent();
		this.eventScheduler.Tapped += Scheduler_Tapped;
	}

	private void Scheduler_Tapped(object sender, SchedulerTappedEventArgs e)
	{
		if (BindingContext is EventsViewModel viewModel && e.Appointments != null && e.Appointments.Count > 0)
		{
			// Pass the first appointment from the tapped event as the parameter
			viewModel.EventTappedCommand.Execute(e.Appointments[0]);
		}
	}
}