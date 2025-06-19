using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Soditech.IntelPrev.Prevensions.Shared;
using Soditech.IntelPrev.Prevensions.Shared.Campaigns;

namespace Soditech.IntelPrev.Web.Pages.Administration.Campaigns;

public partial class AddCampaign
{
    public CampaignResult NewCampaign { get; set; } = new();

    public string? ErrorMessage { get; set; }

    public string? SuccessMessage { get; set; }
    [Inject] private ILogger<AddCampaign> Logger { get; set; } = default!;
    private async Task CreateCampaignAsync()
    {
        ErrorMessage = null;
        SuccessMessage = null;
        try
        {
            var result = await ProxyService.PostAsync<CampaignResult>(PreventionRoutes.Campaigns.Create, NewCampaign);

            if (result.IsSuccess)
            {
                SuccessMessage = "La campaigne a été ajouté avec succès !";
                Navigation.NavigateTo("/campaigns");
            }
            else
            {
                ErrorMessage = result.Error?.Message ?? "Une erreur est survenue lors de la création de la campagne.";
                Logger.LogError("{code} : {message}", result.Error?.Code, result.Error?.Message);
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = "Error: Cannot create campaign";
            Logger.LogError(ex, ErrorMessage);
        }
    }


}
