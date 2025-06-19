using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls;
using Soditech.IntelPrev.Mobile.Core.Dependency;
using Soditech.IntelPrev.Mobile.Models;
using Soditech.IntelPrev.Mobile.ViewModels.Base;
using Soditech.IntelPrev.Mobile.Views.Sensibilisation;

namespace Soditech.IntelPrev.Mobile.ViewModels.Sensibilisation;

public class RiskPreventionViewModel : MauiViewModel
{
	private Stream? _pdfDocumentStream;
	private readonly ILogger<RiskPreventionViewModel> _logger = DependencyResolver.GetRequiredService<ILogger<RiskPreventionViewModel>>();

	public Stream? PdfDocumentStream
	{
		get => _pdfDocumentStream;
		set => SetProperty(ref _pdfDocumentStream, value);
	}

	public ObservableCollection<PreventionTopic> PreventionTopics { get; } =
	[
		new()
		{
			Title = "La chaleur et l'été",
			DocumentPath = "prevention_chaleur.pdf",
			DocumentTitle = "Guide Chaleur"
		},

		new()
		{
			Title = "Les chutes",
			DocumentPath = "doc1.pdf",
			DocumentTitle = "Guide Chutes"
		},

		new()
		{
			Title = "Les accidents chimiques",
			DocumentPath = "prevention_chimique.pdf",
			DocumentTitle = "Guide Risques Chimiques"
		},

		new()
		{
			Title = "Les risques de glissade",
			DocumentPath = "risque_glissade.pdf",
			DocumentTitle = "Glissades"
		},

		new()
		{
			Title = "Les EPI",
			DocumentPath = "epi.pdf",
			DocumentTitle = "les epis"
		},

		new()
		{
			Title = "Les risques psychosociaux",
			DocumentPath = "risk_ps.pdf",
			DocumentTitle = "les risques"
		}
	];

	public ICommand OpenDocumentCommand => new AsyncRelayCommand<string>(OpenDocumentAsync);
	public ICommand PageAppearingCommand => new AsyncRelayCommand(async () => await InitializeAsync());


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
			await Shell.Current.DisplayAlert("Erreur", "Error d'ouverture du document.", "OK");
			_logger.LogError(ex, "Unable to open document");
		}
	}
}