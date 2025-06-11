using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Soditech.IntelPrev.Mobile.ViewModels.Base;
using Soditech.IntelPrev.Preventions.Shared.Floors;
using Soditech.IntelPrev.Preventions.Shared;
using Soditech.IntelPrev.Mobile.Core.Dependency;
using Soditech.IntelPrev.Proxy;
using Soditech.IntelPrev.Preventions.Shared.PreventionSetting;

namespace Soditech.IntelPrev.Mobile.ViewModels.Preventions;

public class DefinitionPreventionViewModel : MauiViewModel
{

	private string _definitionText = "La prévention des risques professionnels, c’est l’ensemble des dispositions à mettre" +
									 " \r\nEn œuvre pour préserver la santé et la sécurité des salariés, " +
									 " \r\naméliorer les conditions de travail et tendre au bien-être au travail.";
	private readonly IProxyService _proxyClientService = DependencyResolver.GetRequiredService<IProxyService>();

	public override async Task InitializeAsync()
	{
		IsBusy = true;
		await GetContentAsync();
		IsBusy = false;
	}

	public string DefinitionText
	{
		get => _definitionText;
		set
		{
			if (_definitionText == value) return;
			_definitionText = value;
			_definitionText = $"<html><head><style>body {{ font-size: 18px; color: #424242; line-height: 1.6; }}</style></head><body>{_definitionText}</body></html>";
			OnPropertyChanged();
		}
	}

	public ICommand ToggleCausesCommand => new Command(() => IsCausesExpanded = !IsCausesExpanded);

	public bool IsCausesExpanded
	{
		get => _isCausesExpanded;
		set
		{
			_isCausesExpanded = value;
			OnPropertyChanged();
			OnPropertyChanged(nameof(IsCausesCollapsed));
			OnPropertyChanged(nameof(CausesToggleText));
		}
	}

	public bool IsCausesCollapsed => !IsCausesExpanded;

	private bool _isCausesExpanded = false;
	public string CausesToggleText => IsCausesExpanded ? "Voir moins" : "Voir plus...";
	//public bool IsOverviewCollapsed => !IsOverviewExpanded;



	public ICommand PageAppearingCommand => new AsyncRelayCommand(async () => await InitializeAsync());

	private async Task GetContentAsync()
	{
		var result = await _proxyClientService.GetAsync<PreventionContentResult>(PreventionRoutes.PreventionSettings.GetDefinitionContent);
		if (result.IsSuccess && result.Value != null)
		{
			DefinitionText = result.Value.Content ?? "Aucune donnée disponible.";
		}
	}

}