using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Soditech.IntelPrev.Mobile.ViewModels.Base;

namespace Soditech.IntelPrev.Mobile.ViewModels.Preventions;

public class PreventionStatisticInfosViewModel : MauiViewModel
{
   
	public ICommand GoToDevViewCommand => new AsyncRelayCommand(GoToDevViewAsync);

	private async Task GoToDevViewAsync()
	{
		await Shell.Current.GoToAsync(new ShellNavigationState($"{AppRoutes.DevPage}"));
	}
    
}