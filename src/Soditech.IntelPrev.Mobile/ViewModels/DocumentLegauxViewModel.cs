using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls;
using Soditech.IntelPrev.Mobile.Core.Dependency;
using Soditech.IntelPrev.Mobile.ViewModels.Base;
using Soditech.IntelPrev.Mobile.Views.Sensibilisation;

namespace Soditech.IntelPrev.Mobile.ViewModels;

public class DocumentLegauxViewModel : MauiViewModel
{
	private DocumentItem _selectedDocument = null!;

	private readonly ILogger<DocumentLegauxViewModel> _logger =
		DependencyResolver.GetRequiredService<ILogger<DocumentLegauxViewModel>>();
	
	
	public DocumentItem SelectedDocument
	{
		get => _selectedDocument;
		set => SetProperty(ref _selectedDocument, value);
	}

	public ObservableCollection<DocumentItem> Documents { get; } = new();

	public ICommand PageAppearingCommand => new AsyncRelayCommand(async () => await InitializeAsync());
	public ICommand OpenDocumentCommand => new AsyncRelayCommand<string>(OpenDocumentAsync);
	public ICommand SelectionChangedCommand => new AsyncRelayCommand<DocumentItem>(HandleSelection);

	/// <inheritdoc />
	public override async Task InitializeAsync()
	{
		IsBusy = true;

		// Initialize the documents if not already populated
		if (Documents.Count == 0)
		{
			PopulateDocuments();
		}

		IsBusy = false;
	}

	private void PopulateDocuments()
	{
		Documents.Clear();

		Documents.Add(new DocumentItem
		{
			Title = "DUERP",
			Description = "Document Unique d'Évaluation des Risques Professionnels",
			Icon = "papier_juridique.png",
			FilePath = "doc1.pdf"
		});

		Documents.Add(new DocumentItem
		{
			Title = "Règlement intérieur de l'entreprise",
			Description = "Règlement définissant les droits et devoirs des employés",
			Icon = "papier_juridique.png",
			FilePath = "règlement_intérieur.pdf"
		});

		Documents.Add(new DocumentItem
		{
			Title = "Plan de prévention",
			Description = "Plan de prévention des risques professionnels",
			Icon = "papier_juridique.png",
			FilePath = "prevention_chaleur.pdf"
		});
	}

	private async Task HandleSelection(DocumentItem item)
	{
		if (item != null)
		{
			await OpenDocumentAsync(item.FilePath);

			// Reset selection after a short delay to allow reselection
			await Task.Delay(300);
			SelectedDocument = null;
		}
	}

	private async Task OpenDocumentAsync(string? documentPath)
	{
		if (string.IsNullOrWhiteSpace(documentPath))
			return;

		try
		{
			IsBusy = true;
			await Shell.Current.GoToAsync(nameof(PdfViewerView), true,
				new Dictionary<string, object>
				{
					{ "DocumentPath", documentPath }
				});
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlert("Erreur", "Impossible d'ouvrir le document", "OK");
			_logger.LogError(ex, "Unable to open document");
		}
		finally
		{
			IsBusy = false;
		}
	}
}

// Model class for document items
public class DocumentItem
{
	public string Title { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public string Icon { get; set; } = string.Empty;
	public string FilePath { get; set; } = string.Empty;
}