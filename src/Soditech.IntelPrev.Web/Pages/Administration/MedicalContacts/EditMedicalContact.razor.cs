using Microsoft.AspNetCore.Components;
using Soditech.IntelPrev.Preventions.Shared.MedicalContacts;
using Soditech.IntelPrev.Preventions.Shared;
using Soditech.IntelPrev.Web.Services.Cache;

namespace Soditech.IntelPrev.Web.Pages.Administration.MedicalContacts;

public partial class EditMedicalContact
{
    [Parameter]
    public string medicalContactId { get; set; } = string.Empty;
    public string title { get; set; } = "Modification du contact médical";
    private MedicalContactResult medicalContact { get; set; } = new MedicalContactResult();

    private string? successMessage;
    private string? errorMessage;
    private bool IsLoading = false;
    private const string MedicalContactsCacheKey = "MedicalContacts";

    private string GetMedicalContactCacheKey() => $"MedicalContact_{medicalContactId}";

    [Inject] private ILogger<EditMedicalContact> Logger { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await LoadMedicalContactsAsync();
    }

    private async Task LoadMedicalContactsAsync()
    {
        IsLoading = true;
        var cacheKey = GetMedicalContactCacheKey();
        var (exists, cachedValue) = CacheService.Get(cacheKey);

        if (exists)
        {
            medicalContact = (MedicalContactResult)cachedValue;
        }
        else
        {
            await LoadMedicalContactsFromApiAsync();
            CacheService.Set(cacheKey, medicalContact);
        }
        IsLoading = false;
    }

    private async Task LoadMedicalContactsFromApiAsync()
    {
        try
        {
            IsLoading = true;
            var result = await ProxyService.GetAsync<MedicalContactResult>(
                PreventionRoutes.MedicalContacts.GetById.Replace("{id:guid}", medicalContactId));

            if (result.IsSuccess)
            {
                medicalContact = result.Value;
            }
            else
            {
                errorMessage = "Erreur de récupération des informations du contact médical.";
            }
        }
        catch (Exception ex)
        {
            errorMessage = $"Erreur: {ex.Message}";
        }

        IsLoading = false;
    }

    private async Task UpdateMedicalContact()
    {
        if (medicalContact.Id == Guid.Empty)
        {
            errorMessage = "L'ID du contact médical est invalide.";
            return;
        }

        var updateResult = await ProxyService.PostAsync<MedicalContactResult>(
            PreventionRoutes.MedicalContacts.Update.Replace("{id:guid}", medicalContact.Id.ToString()), medicalContact);

        if (updateResult.IsSuccess)
        {
            successMessage = "Contact médical mis à jour avec succès.";
            errorMessage = null;
            CacheService.Set(GetMedicalContactCacheKey(), medicalContact);
            CacheService.Set(MedicalContactsCacheKey, null);
            Navigation.NavigateTo("/medicalcontacts");
        }
        else
        {
            errorMessage = "Erreur lors de la mise à jour des informations du contact médical.";
        }
    }
}
