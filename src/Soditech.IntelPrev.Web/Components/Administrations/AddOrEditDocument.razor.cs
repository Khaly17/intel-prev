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
    public EventCallback onCreateDocument { get; set; }

    [Parameter]
    public EventCallback<InputFileChangeEventArgs> onSeletectedFileChange { get; set; }

    public async Task CreateDocument() 
    {
        await onCreateDocument.InvokeAsync(null);
    }

    public async Task SelectedFileChange(InputFileChangeEventArgs e)
    {
        await onSeletectedFileChange.InvokeAsync(e);
    }
}
