using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Sensor6ty.Results;
using Soditech.IntelPrev.Prevensions.Shared;
using Soditech.IntelPrev.Prevensions.Shared.FireSecuritySetting;

namespace Soditech.IntelPrev.Web.Pages.FireSecurity.FireMaterials;

public partial class FireMaterialsIndex
{
    [Inject]
    private ILogger<FireMaterialsIndex> Logger { get; set; } = default!;

    public FireSecuritySettingContentResult NewFireSecuritySettingContentResult = new();
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
            NewFireSecuritySettingContentResult.Content = _value;

            if (AddBtnLabel == "Ajouter")
            {
                var result = await ProxyService.PostAsync<FireSecuritySettingContentResult>(PreventionRoutes.FireSecuritySettings.UpdateFireMaterialsContent, NewFireSecuritySettingContentResult);

                HandleResult(result, "La consigne a été ajouté avec succès !");
            }
            else
            {
                var result = await ProxyService.PostAsync<FireSecuritySettingContentResult>(PreventionRoutes.FireSecuritySettings.UpdateFireMaterialsContent, NewFireSecuritySettingContentResult);
                HandleResult(result, "La consigne a été modifié avec succès !");
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

    private async Task GetContentAsync()
    {
        var result = await ProxyService.GetAsync<FireSecuritySettingContentResult>(PreventionRoutes.FireSecuritySettings.GetFireMaterialsContent);
        if (result.IsSuccess)
        {
            _value = result.Value.Content;
            AddBtnLabel = string.IsNullOrEmpty(_value) ? "Ajouter" : "Modifier";
        }
    }

}
