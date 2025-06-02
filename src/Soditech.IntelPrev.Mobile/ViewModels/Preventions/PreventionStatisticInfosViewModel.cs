using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Soditech.IntelPrev.Mobile.ViewModels.Base;
using Soditech.IntelPrev.Mobile.Views.Preventions.PreventionStatisticInfos;

namespace Soditech.IntelPrev.Mobile.ViewModels.Preventions;

public class PreventionStatisticInfosViewModel : MauiViewModel
{
   
	public ICommand GoToDevViewCommand => new RelayCommand(GoToDevViewAsync);

	private async void GoToDevViewAsync()
	{
		await Shell.Current.GoToAsync(new ShellNavigationState($"{AppRoutes.DevPage}"));
	}
    
}