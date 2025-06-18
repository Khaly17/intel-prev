using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Sensor6ty.Results;
using Soditech.IntelPrev.Prevensions.Shared;
using Soditech.IntelPrev.Prevensions.Shared.PreventionSetting;

namespace Soditech.IntelPrev.Web.Pages.Prev.Preventions;

public partial class PreventionsIndex
{
    [Inject]
    private ILogger<PreventionsIndex> Logger { get; set; } = default!;

    public PreventionContentResult NewPreventionContentResult = new();
    public string? ErrorMessage { get; set; }
    public string? SuccessMessage { get; set; }

    private string _value = string.Empty;
    private bool IsSaving => AddBtnLabel == "En cours ...";
    public string AddBtnLabel = "Ajouter";
    private bool _isLoading = false;
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
        ErrorMessage = null;
        SuccessMessage = null;

        try
        {
            NewPreventionContentResult.Content = _value;

            if (AddBtnLabel == "Ajouter")
            {
                var result = await ProxyService.PostAsync<PreventionContentResult>(PreventionRoutes.PreventionSettings.UpdateDefinitionContent, NewPreventionContentResult);

                HandleResult(result, "La prévention a été ajouté avec succès !");
            }
            else
            {
                var result = await ProxyService.PostAsync<PreventionContentResult>(PreventionRoutes.PreventionSettings.UpdateDefinitionContent, NewPreventionContentResult);
                HandleResult(result, "La prévention a été modifié avec succès !");
            }

            CacheService.Set(ContentCacheKey, _value);
            await LoadContentAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = "Une erreur interne est survenue lors de l'ajout ou la modification.";
            Logger.LogError(ex, ErrorMessage);
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
            ErrorMessage = "Le résultat du serveur est nul.";
            Logger.LogError("Service result is null.");
            return;
        }

        if (result.IsSuccess)
        {
            SuccessMessage = successMessageText;
            _value = string.Empty;
            AddBtnLabel = "Ajouter";
        }
        else
        {
            ErrorMessage = result.Error?.Message ?? "Une erreur est survenue.";
            Logger.LogError("{code} : {message}", result.Error?.Code, result.Error?.Message);
        }
    }

    private async Task LoadContentFromApiAsync()
    {
        _isLoading = true;
        var result = await ProxyService.GetAsync<PreventionContentResult>(PreventionRoutes.PreventionSettings.GetDefinitionContent);
        if (result.IsSuccess)
        {
            _value = result.Value.Content;
            AddBtnLabel = string.IsNullOrEmpty(_value) ? "Ajouter" : "Modifier";
        }

        _isLoading = false;
    }

    private async Task LoadContentAsync()
    {
        _isLoading = true;
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
        _isLoading = false;
    }
}
