//using Microsoft.AspNetCore.Components;
//using Soditech.IntelPrev.Preventions.Shared.Equipments;
//using Soditech.IntelPrev.Users.Shared.Tenants;
//using Soditech.IntelPrev.Users.Shared;
//using Soditech.IntelPrev.Preventions.Shared;

//namespace Soditech.IntelPrev.Web.Pages.Administration.Equipements;

//public partial class AddEquipment
//{
//    public EquipmentResult NewEquipment { get; set; } = default!;
//    public string title { get; set; } = "Ajouter un équipement";
//    public string? errorMessage { get; set; }
//    public string? successMessage { get; set; }
//    [Inject] private ILogger<AddEquipment> Logger { get; set; } = default!;

//    private async Task CreateEquipment()
//    {
//        errorMessage = null;
//        successMessage = null;
//        try
//        {
//            var result = await ProxyService.PostAsync<EquipmentResult>(PreventionRoutes.Equipments.Create, NewEquipment);

//            if (result.IsSuccess)
//            {
//                successMessage = "L'équipement a été ajouté avec succès !";
//                var equipmentId = result.Value.Id;

//                Navigation.NavigateTo("/equipements");
//            }
//            else
//            {
//                errorMessage = result.Error?.Message ?? "Une erreur est survenue lors de la création de l'équipement";
//                Logger.LogError("{code} : {message}", result.Error?.Code, result.Error?.Message);
//            }
//        }
//        catch (Exception ex)
//        {
//            errorMessage = $"Une erreur interne est survenue lors de la création de l'équipement.";
//            Logger.LogError(ex, errorMessage);
//        }
//    }
//}