using Microsoft.AspNetCore.Components;
using Soditech.IntelPrev.Mediatheques.Shared;
using Soditech.IntelPrev.Mediatheques.Shared.Documents;
using Soditech.IntelPrev.Users.Shared;
using Soditech.IntelPrev.Users.Shared.Users;
using Soditech.IntelPrev.Web.Pages.Administration.Users;
using Syncfusion.Blazor.Grids;
using System.ComponentModel;

namespace Soditech.IntelPrev.Web.Pages.Administration.Documents
{
    public partial class DocumentsIndex
    {

        [Inject] private ILogger<DocumentsIndex> Logger { get; set; } = default!;
        private IList<DocumentResult> Documents { get; set; } = [];
        private int PageCount { get; set; } = 10;
        private int PageSize { get; set; } = 10;
        private bool IsLoading { get; set; }


        private static List<GridColumn> Columns =>
        [
            new GridColumn { Field = nameof(DocumentResult.Name), HeaderText = "Nom" },
            new GridColumn { Field = nameof(DocumentResult.Type), HeaderText = "Type" },
            new GridColumn { Field = nameof(DocumentResult.FileType), HeaderText = "Type de fichier" },
            new GridColumn { Field = nameof(DocumentResult.IsDownloadable), HeaderText = "Téléchargeable" , DisplayAsCheckBox = true}
        ];

        private static List<string> ToolbarItems => ["Search", "ExcelExport", "PdfExport", "Print"];

        private bool _isDeleteModalVisible;
        private string _alertMessage = string.Empty;
        private string _alertType = "success";
        private bool _isAlertVisible;
        private bool _isResetPasswordModalVisible;
        private bool _isDisableDocumentModalVisible;
        private const string DocumentsCacheKey = "Documents";

        private DocumentResult SelectedDocument { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            await LoadDocumentsAsync();
        }

        private void AddDocument()
        {
            Navigation.NavigateTo("/documents/add");
        }
        private void GoToEdit(DocumentResult document)
        {
            Navigation.NavigateTo($"documents/edit/{document.Id}");

        }

        private async Task LoadDocumentsAsync()
        {
            IsLoading = true;
            var (exists, cachedValue) = CacheService.Get(DocumentsCacheKey);

            if (exists)
            {
                Documents = (IList<DocumentResult>)cachedValue;
            }
            else
            {
                await LoadDocumentsFromApiAsync();
                CacheService.Set(DocumentsCacheKey, Documents);
            }
            IsLoading = false;
        }
        private async Task LoadDocumentsFromApiAsync()
        {

            IsLoading = true;
            var documentsResult = await ProxyService.GetAsync<IList<DocumentResult>>(MediathequeRoutes.Documents.GetAll);

            if (documentsResult.IsSuccess)
            {
                Documents = documentsResult.Value;
            }
            IsLoading = false;
        }

        private void ShowAlert(string message, string type = "success")
        {
            _alertMessage = message;
            _alertType = type;
            _isAlertVisible = true;
            Task.Delay(3000).ContinueWith(_ =>
            {
                _isAlertVisible = false;
                StateHasChanged();
            });
        }

        private void DeleteDocument(DocumentResult document)
        {
            SelectedDocument = document;
            _isDeleteModalVisible = true;
        }
        private void HideDeleteModal() => _isDeleteModalVisible = false;

        private async Task DeleteDocumentAsync()
        {
            try
            {
                var result = await ProxyService.DeleteAsync(MediathequeRoutes.Documents.Delete.Replace("{id:guid}", SelectedDocument.Id.ToString()));

                if (result.IsSuccess)
                {
                    Documents = Documents.Where(n => n.Id != SelectedDocument.Id).ToList();
                    ShowAlert("Document supprimé avec succès.");
                }
                else
                {
                    ShowAlert("Échec de la suppression du document.", "danger");
                }

            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Erreur lors de la suppression du document : {Message}", ex.Message);
                ShowAlert("Une erreur s'est produite lors de la suppression du document. Veuillez réessayer.", "danger");
            }
            finally
            {
                HideDeleteModal();
            }
        }

    }
}
