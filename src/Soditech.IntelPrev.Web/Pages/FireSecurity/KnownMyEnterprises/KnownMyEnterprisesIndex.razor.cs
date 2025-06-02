using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Sensor6ty.Results;
using Soditech.IntelPrev.Preventions.Shared;
using Soditech.IntelPrev.Preventions.Shared.FireSecuritySetting;
using Soditech.IntelPrev.Preventions.Shared.ProPrevSetting;

namespace Soditech.IntelPrev.Web.Pages.FireSecurity.KnownMyEnterprises;

public partial class KnownMyEnterprisesIndex
{
    [Inject]
    private ILogger<KnownMyEnterprisesIndex> Logger { get; set; } = default!;

    public FireSecuritySettingContentResult newFireSecuritySettingContentResult = new();
    public string? errorMessage { get; set; }
    public string? successMessage { get; set; }

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
        errorMessage = null;
        successMessage = null;

        try
        {
            newFireSecuritySettingContentResult.Content = _value;

            if (AddBtnLabel == "Ajouter")
            {
                var result = await ProxyService.PostAsync<FireSecuritySettingContentResult>(PreventionRoutes.FireSecuritySettings.UpdateKnownMyEnterpriseContent, newFireSecuritySettingContentResult);

                HandleResult(result, "Ajouté avec succès !");
            }
            else
            {
                var result = await ProxyService.PostAsync<FireSecuritySettingContentResult>(PreventionRoutes.FireSecuritySettings.UpdateKnownMyEnterpriseContent, newFireSecuritySettingContentResult);
                HandleResult(result, "Modifié avec succès !");
            }

            await GetContent();
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

    private void HandleResult(TResult<FireSecuritySettingContentResult>? result, string successMessageText)
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
