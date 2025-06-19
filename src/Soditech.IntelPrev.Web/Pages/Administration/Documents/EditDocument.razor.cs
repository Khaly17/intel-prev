using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Soditech.IntelPrev.Mediatheques.Shared;
using Soditech.IntelPrev.Mediatheques.Shared.Documents;

namespace Soditech.IntelPrev.Web.Pages.Administration.Documents;

public partial class EditDocument
{
    [Parameter]
    public string DocumentId { get; set; } = string.Empty;

    public DocumentResult CurrentDocument { get; set; } = new();
    public string Title { get; set; } = "Modifier le document";
    private string? _successMessage;
    private string? _errorMessage;
    private bool _isLoading = false;

    private const string DocumentsCacheKey = "Documents";
    private string GetDocumentCacheKey() => $"Document_{DocumentId}";

    protected override async Task OnInitializedAsync()
    {
        await LoadCurrentDocumentAsync();
    }

    private async Task LoadCurrentDocumentAsync()
    {
        _isLoading = true;
        var cacheKey = GetDocumentCacheKey();
        var (exists, cachedValue) = CacheService.Get(cacheKey);

        if (exists)
        {
            CurrentDocument = (DocumentResult)cachedValue;
        }
        else
        {
            await LoadCurrentDocumentFromApiAsync();
            CacheService.Set(cacheKey, CurrentDocument);
        }
        _isLoading = false;
    }

    private async Task LoadCurrentDocumentFromApiAsync()
    {
        try
        {
            var result = await ProxyService.GetAsync<DocumentResult>(
                MediathequeRoutes.Documents.GetById.Replace("{id:guid}", DocumentId));

            if (result.IsSuccess)
            {
                CurrentDocument = result.Value;
            }
            else
            {
                _errorMessage = "Erreur de récupération du document.";
            }
        }
        catch (Exception ex)
        {
            _errorMessage = $"Erreur: {ex.Message}";
        }
    }

    private async Task UpdateDocumentAsync()
    {
        if (CurrentDocument.Id == Guid.Empty)
        {
            _errorMessage = "L'ID du document est invalide.";
            return;
        }

        var updateResult = await ProxyService.PostAsync<DocumentResult>(
            MediathequeRoutes.Documents.Update.Replace("{id:guid}", CurrentDocument.Id.ToString()), CurrentDocument);

        if (updateResult.IsSuccess)
        {
            _successMessage = "Document mis à jour avec succès.";
            _errorMessage = null;
            CacheService.Set(GetDocumentCacheKey(), CurrentDocument);
            CacheService.Set(DocumentsCacheKey, null);
            Navigation.NavigateTo("/documents");
        }
        else
        {
            _errorMessage = "Erreur lors de la mise à jour du document.";
        }
    }

    private async Task HandleFileSelectedAsync(InputFileChangeEventArgs e)
    {
        var file = e.File;
        if (file != null)
        {
            using var stream = file.OpenReadStream(maxAllowedSize: 5242880);
            var buffer = new byte[file.Size];
            await stream.ReadAsync(buffer, 0, (int)file.Size);
            CurrentDocument.BlobFile = buffer;
        }
    }
}
