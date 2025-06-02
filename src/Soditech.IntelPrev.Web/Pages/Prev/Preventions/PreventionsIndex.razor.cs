using Soditech.IntelPrev.Preventions.Shared.PreventionSetting;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Sensor6ty.Results;
using Soditech.IntelPrev.Preventions.Shared;
using Soditech.IntelPrev.Preventions.Shared.ProPrevSetting;
using Soditech.IntelPrev.Mediatheques.Shared.Documents;
using static Soditech.IntelPrev.Mediatheques.Shared.MediathequeRoutes;

namespace Soditech.IntelPrev.Web.Pages.Prev.Preventions;

public partial class PreventionsIndex
{
    [Inject]
    private ILogger<PreventionsIndex> Logger { get; set; } = default!;

    public PreventionContentResult newPreventionContentResult = new();
    public string? errorMessage { get; set; }
    public string? successMessage { get; set; }

    private string _value = string.Empty;
    private bool IsSaving => AddBtnLabel == "En cours ...";
    public string AddBtnLabel = "Ajouter";
    private bool IsLoading = false;
    private const string ContentCacheKey = "Content";
    public bool DisplaySpinnerOnLoading { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await LoadContentAsync();
    }

    private void OnValueChanged(string newValue)
    {
        _value = newValue;
    }

    private async Task SaveContent()
    {
        if (IsSaving)
            return;

        AddBtnLabel = "En cours ...";
        errorMessage = null;
        successMessage = null;

        try
        {
            newPreventionContentResult.Content = _value;

            if (AddBtnLabel == "Ajouter")
            {
                var result = await ProxyService.PostAsync<PreventionContentResult>(PreventionRoutes.PreventionSettings.UpdateDefinitionContent, newPreventionContentResult);

                HandleResult(result, "La prévention a été ajouté avec succès !");
            }
            else
            {
                var result = await ProxyService.PostAsync<PreventionContentResult>(PreventionRoutes.PreventionSettings.UpdateDefinitionContent, newPreventionContentResult);
                HandleResult(result, "La prévention a été modifié avec succès !");
            }

            CacheService.Set(ContentCacheKey, _value);
            await LoadContentAsync();
        }
        catch (Exception ex)
        {
            errorMessage = "Une erreur interne est survenue lors de l'ajout ou la modification.";
            Logger.LogError(ex, errorMessage);
        }
        finally
        {
            AddBtnLabel = AddBtnLabel == "Ajouter" ? "Ajouter" : "Modifier";
        }
    }

    private void HandleResult(TResult<PreventionContentResult>? result, string successMessageText)
    {
        if (result == null) 
        {
            errorMessage = "Le résultat du serveur est nul.";
            Logger.LogError("Service result is null.");
            return;
        }

        if (result.IsSuccess)
        {
            successMessage = successMessageText;
            _value = string.Empty;
            AddBtnLabel = "Ajouter";
        }
        else
        {
            errorMessage = result.Error?.Message ?? "Une erreur est survenue.";
            Logger.LogError("{code} : {message}", result.Error?.Code, result.Error?.Message);
        }
    }

    private async Task LoadContentFromApiAsync()
    {
        IsLoading = true;
        var result = await ProxyService.GetAsync<PreventionContentResult>(PreventionRoutes.PreventionSettings.GetDefinitionContent);
        if (result.IsSuccess)
        {
            _value = result.Value.Content;
            AddBtnLabel = string.IsNullOrEmpty(_value) ? "Ajouter" : "Modifier";
        }

        IsLoading = false;
    }

    private async Task LoadContentAsync()
    {
        IsLoading = true;
        var (exists, cachedValue) = CacheService.Get(ContentCacheKey);

        if (exists)
        {
            _value = (string)cachedValue;
        }
        else
        {
            await LoadContentFromApiAsync();
            CacheService.Set(ContentCacheKey, _value);
        }
        IsLoading = false;
    }
}
