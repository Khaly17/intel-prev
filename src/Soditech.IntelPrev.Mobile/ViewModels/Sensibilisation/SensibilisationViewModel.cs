using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Soditech.IntelPrev.Mobile.Models;
using Soditech.IntelPrev.Mobile.ViewModels.Base;
using Soditech.IntelPrev.Mobile.Views.Sensibilisation;
using Soditech.IntelPrev.Preventions.Shared.PreventionSetting;
using Soditech.IntelPrev.Preventions.Shared;
using Soditech.IntelPrev.Proxy;
using Soditech.IntelPrev.Mobile.Core.Dependency;

namespace Soditech.IntelPrev.Mobile.ViewModels.Sensibilisation
{
	public class SensibilisationViewModel : MauiViewModel
	{
		private readonly IProxyService _proxyClientService = DependencyResolver.GetRequiredService<IProxyService>();

		private string _sensibilisationDescription;
		private ObservableCollection<RiskCategory> _riskCategories;
		private ObservableCollection<PreventionTip> _preventionTips;

		public string OverviewTitle => "Sensibilisation";

		public string SensibilisationDescription
		{
			get => _sensibilisationDescription;
			set
			{
				if (_sensibilisationDescription == value) return;
				_sensibilisationDescription = value;
				OnPropertyChanged();
			}
		}

		public ObservableCollection<RiskCategory> RiskCategories
		{
			get => _riskCategories;
			set
			{
				if (_riskCategories == value) return;
				_riskCategories = value;
				OnPropertyChanged();
			}
		}

		public ObservableCollection<PreventionTip> PreventionTips
		{
			get => _preventionTips;
			set
			{
				if (_preventionTips == value) return;
				_preventionTips = value;
				OnPropertyChanged();
			}
		}

		public ICommand PageAppearingCommand => new AsyncRelayCommand(async () => await InitializeAsync());

		public ICommand NavigateToRiskPreventionCommand => new AsyncRelayCommand(GotoRiskPreventionAsync);

		private async Task GotoRiskPreventionAsync()
		{
			await Shell.Current.GoToAsync(new ShellNavigationState(nameof(RiskPreventionTipsView)));
		}
		public SensibilisationViewModel(string sensibilisationDescription, ObservableCollection<RiskCategory> riskCategories, ObservableCollection<PreventionTip> preventionTips)
		{
			_sensibilisationDescription = sensibilisationDescription;
			_riskCategories = riskCategories;
			_preventionTips = preventionTips;
			InitializeCollections();
		}

		private void InitializeCollections()
		{
			SensibilisationDescription = "informer et sensibiliser :\n" +
				"Les actions d'informations mises en place par l'équipe de prévention " +
				"ont pour objectif de sensibiliser les salariés sur les questions de santé et " +
				"de sécurité au travail.\n\n" +
				"Il s'agit de mutualiser les informations afin que régulièrement les salariés " +
				"puissent avoir des connaissances sur les risques, la prévention, et de " +
				"contribuer au développement de la culture de prévention dans l'entreprise.";

			RiskCategories = new ObservableCollection<RiskCategory>
			{
				new RiskCategory { Name = "Physique", IconSource = "physical_risk.jpg" },
				new RiskCategory { Name = "Risques liés aux gestes répétitifs", IconSource = "repetitive_risk.png" },
				new RiskCategory { Name = "Risques liés aux déplacements", IconSource = "travel_risk.png" },
				new RiskCategory { Name = "Risques biologiques", IconSource = "biological_risk.png" },
				new RiskCategory { Name = "Risques chimiques", IconSource = "chemical_risk.png" },
				new RiskCategory { Name = "Risques liés aux conditions de sécurité", IconSource = "safety_risk.png" },
				new RiskCategory { Name = "Risques psychosociaux (RPS)", IconSource = "psycho_risk.png" },
				new RiskCategory { Name = "Risques liés aux machines et équipements", IconSource = "machine_risk.png" }
			};

			PreventionTips = new ObservableCollection<PreventionTip>
			{
				new PreventionTip { TipText = "Donner du sens à la sécurité au travail", TutorialLink = "TutorialPage1" },
				new PreventionTip { TipText = "La formation des salariés", TutorialLink = "TutorialPage2" },
				new PreventionTip { TipText = "Le rappel des messages de sensibilisation", TutorialLink = "TutorialPage3" },
				new PreventionTip { TipText = "L'implication de tous les acteurs de l'entreprise", TutorialLink = "TutorialPage4" },
				new PreventionTip { TipText = "La planification des actions de prévention et de sensibilisation", TutorialLink = "TutorialPage5" },
				new PreventionTip { TipText = "L'identification des risques et des dangers", TutorialLink = null }
			};
		}

		public override async Task InitializeAsync()
		{
			await SetBusyAsync(() =>
			{
				_ = GetContentAsync();
				return Task.CompletedTask;
			});
		}

		private async Task GetContentAsync()
		{
			var result = await _proxyClientService.GetAsync<PreventionContentResult>(PreventionRoutes.PreventionSettings.GetSensibilisationContent);
			if (result.IsSuccess && result.Value != null)
			{
				SensibilisationDescription = result.Value.Content;
			}
		}
	}
}

