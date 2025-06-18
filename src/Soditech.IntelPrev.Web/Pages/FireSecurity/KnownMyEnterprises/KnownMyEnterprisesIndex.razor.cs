using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Sensor6ty.Results;
using Soditech.IntelPrev.Prevensions.Shared;
using Soditech.IntelPrev.Prevensions.Shared.FireSecuritySetting;

namespace Soditech.IntelPrev.Web.Pages.FireSecurity.KnownMyEnterprises;

public partial class KnownMyEnterprisesIndex
{
    [Inject]
    private ILogger<KnownMyEnterprisesIndex> Logger { get; set; } = default!;

    public FireSecuritySettingContentResult NewFireSecuritySettingContentResult = new();
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
            NewFireSecuritySettingContentResult.Content = _value;

            if (AddBtnLabel == "Ajouter")
            {
                var result = await ProxyService.PostAsync<FireSecuritySettingContentResult>(PreventionRoutes.FireSecuritySettings.UpdateKnownMyEnterpriseContent, NewFireSecuritySettingContentResult);

                HandleResult(result, "Ajouté avec succès !");
            }
            else
            {
                var result = await ProxyService.PostAsync<FireSecuritySettingContentResult>(PreventionRoutes.FireSecuritySettings.UpdateKnownMyEnterpriseContent, NewFireSecuritySettingContentResult);
                HandleResult(result, "Modifié avec succès !");
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

    private void HandleResult(TResult<FireSecuritySettingContentResult>? result, string successMessageText)
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
        var result = await ProxyService.GetAsync<FireSecuritySettingContentResult>(PreventionRoutes.FireSecuritySettings.GetKnownMyEnterpriseContent);
        if (result.IsSuccess)
        {
            _value = result.Value.Content;
            AddBtnLabel = string.IsNullOrEmpty(_value) ? "Ajouter" : "Modifier";
        }
    }

}
