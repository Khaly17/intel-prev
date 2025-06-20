    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Forms;
    using Microsoft.Extensions.Logging;
    using Soditech.IntelPrev.Mediatheques.Shared.Documents;
    using Soditech.IntelPrev.Mediatheques.Shared;
using System.Linq;

namespace Soditech.IntelPrev.Web.Pages.Administration.Documents;

    public partial class Document: ComponentBase
    {

        private readonly DocumentResult _documentCommand = new(); // "readonly" to fix CS-R1137
        private string? ErrorMessage { get; set; }

        private string? SuccessMessage { get; set; }
        [Inject] private ILogger<Document> Logger { get; set; } = default!;

        private async Task CreateDocument()
        {
            ErrorMessage = null;
            SuccessMessage = null;
            try
            {
                var result = await ProxyService.PostAsync<DocumentResult>(MediathequeRoutes.Documents.Create, _documentCommand);

                if (result.IsSuccess)
                {
                    SuccessMessage = "La document a été ajouté avec succès !";
                    Navigation.NavigateTo("/documents");
                }
                else
                {
                    ErrorMessage = !string.IsNullOrEmpty(result.Error.Message) ? result.Error.Message : "Une erreur est survenue lors de la création du document.";
                    Logger.LogError("{code} : {message}", result.Error.Code, result.Error?.Message);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Une erreur interne est survenue lors de la création du document.";
                Logger.LogError(ex, ErrorMessage);
            }
        }

        private async Task HandleFileSelectedAsync(InputFileChangeEventArgs e)
        {
            var file = e.File;
            if (file.Size != 0)
            {
                _documentCommand.Extension = Path.GetExtension(file.Name);
                await using var stream = file.OpenReadStream(); //maxAllowedSize: 5242880
                var buffer = new byte[file.Size];
                await stream.ReadExactlyAsync(buffer, 0, (int)file.Size);
                _documentCommand.BlobFile = [.. buffer];
            }
        }

    }
