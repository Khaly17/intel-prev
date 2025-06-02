using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Soditech.IntelPrev.Mobile.Services.Account;
using Soditech.IntelPrev.Mobile.ViewModels.Base;
using System.Diagnostics;

namespace Soditech.IntelPrev.Mobile.ViewModels.Account;

public partial class ForgotPasswordViewModel : MauiViewModel
{
    private readonly IAccountService _accountService;
    private readonly ILogger<ForgotPasswordViewModel> _logger = Core.Dependency.DependencyResolver.GetRequiredService<ILogger<ForgotPasswordViewModel>>();
    public ForgotPasswordViewModel()
    {
        _accountService = Core.Dependency.DependencyResolver.GetRequiredService<IAccountService>();

        ForgotPasswordCommand = new AsyncRelayCommand(ForgotPasswordAsync);
        NavigateToLoginCommand = new AsyncRelayCommand(NavigateToLoginAsync);
    }

    [ObservableProperty]
    private string username = string.Empty;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    public IAsyncRelayCommand ForgotPasswordCommand { get; }
    public IAsyncRelayCommand NavigateToLoginCommand { get; }

    private async Task ForgotPasswordAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(Username))
            {
                ErrorMessage = "Veuillez entrer votre adresse email.";
                return;
            }

            var response = await _accountService.ForgotPasswordAsync(Username);
            if (response.IsSuccess)
            {
                await Shell.Current.DisplayAlert("Réinitialisation du mot de passe", "Un lien de réinitialisation a été envoyé à votre adresse email.", "OK");
                await NavigateToLoginAsync();
            }
            else
            {
                ErrorMessage = response.Error.Message ?? "Une erreur s'est produite lors de l'envoi du lien de réinitialisation.";
            }

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Forgot password failed");
            ErrorMessage = $"Impossible d’envoyer le lien. Vérifiez votre email. {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task NavigateToLoginAsync()
    {
        await Shell.Current.GoToAsync(AppRoutes.LoginPage);
    }
}


