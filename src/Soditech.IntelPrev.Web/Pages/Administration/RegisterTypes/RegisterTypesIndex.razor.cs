using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Soditech.IntelPrev.Reports.Shared;
using Soditech.IntelPrev.Reports.Shared.RegisterTypes;
using Soditech.IntelPrev.Web.Components.Administrations.Registers;
using Soditech.IntelPrev.Web.Models.Utils;

namespace Soditech.IntelPrev.Web.Pages.Administration.RegisterTypes;

public partial class RegisterTypesIndex : ComponentBase
{
    private bool _isLoading;
    private bool _isLoadingRegisterTypes;
    private const string registerTypesCacheKey = "RegisterTypes";
    private IEnumerable<RegisterTypeResult> _registerTypes = [];
    private RegisterTypeResult _registerTypeTest = default!;
    [Inject] 
    private ILogger<RegisterTypesIndex> Logger { get; set; } = default!;
    private string saveBtnLabel { get; set; } = "Enregistrer";
    public RegisterTypeResult RegisterTypeResult = new()
    {
        RegisterFieldGroups = [],
        RegisterFields = []
    };

    private bool _isCreating;

    public string? errorMessage { get; set; }
    public string? successMessage { get; set; }

    private void HandleIsCreatingChanged(bool newValue)
    {
        _isCreating = newValue;
    }

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        // get all register types
        await LoadRegisterTypesAsync();
    }
    
    private async Task LoadRegisterTypesAsync()
    {
        _isLoading = true;
        var (exists, cachedValue) = CacheService.Get(registerTypesCacheKey);

        if (exists)
        {
            _registerTypes = (IList<RegisterTypeResult>)cachedValue;
        }
        else
        {
            await LoadRegisterTypesFromApiAsync();
            CacheService.Set(registerTypesCacheKey, _registerTypes);
        }
        _registerTypeTest = _registerTypes.FirstOrDefault() ?? new RegisterTypeResult();
        _isLoading = false;
    }

    private async Task LoadRegisterTypesFromApiAsync()
    {
        _isLoadingRegisterTypes = true;
        var result = await ProxyService.GetAsync<IEnumerable<RegisterTypeResult>>(ReportRoutes.RegisterTypes.GetAll);
        if (result.IsSuccess)
        {
            _registerTypes = result.Value ?? [];
        }
        _isLoadingRegisterTypes = false;
    }

    private async Task CreateRegister()
    {
        saveBtnLabel = "Enregistrement ...";
        errorMessage = null;
        successMessage = null;
        try
        {
            var path = ReportRoutes.RegisterTypes.Create;
            var result = await ProxyService.PostAsync<RegisterTypeResult>(path, RegisterTypeResult);

            if (result.IsSuccess)
            {
                successMessage = "Le registre a �t� ajout� avec succ�s !";
                Navigation.NavigateTo("/registers");
            }
            else
            {
                errorMessage = result.Error?.Message ?? "Une erreur est survenue lors de la cr�ation du registre.";
                Logger.LogError("{code} : {message}", result.Error?.Code, result.Error?.Message);
            }
        }
        catch (Exception ex)
        {
            errorMessage = "Une erreur interne est survenue lors de la cr�ation du registre.";
            Logger.LogError(ex, errorMessage);
        }
        saveBtnLabel = "Enregistrer";

    }

}