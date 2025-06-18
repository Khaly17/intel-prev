using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Soditech.IntelPrev.Mobile.Services.Account;
using Soditech.IntelPrev.Mobile.Services.Account.Models;
using Soditech.IntelPrev.Mobile.Services.ApiClient;
using Soditech.IntelPrev.Mobile.Services.Storage;
using Soditech.IntelPrev.Mobile.ViewModels.Base;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;

namespace Soditech.IntelPrev.Mobile.ViewModels.Account
{
	public partial class LoginViewModel : MauiViewModel
	{
		private readonly IAccountService _accountService;
		private readonly IAccessTokenManager _accessTokenManager;
		private readonly IDataStorageService _dataStorageService;
		private readonly ILogger<LoginViewModel> _logger;
		private string _errorMessage;
		public string ErrorMessage
		{
			get => _errorMessage;
			set => SetProperty(ref _errorMessage, value);
		}

		public LoginViewModel()
		{
			_accountService = Core.Dependency.DependencyResolver.GetRequiredService<IAccountService>();
			_accessTokenManager = Core.Dependency.DependencyResolver.GetRequiredService<IAccessTokenManager>();
			_dataStorageService = Core.Dependency.DependencyResolver.GetRequiredService<IDataStorageService>();
			_logger = Core.Dependency.DependencyResolver.GetRequiredService<ILogger<LoginViewModel>>();
			_errorMessage = string.Empty;

			LoginCommand = new AsyncRelayCommand(LoginAsync);
			PageAppearingCommand = new AsyncRelayCommand(() => InitializeAsync());
			NavigateToForgotPasswordCommand = new AsyncRelayCommand(NavigateToForgotPasswordAsync);
		}

		public IAsyncRelayCommand LoginCommand { get; }
		public IAsyncRelayCommand PageAppearingCommand { get; }
		public IAsyncRelayCommand NavigateToForgotPasswordCommand { get; }

		public string Username
		{
			get => _accountService.AuthenticateModel.Email;
			set
			{
				if (_accountService.AuthenticateModel.Email != value)
				{
					_accountService.AuthenticateModel.Email = value;
					OnPropertyChanged();
				}
			}
		}

		public string Password
		{
			get => _accountService.AuthenticateModel.Password;
			set
			{
				if (_accountService.AuthenticateModel.Password != value)
				{
					_accountService.AuthenticateModel.Password = value;
					OnPropertyChanged();
				}
			}
		}

		private string _fullName;

        public override async Task InitializeAsync()
		{
			try
			{
				IsBusy = true;

				var authenticateResult = _dataStorageService.RetrieveAuthenticateResult();

				// Only proceed if we have valid authentication
				if (authenticateResult != null)
				{
					_accountService.AuthenticateResult = authenticateResult;
					_accessTokenManager.AuthenticateResult = authenticateResult;

					// Check if user is logged in
					if (_accountService.IsUserLoggedIn)
					{
						FlurlClientConfig.ConfigureFlurlHttp();

						UpdateUserInfo();

						await Shell.Current.GoToAsync(new ShellNavigationState(AppRoutes.MainViewPage));
					}
				}
			}
			catch (Exception ex)
			{
				ErrorMessage = "";
				_logger.LogError(ex, "Error during initialization in LoginViewModel");
				// Handle specific exceptions if needed
				// Don't rethrow - let the app continue with login screen
			}
			finally
			{
				IsBusy = false;
			}
		}
        private async Task LoginAsync()
        {
            try
            {
                if (IsBusy) return;
                IsBusy = true;
                ErrorMessage = string.Empty;

                if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
                {
                    ErrorMessage = "Veuillez entrer votre nom d�utilisateur et votre mot de passe.";
                    return;
                }

                await _accountService.LoginUserAsync();

                if (_accountService is { IsUserLoggedIn: true, AuthenticateResult: not null })
                {
                    UpdateUserInfo();
                    await Shell.Current.GoToAsync(new ShellNavigationState(AppRoutes.MainViewPage));
                }
                else
				{
					ErrorMessage = "Nom d'utilisateur ou mot de passe incorrect.";

				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Login failed in LoginViewModel");
				ErrorMessage = "La connexion a échoué. Veuillez réessayer.";
			}
			finally
			{
				IsBusy = false;
			}
		}

        public async Task NavigateToForgotPasswordAsync()
		{
			await Shell.Current.GoToAsync(new ShellNavigationState(AppRoutes.ForgotPasswordPage));
		}
        private void UpdateUserInfo()
        {
            // Check if user is logged in and has a valid token
            if (_accountService is not { IsUserLoggedIn: true, AuthenticateResult: not null }) return;

            try
            {
                var appShellViewModel = (AppShellViewModel)Application.Current.MainPage.BindingContext;

                var token = _accountService.AuthenticateResult?.AccessToken;

                if (string.IsNullOrEmpty(token)) return;

                // Parse the JWT token to get user information
                var jwtHandler = new JwtSecurityTokenHandler();
                var jwtToken = jwtHandler.ReadJwtToken(token);

                // Extract user information from claims
                var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
                var fullName = jwtToken.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
                var username = jwtToken.Claims.FirstOrDefault(c => c.Type == "username")?.Value;
                var email = jwtToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value;

				var userProfile = new UserInfoModel
				{
					UserId = userId ?? string.Empty,
					FirstName = fullName ?? string.Empty,
					LastName = username ?? string.Empty,
					Email = email ?? string.Empty
				};		
				// Store user profile in the local storage
				_dataStorageService.SetValue<UserInfoModel>(AppConsts.UserProfileKey, userProfile);

                // Set user name with priority: fullName > username > email > default
                if (!string.IsNullOrEmpty(fullName))
                {
                    _fullName = fullName;
                }
                else if (!string.IsNullOrEmpty(username))
                {
                    _fullName = username;
                }
                else if (!string.IsNullOrEmpty(email))
                {
                    _fullName = email;
                }
                else
                {
                    _fullName = "Logged In User";
                }

                appShellViewModel.UserName = _fullName;
            }
            catch (Exception ex)
            {
                // Log any errors but don't crash
				_logger.LogError(ex, "Error parsing JWT token in LoginViewModel");
                _fullName = "Logged In User";
            }
        }

    }
}