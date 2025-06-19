using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Soditech.IntelPrev.Mediatheques.Shared.Documents;

namespace Soditech.IntelPrev.Web.Components.Administrations;

public partial class AddOrEditDocument
{
    [Parameter]
    public DocumentResult DocumentCommand { get; set; } = default!;

    [Parameter]
    public string Title { get; set; } = "Créer un document";

    [Parameter]
    public string? ErrorMessage { get; set; }

    [Parameter]
    public string? SuccessMessage { get; set; }

    [Parameter]
    public EventCallback OnCreateDocument { get; set; }

    [Parameter]
    public EventCallback<InputFileChangeEventArgs> OnSeletectedFileChange { get; set; }

    public async Task CreateDocumentAsync() 
    {
        await OnCreateDocument.InvokeAsync(null);
    }

    public async Task SelectedFileChangeAsync(InputFileChangeEventArgs e)
    {
        await OnSeletectedFileChange.InvokeAsync(e);
    }
}
