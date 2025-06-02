using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Soditech.IntelPrev.Mobile.Services.Account;
using Soditech.IntelPrev.Mobile.Services.Storage;
using Soditech.IntelPrev.Mobile.ViewModels.Base;
using System.Diagnostics;

namespace Soditech.IntelPrev.Mobile.ViewModels.Account;

public partial class PinVerificationViewModel : MauiViewModel
{
    private readonly IAccountService _accountService;
    private readonly IDataStorageService _dataStorageService;

    public PinVerificationViewModel()
    {
        _accountService = Core.Dependency.DependencyResolver.GetRequiredService<IAccountService>();
        _dataStorageService = Core.Dependency.DependencyResolver.GetRequiredService<IDataStorageService>();
        ValidatePinCommand = new AsyncRelayCommand(ValidatePinAsync);
    }

    [ObservableProperty] private string currentPin = string.Empty;
    [ObservableProperty] private string errorMessage = string.Empty;

    public IAsyncRelayCommand ValidatePinCommand { get; }

    private async Task ValidatePinAsync()
    {
        ErrorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(CurrentPin) || CurrentPin.Length != 4)
        {
            ErrorMessage = "Le code PIN doit contenir 4 chiffres.";
            return;
        }

        try
        {
            var isValid = await _accountService.VerifyCurrentPinAsync(CurrentPin);
            if (isValid)
            {
                await _dataStorageService.SetValue<string>(AppConsts.CurrentPinKey, CurrentPin);
                await Shell.Current.GoToAsync(AppRoutes.ChangePasswordPage);
            }
            else
            {
                ErrorMessage = "Code PIN incorrect.";
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Erreur PIN : {ex.Message}");
            ErrorMessage = "Erreur de validation.";
        }
    }
}
