using System.Threading.Tasks;
using Soditech.IntelPrev.Mobile.Core.Dependency;
using Soditech.IntelPrev.Mobile.Services.Account;
using Soditech.IntelPrev.Mobile.ViewModels.Base;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;

namespace Soditech.IntelPrev.Mobile;

public class AppShellViewModel : MauiViewModel
{
    private string _userName = "User";
    public string UserName
    {
        get => _userName;
        set => SetProperty(ref _userName, value);
    }

    private string _appVersion = AppInfo.VersionString;
    public string AppVersion
    {
        get => _appVersion;
        set => SetProperty(ref _appVersion, value);
    }


    public ICommand LogoutCommand { get; }
    public ICommand GoToUserProfileCommand { get; }


    public AppShellViewModel()
    {
        LogoutCommand = new AsyncRelayCommand(LogoutAsync);
        GoToUserProfileCommand = new AsyncRelayCommand(GoTOUserProfileAsync);
    }


    private async Task LogoutAsync()
    {
        var accountService = DependencyResolver.GetRequiredService<IAccountService>();

        await accountService.LogoutAsync();

        if (Application.Current.MainPage is Shell { FlyoutIsPresented: true } shell)
        {
            shell.FlyoutIsPresented = false;
        }


        await Shell.Current.GoToAsync(new ShellNavigationState(AppRoutes.LoginPage));
    }

    private async Task GoTOUserProfileAsync()
    {
        await Shell.Current.GoToAsync(new ShellNavigationState(AppRoutes.ProfilePage));

        // if the flyout is open, then close it
        if (Application.Current.MainPage is Shell { FlyoutIsPresented: true } shell)
        {
            shell.FlyoutIsPresented = false;
        }
    }

}