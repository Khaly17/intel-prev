using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Soditech.IntelPrev.Prevensions.Shared;
using Soditech.IntelPrev.Prevensions.Shared.MedicalContacts;

namespace Soditech.IntelPrev.Web.Pages.Administration.MedicalContacts;

public partial class EditMedicalContact
{
    [Parameter]
    public string MedicalContactId { get; set; } = string.Empty;
    public string Title { get; set; } = "Modification du contact médical";
    private MedicalContactResult MedicalContact { get; set; } = new();

    private string? _successMessage;
    private string? _errorMessage;
    private bool _isLoading = false;
    private const string MedicalContactsCacheKey = "MedicalContacts";

    private string GetMedicalContactCacheKey() => $"MedicalContact_{MedicalContactId}";

    [Inject] private ILogger<EditMedicalContact> Logger { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await LoadMedicalContactsAsync();
    }

    private async Task LoadMedicalContactsAsync()
    {
        _isLoading = true;
        var cacheKey = GetMedicalContactCacheKey();
        var (exists, cachedValue) = CacheService.Get(cacheKey);

        if (exists)
        {
            MedicalContact = (MedicalContactResult)cachedValue;
        }
        else
        {
            await LoadMedicalContactsFromApiAsync();
            CacheService.Set(cacheKey, MedicalContact);
        }
        _isLoading = false;
    }

    private async Task LoadMedicalContactsFromApiAsync()
    {
        try
        {
            _isLoading = true;
            var result = await ProxyService.GetAsync<MedicalContactResult>(
                PreventionRoutes.MedicalContacts.GetById.Replace("{id:guid}", MedicalContactId));

            if (result.IsSuccess)
            {
                MedicalContact = result.Value;
            }
            else
            {
                _errorMessage = "Erreur de récupération des informations du contact médical.";
            }
        }
        catch (Exception ex)
        {
            _errorMessage = $"Erreur: {ex.Message}";
        }

        _isLoading = false;
    }

    private async Task UpdateMedicalContact()
    {
        if (MedicalContact.Id == Guid.Empty)
        {
            _errorMessage = "L'ID du contact médical est invalide.";
            return;
        }

        var updateResult = await ProxyService.PostAsync<MedicalContactResult>(
            PreventionRoutes.MedicalContacts.Update.Replace("{id:guid}", MedicalContact.Id.ToString()), MedicalContact);

        if (updateResult.IsSuccess)
        {
            _successMessage = "Contact médical mis à jour avec succès.";
            _errorMessage = null;
            CacheService.Set(GetMedicalContactCacheKey(), MedicalContact);
            CacheService.Set(MedicalContactsCacheKey, null);
            Navigation.NavigateTo("/medicalcontacts");
        }
        else
        {
            _errorMessage = "Erreur lors de la mise à jour des informations du contact médical.";
        }
    }
}
