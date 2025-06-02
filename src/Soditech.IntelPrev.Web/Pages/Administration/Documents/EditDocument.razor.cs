using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Soditech.IntelPrev.Mediatheques.Shared;
using Soditech.IntelPrev.Mediatheques.Shared.Documents;
using Soditech.IntelPrev.Users.Shared;
using Soditech.IntelPrev.Users.Shared.Users;
using Soditech.IntelPrev.Web.Models.Utils;
using Soditech.IntelPrev.Web.Services.Cache;

namespace Soditech.IntelPrev.Web.Pages.Administration.Documents;

public partial class EditDocument
{
    [Parameter]
    public string DocumentId { get; set; } = string.Empty;

    public DocumentResult currentDocument { get; set; } = new();
    public string title { get; set; } = "Modifier le document";
    private string? successMessage;
    private string? errorMessage;
    private bool IsLoading = false;

    private const string DocumentsCacheKey = "Documents";
    private string GetDocumentCacheKey() => $"Document_{DocumentId}";

    protected override async Task OnInitializedAsync()
    {
        await LoadCurrentDocumentAsync();
    }

    private async Task LoadCurrentDocumentAsync()
    {
        IsLoading = true;
        var cacheKey = GetDocumentCacheKey();
        var (exists, cachedValue) = CacheService.Get(cacheKey);

        if (exists)
        {
            currentDocument = (DocumentResult)cachedValue;
        }
        else
        {
            await LoadCurrentDocumentFromApiAsync();
            CacheService.Set(cacheKey, currentDocument);
        }
        IsLoading = false;
    }

    private async Task LoadCurrentDocumentFromApiAsync()
    {
        try
        {
            var result = await ProxyService.GetAsync<DocumentResult>(
                MediathequeRoutes.Documents.GetById.Replace("{id:guid}", DocumentId));

            if (result.IsSuccess)
            {
                currentDocument = result.Value;
            }
            else
            {
                errorMessage = "Erreur de récupération du document.";
            }
        }
        catch (Exception ex)
        {
            errorMessage = $"Erreur: {ex.Message}";
        }
    }

    private async Task UpdateDocument()
    {
        if (currentDocument.Id == Guid.Empty)
        {
            errorMessage = "L'ID du document est invalide.";
            return;
        }

        var updateResult = await ProxyService.PostAsync<DocumentResult>(
            MediathequeRoutes.Documents.Update.Replace("{id:guid}", currentDocument.Id.ToString()), currentDocument);

        if (updateResult.IsSuccess)
        {
            successMessage = "Document mis à jour avec succès.";
            errorMessage = null;
            CacheService.Set(GetDocumentCacheKey(), currentDocument);
            CacheService.Set(DocumentsCacheKey, null);
            Navigation.NavigateTo("/documents");
        }
        else
        {
            errorMessage = "Erreur lors de la mise à jour du document.";
        }
    }

    private async Task HandleFileSelected(InputFileChangeEventArgs e)
    {
        var file = e.File;
        if (file != null)
        {
            using var stream = file.OpenReadStream(maxAllowedSize: 5242880);
            var buffer = new byte[file.Size];
            await stream.ReadAsync(buffer, 0, (int)file.Size);
            currentDocument.BlobFile = buffer;
        }
    }
}
