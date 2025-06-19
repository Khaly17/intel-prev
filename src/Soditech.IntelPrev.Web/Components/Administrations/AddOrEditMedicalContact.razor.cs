using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Soditech.IntelPrev.Prevensions.Shared.MedicalContacts;

namespace Soditech.IntelPrev.Web.Components.Administrations;

public partial class AddOrEditMedicalContact
{
    [Parameter]
    public MedicalContactResult NewMedicalContact { get; set; } = new();

    [Parameter]
    public string Title { get; set; } = "Ajouter un nouveau";
    [Parameter]
    public string? ErrorMessage { get; set; }

    [Parameter]
    public string? SuccessMessage { get; set; }

    [Parameter]
    public EventCallback OnMedicalContactCreated { get; set; }
    public async Task CreateMedicalContactAsync()
    {
        await OnMedicalContactCreated.InvokeAsync(null);
    }
}
