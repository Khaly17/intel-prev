using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Sensor6ty.Results;
using Soditech.IntelPrev.Prevensions.Shared;
using Soditech.IntelPrev.Prevensions.Shared.ProPrevSetting;

namespace Soditech.IntelPrev.Web.Pages.ProPrev.HealthFormationContents;


public partial class HealthFormationIndex
{
    [Inject]
    private ILogger<HealthFormationIndex> Logger { get; set; } = default!;

    public ProPrevContentResult NewProPrevContentResult = new();
    public string? ErrorMessage { get; set; }
    public string? SuccessMessage { get; set; }

    private string _value = string.Empty;
    private bool IsSaving => AddBtnLabel == "En cours ...";
    public string AddBtnLabel = "Ajouter";

    protected override async Task OnInitializedAsync()
    {
        await GetContent();
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
            NewProPrevContentResult.Content = _value;

            if (AddBtnLabel == "Ajouter")
            {
                var result = await ProxyService.PostAsync<ProPrevContentResult>(PreventionRoutes.ProPrevSettings.UpdateHealthFormationContent, NewProPrevContentResult);

                HandleResult(result, "Le protocol d'analyse des risques a été ajouté avec succès !");
            }
            else
            {
                var result = await ProxyService.PostAsync<ProPrevContentResult>(PreventionRoutes.ProPrevSettings.UpdateHealthFormationContent, NewProPrevContentResult);
                HandleResult(result, "Le protocol d'analyse des risques a été modifié avec succès !");
            }

            await GetContent();
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

    private void HandleResult(TResult<ProPrevContentResult>? result, string successMessageText)
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

    private async Task GetContent()
    {
        var result = await ProxyService.GetAsync<ProPrevContentResult>(PreventionRoutes.ProPrevSettings.GetHealthFormationContent);
        if (result.IsSuccess)
        {
            _value = result.Value.Content;
            AddBtnLabel = string.IsNullOrEmpty(_value) ? "Ajouter" : "Modifier";
        }
    }

}
