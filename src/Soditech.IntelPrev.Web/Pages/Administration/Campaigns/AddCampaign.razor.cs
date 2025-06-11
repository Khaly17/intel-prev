using Microsoft.AspNetCore.Components;
using Soditech.IntelPrev.Preventions.Shared.Campaigns;
using Soditech.IntelPrev.Users.Shared;
using Soditech.IntelPrev.Preventions.Shared;

namespace Soditech.IntelPrev.Web.Pages.Administration.Campaigns;

public partial class AddCampaign
{
    public CampaignResult NewCampaign { get; set; } = new();

    public string? errorMessage { get; set; }

    public string? successMessage { get; set; }
    [Inject] private ILogger<AddCampaign> Logger { get; set; } = default!;
    private async Task CreateCampaign()
    {
        errorMessage = null;
        successMessage = null;
        try
        {
            var result = await ProxyService.PostAsync<CampaignResult>(PreventionRoutes.Campaigns.Create, NewCampaign);

            if (result.IsSuccess)
            {
                successMessage = "La campaigne a été ajouté avec succès !";
                var campaignId = result.Value.Id;
                Navigation.NavigateTo("/campaigns");
            }
            else
            {
                errorMessage = result.Error?.Message ?? "Une erreur est survenue lors de la création de la campagne.";
                Logger.LogError("{code} : {message}", result.Error?.Code, result.Error?.Message);
            }
        }
        catch (Exception ex)
        {
            errorMessage = "Error: Cannot create campaign";
            Logger.LogError(ex, errorMessage);
        }
    }


}
