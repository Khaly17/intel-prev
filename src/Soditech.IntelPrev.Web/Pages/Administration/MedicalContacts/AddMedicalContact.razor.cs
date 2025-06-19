using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Soditech.IntelPrev.Prevensions.Shared;
using Soditech.IntelPrev.Prevensions.Shared.MedicalContacts;

namespace Soditech.IntelPrev.Web.Pages.Administration.MedicalContacts;

public partial class AddMedicalContact
{
    public MedicalContactResult NewMedicalContact { get; set; } = new();
    public string Title { get; set; } = "Ajouter un nouveau";
    public string? ErrorMessage { get; set; }
    public string? SuccessMessage { get; set; }
    [Inject] private ILogger<AddMedicalContact> Logger { get; set; } = default!;

    private async Task CreateMedicalContactAsync()
    {
        ErrorMessage = null;
        SuccessMessage = null;
        try
        {
            var result = await ProxyService.PostAsync<MedicalContactResult>(PreventionRoutes.MedicalContacts.Create, NewMedicalContact);

            if (result.IsSuccess)
            {
                SuccessMessage = "Le contact a été ajouté avec succès !";

                Navigation.NavigateTo("/medicalcontacts");
            }
            else
            {
                ErrorMessage = result.Error?.Message ?? "Une erreur est survenue lors de l'ajout du contact'";
                Logger.LogError("{code} : {message}", result.Error?.Code, result.Error?.Message);
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = "Une erreur interne est survenue lors de l'ajout du contact'.";
            Logger.LogError(ex, ErrorMessage);
        }
    }


}
