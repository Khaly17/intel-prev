using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Soditech.IntelPrev.Prevensions.Shared.Campaigns;

namespace Soditech.IntelPrev.Web.Components.Administrations;

public partial class AddOrEditCampaign
{
    [Parameter]
    public CampaignResult NewCampaign { get; set; } = default!;

    [Parameter]
    public string Title { get; set; } = "Ajouter un nouveau";

    [Parameter]
    public string? ErrorMessage { get; set; }

    [Parameter]
    public string? SuccessMessage { get; set; }

    [Parameter]
    public EventCallback OnCampaignCreated { get; set; }

    public async Task CreateCampaign()
    {
        await OnCampaignCreated.InvokeAsync(null);
    }
}
