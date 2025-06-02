//using Microsoft.AspNetCore.Components;
//using Soditech.IntelPrev.Preventions.Shared.Equipments;
//using Soditech.IntelPrev.Preventions.Shared;

//namespace Soditech.IntelPrev.Web.Pages.Administration.Equipements;

//public partial class EditEquipment: ComponentBase
//{
//    [Parameter]
//    public string equipmentId { get; set; } = string.Empty;
//    public string title { get; set; } = "Modification de l'équipement";
//    private EquipmentResult equipment { get; set; } = new EquipmentResult();

//    private string? successMessage;
//    private string? errorMessage;
//    [Inject] private ILogger<EditEquipment> Logger { get; set; } = default!;

//    protected override async Task OnInitializedAsync()
//    {
//        await GetEquipmentAsync();
//    }

//    private async Task GetEquipmentAsync()
//    {
//        try
//        {
//            var result = await ProxyService.GetAsync<EquipmentResult>(PreventionRoutes.Equipments.GetById.Replace("{id:guid}", equipmentId));

//            if (result.IsSuccess)
//            {
//                equipment = result.Value;
//            }
//            else
//            {
//                errorMessage = $"Erreur de récupération des informations de l'équipement.";
//            }
//        }
//        catch (Exception ex)
//        {
//            errorMessage = $"Erreur: {ex.Message}";
//        }
//    }

//    private async Task UpdateEquipment()
//    {
//        if (equipment.Id == Guid.Empty)
//        {
//            errorMessage = "L'ID de l'équipement est invalide.";
//            return;
//        }

//        var updateResult = await ProxyService.PostAsync<EquipmentResult>(PreventionRoutes.Equipments.Update.Replace("{id:guid}", equipment.Id.ToString()), equipment);
//        if (updateResult.IsSuccess)
//        {
//            successMessage = "Équipement mis à jour avec succès.";
//            errorMessage = null;
//            Navigation.NavigateTo("/equipements");
//        }
//        else
//        {
//            errorMessage = "Erreur lors de la mise à jour des informations de l'équipement.";
//        }
//    }
//}