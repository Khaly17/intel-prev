using System;
using Microsoft.AspNetCore.Components; 
using System.Threading.Tasks;
using Soditech.IntelPrev.Web.Services.Authentications;

namespace Soditech.IntelPrev.Web.Pages.auth;

public partial class ForgotPassword
{
    [Inject] public IAuthenticationServices AuthenticationServices { get; set; } = default!;
        
    private string Username { get; set; } = default!;
    private string ForgotPasswordMessage { get; set; } = string.Empty;
    private bool IsSendingReset { get; set; } = false;

    private async Task HandleForgotPasswordAsync()
    {
        ForgotPasswordMessage = string.Empty;
            
        if (string.IsNullOrWhiteSpace(Username))
        {
            ForgotPasswordMessage = "Veuillez d'abord saisir votre nom d'utilisateur.";
            return;
        }

        IsSendingReset = true;
        StateHasChanged();

        try
        {
            var result = await AuthenticationServices.ForgotPassword(Username);
                
            if (result.IsSuccess)
            {
                ForgotPasswordMessage = "Un email de réinitialisation a été envoyé à votre adresse.";
                await Task.Delay(3000); 
                Navigation.NavigateTo("/login");
            }
            else
            {
                ForgotPasswordMessage = MapErrorMessage(result.Error.Code, result.Error.Message);
            }
        }
        catch (Exception ex)
        {
            ForgotPasswordMessage = $"Une erreur s'est produite : {ex.Message}";
        }
        finally
        {
            IsSendingReset = false;
            StateHasChanged();
        }
    }

    private static string MapErrorMessage(string errorCode, string errorMessage)
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

    private void ValidateUsername(ChangeEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(e.Value?.ToString()))
        {
            ForgotPasswordMessage = "Le nom d'utilisateur est requis.";
        }
        else
        {
            ForgotPasswordMessage = string.Empty;
        }
        StateHasChanged();
    }
}