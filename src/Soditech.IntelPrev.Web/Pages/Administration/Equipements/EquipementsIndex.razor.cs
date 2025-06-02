//using Microsoft.AspNetCore.Components;
//using Syncfusion.Blazor.Grids;
//using Soditech.IntelPrev.Preventions.Shared.Equipments;
//using Soditech.IntelPrev.Preventions.Shared;

//namespace Soditech.IntelPrev.Web.Pages.Administration.Equipements;

//public partial class EquipementsIndex : ComponentBase
//{
//    public IList<EquipmentResult> EquipmentList { get; set; } = [];
//    private int PageCount { get; set; } = 10;
//    private int PageSize { get; set; } = 10;
//    private bool IsLoading { get; set; }

//    private static List<GridColumn> Columns =>
//    [
//        new GridColumn { Field = nameof(EquipmentResult.Name), HeaderText = "Nom" },
//        new GridColumn { Field = nameof(EquipmentResult.Type), HeaderText = "Type" },
//        new GridColumn { Field = nameof(EquipmentResult.Location), HeaderText = "Lieu" },
//        new GridColumn { Field = nameof(EquipmentResult.LastInspectionDate), HeaderText = "Dernière inspection" },
//        new GridColumn { Field = nameof(EquipmentResult.NextInspectionDate), HeaderText = "Prochaine inspection" },
//        new GridColumn { Field = nameof(EquipmentResult.Description), HeaderText = "Description" }
//    ];
//    // toolbar items
//    private static List<string> ToolbarItems => ["Search", "ExcelExport", "PdfExport", "Print"];

//    private bool _isDeleteModalVisible;
//    private string _alertMessage = string.Empty;
//    private string _alertType = "success";
//    private bool _isAlertVisible;
//    private EquipmentResult SelectedEquipment { get; set; } = default!;

//    [Inject] private ILogger<EquipementsIndex> Logger { get; set; } = default!;

//    protected override async Task OnInitializedAsync()
//    {
//        await GetEquipments();
//    }

//    private async Task GetEquipments()
//    {
//        IsLoading = true;
//        var equipmentsResult = await ProxyService.GetAsync<IList<EquipmentResult>>(PreventionRoutes.Equipments.GetAll);

//        if (equipmentsResult.IsSuccess)
//        {
//            EquipmentList = equipmentsResult.Value;
//        }

//        IsLoading = false;
//    }

//    private void DeleteEquipmentTrigger(EquipmentResult equipment)
//    {
//        SelectedEquipment = equipment;
//        _isDeleteModalVisible = true;
//    }

//    private void HideDeleteModal() => _isDeleteModalVisible = false;

//    private void AddEquipment()
//    {
//        Navigation.NavigateTo("/equipements/add");
//    }

//    private async Task DeleteEquipmentAsync()
//    {
//        try
//        {
//            var result = await ProxyService.DeleteAsync(PreventionRoutes.Equipments.Delete.Replace("{id:guid}", SelectedEquipment.Id.ToString()));

//            if (result.IsSuccess)
//            {
//                EquipmentList = EquipmentList.Where(n => n.Id != SelectedEquipment.Id).ToList();
//                ShowAlert("Équipement supprimé avec succès.");
//            }
//            else
//            {
//                ShowAlert("Échec de la suppression de l'équipement.", "danger");
//            }
//        }
//        catch (Exception ex)
//        {
//            Logger.LogError(ex, "Erreur lors de la suppression de l'équipement : {Message}", ex.Message);
//            ShowAlert("Une erreur est survenue lors de la suppression de l'équipement. Veuillez réessayer.", "danger");
//        }
//        finally
//        {
//            HideDeleteModal();
//        }
//    }

//    private void ShowAlert(string message, string type = "success")
//    {
//        _alertMessage = message;
//        _alertType = type;
//        _isAlertVisible = true;
//        Task.Delay(3000).ContinueWith(_ =>
//        {
//            _isAlertVisible = false;
//            StateHasChanged();
//        });
//    }

//    private void GoToEdit(EquipmentResult equipment)
//    {
//        Navigation.NavigateTo($"equipements/edit/{equipment.Id}");
//    }
//}