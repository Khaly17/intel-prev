using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Sensor6ty.Results;
using Soditech.IntelPrev.Prevensions.Shared;
using Soditech.IntelPrev.Prevensions.Shared.PreventionSetting;

namespace Soditech.IntelPrev.Web.Pages.Prev.Sensibilisations;

public partial class SensibilisationsIndex
{
    [Inject]
    private ILogger<SensibilisationsIndex> Logger { get; set; } = default!;

    public PreventionContentResult NewPreventionContentResult = new();
    public string? ErrorMessage { get; set; }
    public string? SuccessMessage { get; set; }

    private string _value = string.Empty;
    private bool IsSaving => AddBtnLabel == "En cours ...";
    public string AddBtnLabel = "Ajouter";

    protected override async Task OnInitializedAsync()
    {
        await GetContentAsync();
    }

    private void OnValueChanged(string newValue)
    {
        _value = newValue;
    }

    private async Task SaveContentAsync()
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
                var result = await ProxyService.PostAsync<PreventionContentResult>(PreventionRoutes.PreventionSettings.UpdateSensibilisationContent, NewPreventionContentResult);

                HandleResult(result, "La sensibilisation a été ajouté avec succès !");
            }
            else
            {
                var result = await ProxyService.PostAsync<PreventionContentResult>(PreventionRoutes.PreventionSettings.UpdateSensibilisationContent, NewPreventionContentResult);
                HandleResult(result, "La sensibilisation a été modifié avec succès !");
            }

            await GetContentAsync();
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

    private async Task GetContentAsync()
    {
        var result = await ProxyService.GetAsync<PreventionContentResult>(PreventionRoutes.PreventionSettings.GetSensibilisationContent);
        if (result.IsSuccess)
        {
            _value = result.Value.Content;
            AddBtnLabel = string.IsNullOrEmpty(_value) ? "Ajouter" : "Modifier";
        }
    }

}
