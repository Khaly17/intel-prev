using Microsoft.AspNetCore.Components;
using Soditech.IntelPrev.Preventions.Shared;
using Soditech.IntelPrev.Preventions.Shared.MedicalContacts;

namespace Soditech.IntelPrev.Web.Pages.Administration.MedicalContacts;

public partial class AddMedicalContact
{
    public MedicalContactResult NewMedicalContact { get; set; } = new();
    public string title { get; set; } = "Ajouter un nouveau";
    public string? errorMessage { get; set; }
    public string? successMessage { get; set; }
    [Inject] private ILogger<AddMedicalContact> Logger { get; set; } = default!;

    private async Task CreateMedicalContact()
    {
        errorMessage = null;
        successMessage = null;
        try
        {
            var result = await ProxyService.PostAsync<MedicalContactResult>(PreventionRoutes.MedicalContacts.Create, NewMedicalContact);

            if (result.IsSuccess)
            {
                successMessage = "Le contact a été ajouté avec succès !";

                Navigation.NavigateTo("/medicalcontacts");
            }
            else
            {
                errorMessage = result.Error?.Message ?? "Une erreur est survenue lors de l'ajout du contact'";
                Logger.LogError("{code} : {message}", result.Error?.Code, result.Error?.Message);
            }
        }
        catch (Exception ex)
        {
            errorMessage = "Une erreur interne est survenue lors de l'ajout du contact'.";
            Logger.LogError(ex, errorMessage);
        }
    }


}
