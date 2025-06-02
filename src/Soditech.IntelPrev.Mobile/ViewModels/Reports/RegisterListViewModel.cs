using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Soditech.IntelPrev.Mobile.Core.Dependency;
using Soditech.IntelPrev.Mobile.ViewModels.Base;
using Soditech.IntelPrev.Proxy;
using Soditech.IntelPrev.Reports.Shared;
using Soditech.IntelPrev.Reports.Shared.RegisterTypes;

namespace Soditech.IntelPrev.Mobile.ViewModels.Reports;

public class RegisterListViewModel : MauiViewModel
{
    private RegisterTypeResult _selectedRegisterType = default!;
    public RegisterTypeResult SelectedRegisterType
    {
        get => _selectedRegisterType;
        set
        {
            SetProperty(ref _selectedRegisterType, value);
            _ = GoToCreateReportPageAsync();
        }
    }

    private List<RegisterTypeResult> _registerTypes = default!;
    public List<RegisterTypeResult> RegisterTypes
    {
        get => _registerTypes;
        set => SetProperty(ref _registerTypes, value);
    }

    private readonly IProxyService _proxyClientService = DependencyResolver.GetRequiredService<IProxyService>();

    public ICommand PageAppearingCommand => new AsyncRelayCommand(async () => await InitializeAsync());
    public ICommand GoToCreateReportCommand => new AsyncRelayCommand(GoToCreateReportPageAsync);
    
    // Command to handle tap gestures on register type items
    public ICommand SelectRegisterTypeCommand => new AsyncRelayCommand<RegisterTypeResult>(async (registerType) =>
    {
        if (registerType != null)
        {
            _selectedRegisterType = registerType;
            await GoToCreateReportPageAsync();
        }
    });

    /// <inheritdoc />
    public override async Task InitializeAsync()
    {
        IsBusy = true;
        var registerTypeResult = await _proxyClientService.GetAsync<List<RegisterTypeResult>>(ReportRoutes.RegisterTypes.GetAll);

        if (registerTypeResult.IsSuccess)
        {
            RegisterTypes = registerTypeResult.Value;
        }

        IsBusy = false;
    }

    private async Task GoToCreateReportPageAsync()
    {
        await Shell.Current.GoToAsync(new ShellNavigationState(AppRoutes.CreateReportPage),
            new ShellNavigationQueryParameters
            {
                { "Register", _selectedRegisterType }
            });
    }
}
