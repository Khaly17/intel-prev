using Soditech.IntelPrev.Reports.Shared.RegisterTypes;
using Soditech.IntelPrev.Reports.Shared;

namespace Soditech.IntelPrev.Web.Pages.Administration.RegisterTypes;

public partial class RegisterPrevious
{
    private bool _isLoading;
    private bool _isLoadingRegisterTypes;
    private RegisterTypeResult _registerTypeTest = default!; 
    private const string registerTypesCacheKey = "RegisterTypes";

    private IEnumerable<RegisterTypeResult> _registerTypes = [];

    protected override async Task OnInitializedAsync()
    {
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
    
}
