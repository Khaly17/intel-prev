using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Soditech.IntelPrev.Mobile.Models;
using Soditech.IntelPrev.Mobile.ViewModels.Base;
using Soditech.IntelPrev.Mobile.Views.Sensibilisation;

namespace Soditech.IntelPrev.Mobile.ViewModels.Sensibilisation
{
	public partial class RiskPreventionViewModel : MauiViewModel
	{
		private Stream? _pdfDocumentStream;

		public Stream? PdfDocumentStream
		{
			get => _pdfDocumentStream;
			set => SetProperty(ref _pdfDocumentStream, value);
		}

		public ObservableCollection<PreventionTopic> PreventionTopics { get; } = new()
		{
			new PreventionTopic {
				Title = "La chaleur et l'été",
				DocumentPath = "prevention_chaleur.pdf",
				DocumentTitle = "Guide Chaleur"
			},
			new PreventionTopic {
				Title = "Les chutes",
				DocumentPath = "doc1.pdf",
				DocumentTitle = "Guide Chutes"
			},
			new PreventionTopic {
				Title = "Les accidents chimiques",
				DocumentPath = "prevention_chimique.pdf",
				DocumentTitle = "Guide Risques Chimiques"
			},
			new PreventionTopic
			{
				Title = "Les risques de glissade",
				DocumentPath = "risque_glissade.pdf",
				DocumentTitle = "Glissades"
			},
			new PreventionTopic
			{
				Title = "Les EPI",
				DocumentPath = "epi.pdf",
				DocumentTitle = "les epis"
			},
			new PreventionTopic
			{
				Title = "Les risques psychosociaux",
				DocumentPath = "risk_ps.pdf",
				DocumentTitle = "les risques"
			}
		};

		public ICommand OpenDocumentCommand => new AsyncRelayCommand<string>(OpenDocumentAsync);
		public ICommand PageAppearingCommand => new AsyncRelayCommand(async () => await InitializeAsync());

		public RiskPreventionViewModel()
		{
		}


		private async Task OpenDocumentAsync(string? documentPath)
		{
			if (string.IsNullOrEmpty(documentPath))
				return;

			try
			{
				await Shell.Current.GoToAsync($"{nameof(PdfViewerView)}", true, new Dictionary<string, object>
				{
					{ "DocumentPath", documentPath }
				});
			}
			catch (Exception ex)
			{
				await Shell.Current.DisplayAlert("Error", "Unable to open document", "OK");
			}
		}
	}
}