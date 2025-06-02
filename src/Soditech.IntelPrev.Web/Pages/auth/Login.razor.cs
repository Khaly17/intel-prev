using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components;
using Soditech.IntelPrev.Users.Shared.Account;
using Soditech.IntelPrev.Web.Services.Authentications;

namespace Soditech.IntelPrev.Web.Pages.auth
{
    public partial class Login
    {
        [Inject] public IAuthenticationServices AuthenticationServices { get; set; } = default!;
        private string Tenant { get; set; } = default!;
        private string Username { get; set; } = default!;
        private string Password { get; set; } = default!;
        private string ErrorMessage { get; set; } = string.Empty;
        private bool IsLoading { get; set; } = true;
        private bool IsConnecting { get; set; } = false;

        [Parameter]
        [SupplyParameterFromQuery(Name = "returnUrl")]
        public string ReturnUrl { get; set; } = "/";


        /// <inheritdoc />
        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;
            await AuthenticationServices.Logout();
            IsLoading = false;
        }

        private async Task HandleLogin()
        {
            IsConnecting = true;
            StateHasChanged();
            var  authenticationRequest = new WebLoginCommand
            {
                Email = Username,
                Password = Password
            };
            var result = await AuthenticationServices.Login(authenticationRequest);

            if (result.IsSuccess)
            {
                if (string.IsNullOrWhiteSpace(ReturnUrl)) ReturnUrl = "/";

                Navigation.NavigateTo(ReturnUrl);
            }
            else
            {
                ErrorMessage = MapErrorMessage(result.Error.Code, result.Error.Message);
            }

            IsConnecting = false;

            StateHasChanged();
        }
        private string MapErrorMessage(string errorCode, string errorMessage)
        {
            return errorCode switch
            {
                "Unauthorized" or "401" => "Nom d'utilisateur ou mot de passe incorrect",
                "500" => "Une erreur interne s'est produite. Veuillez réessayer plus tard.",
                "404" => "Le service demandé est introuvable.",
                _ => errorMessage.Contains("Cannot post data to server")
                    ? "Impossible d'envoyer les données au serveur. Veuillez vérifier votre connexion."
                    : "Une erreur inconnue s'est produite. Veuillez contacter le support."
            };
        }

        private void NavigateToForgotPassword()
        {
            Navigation.NavigateTo("/auth/forgot-password");
        }
        
    }

}
