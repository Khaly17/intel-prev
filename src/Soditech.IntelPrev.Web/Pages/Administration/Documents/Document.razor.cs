    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Forms;
    using Soditech.IntelPrev.Mediatheques.Shared.Documents;
    using Soditech.IntelPrev.Users.Shared.Tenants;
    using Soditech.IntelPrev.Users.Shared;
    using Soditech.IntelPrev.Web.Models.Utils;
    using Soditech.IntelPrev.Web.Pages.Administration.Tenants;
    using Soditech.IntelPrev.Mediatheques.Shared;

    namespace Soditech.IntelPrev.Web.Pages.Administration.Documents;

    public partial class Document: ComponentBase
    {

        private DocumentResult documentCommand = new();
        public string? errorMessage { get; set; }

        public string? successMessage { get; set; }
        [Inject] private ILogger<Document> Logger { get; set; } = default!;

        private async Task CreateDocument()
        {
            errorMessage = null;
            successMessage = null;
            try
            {
                var result = await ProxyService.PostAsync<DocumentResult>(MediathequeRoutes.Documents.Create, documentCommand);

                if (result.IsSuccess)
                {
                    successMessage = "La document a été ajouté avec succès !";
                    Navigation.NavigateTo("/documents");
                }
                else
                {
                    errorMessage = result.Error?.Message ?? "Une erreur est survenue lors de la création du document.";
                    Logger.LogError("{code} : {message}", result.Error?.Code, result.Error?.Message);
                }
            }
            catch (Exception ex)
            {
                errorMessage = "Une erreur interne est survenue lors de la création du document.";
                Logger.LogError(ex, errorMessage);
            }
        }

        private async Task HandleFileSelected(InputFileChangeEventArgs e)
        {
            var file = e.File;
            if (file != null)
            {
            documentCommand.Extension = Path.GetExtension(file.Name);
            using (var stream = file.OpenReadStream(maxAllowedSize: 5242880))
                {
                    var buffer = new byte[file.Size];
                    await stream.ReadAsync(buffer, 0, (int)file.Size);
                    documentCommand.BlobFile = buffer;
                }
        }
    }

    }
