using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Soditech.IntelPrev.Reports.Shared;
using Soditech.IntelPrev.Reports.Shared.RegisterTypes;

namespace Soditech.IntelPrev.Web.Pages.Administration.RegisterTypes;

public partial class EditRegisterType
{
    [Parameter]
    public string RegisterId { get; set; } = string.Empty;
    private bool _isCreating;

    private const string registerTypesCacheKey = "RegisterTypes";
    [Inject]
    private ILogger<EditRegisterType> Logger { get; set; } = default!;

    private string SaveBtnLabel { get; set; } = "Enregistrer";
    public string? ErrorMessage { get; set; }
    public string? SuccessMessage { get; set; }
    private bool _isLoading;

    private string GetRegisterCacheKey() => $"Register_{RegisterId}";

    public RegisterTypeResult RegisterTypeResult = new()
    {
        RegisterFieldGroups = [],
        RegisterFields = []
    };

    private void HandleIsCreatingChanged(bool newValue)
    {
        _isCreating = newValue;
    }

    protected override async Task OnInitializedAsync()
    {
        await LoadRegisterAsync();
    }

    private async Task LoadRegisterAsync()
    {
        _isLoading = true;
        var cacheKey = GetRegisterCacheKey();
        var (exists, cachedValue) = CacheService.Get(cacheKey);

        if (exists)
        {
            RegisterTypeResult = (RegisterTypeResult)cachedValue;
        }
        else
        {
            await GetRegisterFromApiAsync();
            CacheService.Set(cacheKey, RegisterTypeResult);
        }
        _isLoading = false;
    }

    private async Task GetRegisterFromApiAsync()
    {
        try
        {
            var path = ReportRoutes.RegisterTypes.GetById.Replace("{id:guid}", RegisterId);
            var result = await ProxyService.GetAsync<RegisterTypeResult>(path);

            if (result.IsSuccess)
            {
                RegisterTypeResult = result.Value;
            }
            else
            {
                ErrorMessage = "Erreur de récupération des informations du registre.";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Erreur: {ex.Message}";
        }
    }

    private async Task UpdateRegisterAsync()
    {
        SaveBtnLabel = "Enregistrement ...";
        ErrorMessage = null;
        SuccessMessage = null;

        try
        {
            var path = ReportRoutes.RegisterTypes.Update.Replace("{id:guid}", RegisterTypeResult.Id.ToString());
            var result = await ProxyService.PostAsync<RegisterTypeResult>(path, RegisterTypeResult);

            if (result.IsSuccess)
            {
                SuccessMessage = "Le registre a été enregistré avec succès !";
                CacheService.Set(GetRegisterCacheKey(), RegisterTypeResult);
                CacheService.Set(registerTypesCacheKey, null);
                Navigation.NavigateTo("/registers");
            }
            else
            {
                ErrorMessage = result.Error?.Message ?? "Une erreur est survenue lors de la création du registre.";
                Logger.LogError("{code} : {message}", result.Error?.Code, result.Error?.Message);
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = "Une erreur interne est survenue lors de la création du registre.";
            Logger.LogError(ex, ErrorMessage);
        }

        SaveBtnLabel = "Enregistrer";
    }
}
