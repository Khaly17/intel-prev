using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Soditech.IntelPrev.Mobile.Services.Account;
using Soditech.IntelPrev.Mobile.Services.Account.Models;
using Soditech.IntelPrev.Mobile.Services.Storage;
using Soditech.IntelPrev.Mobile.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soditech.IntelPrev.Mobile.ViewModels.Account;

public partial class ChangePasswordViewModel : MauiViewModel
{
    private readonly IAccountService _accountService;
    private readonly IDataStorageService _dataStorageService;


    public ChangePasswordViewModel()
    {
        _accountService = Core.Dependency.DependencyResolver.GetRequiredService<IAccountService>();
        _dataStorageService = Core.Dependency.DependencyResolver.GetRequiredService<IDataStorageService>();
        ChangePasswordCommand = new AsyncRelayCommand(ChangePasswordAsync);
    }

    [ObservableProperty] private string currentPassword;
    [ObservableProperty] private string newPassword;
    [ObservableProperty] private string confirmPassword;
    [ObservableProperty] private string errorMessage;

    public IAsyncRelayCommand ChangePasswordCommand { get; }

    private async Task ChangePasswordAsync()
    {
        try
        {
            if (IsBusy) return;
            IsBusy = true;

            ErrorMessage = string.Empty;
            CurrentPassword =  _dataStorageService.GetValueOrDefault<string>(AppConsts.CurrentPinKey);

            if (string.IsNullOrWhiteSpace(CurrentPassword) ||
                string.IsNullOrWhiteSpace(NewPassword) ||
                string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                ErrorMessage = "Tous les champs sont obligatoires.";
                return;
            }

            if (NewPassword != ConfirmPassword)
            {
                ErrorMessage = "Les mots de passe ne correspondent pas.";
                return;
            }

            var userId = Guid.NewGuid();
            var userInfos = _dataStorageService.GetValueOrDefault<UserInfoModel>(AppConsts.UserProfileKey);
            if (_accountService.AuthenticateResult!= null)
            {
                userId = Guid.Parse(userInfos.userId);
            }

            var result = await _accountService.ChangePasswordAsync(userId.ToString(), CurrentPassword, NewPassword);
            if (result.IsSuccess)
            {
                await Shell.Current.GoToAsync(AppRoutes.MainViewPage);
            }
            else
            {
                ErrorMessage = "Échec du changement de mot de passe.";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = "Une erreur est survenue.";
        }
        finally
        {
            IsBusy = false;
        }
    }
}
