using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Soditech.IntelPrev.Mobile.Services.Account;
using Soditech.IntelPrev.Mobile.Services.Account.Models;
using Soditech.IntelPrev.Mobile.Services.Storage;
using Soditech.IntelPrev.Mobile.ViewModels.Base;

namespace Soditech.IntelPrev.Mobile.ViewModels.Account;

public partial class ProfileViewModel : MauiViewModel
{
	private readonly IAccountService _accountService;
	private readonly IDataStorageService _dataStorageService;

	public ProfileViewModel()
	{
		_accountService = Core.Dependency.DependencyResolver.GetRequiredService<IAccountService>();
		_dataStorageService = Core.Dependency.DependencyResolver.GetRequiredService<IDataStorageService>();

		FirstName = _dataStorageService.GetValueOrDefault<UserInfoModel>(AppConsts.UserProfileKey).FirstName ?? "Prénom";
        LastName =_dataStorageService.GetValueOrDefault<UserInfoModel>(AppConsts.UserProfileKey).LastName ?? "Nom";
		Email = _dataStorageService.GetValueOrDefault<UserInfoModel>(AppConsts.UserProfileKey).Email ?? "email@example.com";

		PageAppearingCommand = new AsyncRelayCommand(async () => await InitializeAsync());
		GoToPinVerificationCommand = new AsyncRelayCommand(NavigateToPinVerificationAsync);
		LogoutCommand = new AsyncRelayCommand(LogoutAsync);
	}

	[ObservableProperty] private string firstName;
	[ObservableProperty] private string lastName;
	[ObservableProperty] private string email;

	public string ProfileTitle => $"{FirstName}";
	public string ProfileSubtitle => $"Adresse e-mail : {Email}";
	public static string ChangePinText => "Modifier code pin";
	public static string LogoutText => "Déconnexion";
	public IAsyncRelayCommand PageAppearingCommand { get; }
	public IAsyncRelayCommand GoToPinVerificationCommand { get; }
	public IAsyncRelayCommand LogoutCommand { get; }

 	// go home command
    public static IAsyncRelayCommand GoHomeCommand => new AsyncRelayCommand(async () =>
    {
        await Shell.Current.GoToAsync(new ShellNavigationState(AppRoutes.MainViewPage));
    });
	private async Task NavigateToPinVerificationAsync()
	{
		await Shell.Current.GoToAsync(AppRoutes.PinVerificationPage);
	}

	private async Task LogoutAsync()
	{
		await _accountService.LogoutAsync();
		await Shell.Current.GoToAsync(AppRoutes.LoginPage);
	}

	public override Task InitializeAsync()
	{
		return Task.CompletedTask;
	}
}
