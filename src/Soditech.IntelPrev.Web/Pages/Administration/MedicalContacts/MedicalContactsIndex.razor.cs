using Microsoft.AspNetCore.Components;
using Soditech.IntelPrev.Preventions.Shared;
using Soditech.IntelPrev.Preventions.Shared.CommitteeMembers;
using Soditech.IntelPrev.Preventions.Shared.MedicalContacts;
using Syncfusion.Blazor.Grids;
using System.ComponentModel;

namespace Soditech.IntelPrev.Web.Pages.Administration.MedicalContacts;

public partial class MedicalContactsIndex
{
    public IList<MedicalContactResult> MedicalContacts { get; set; } = [];
    private int PageCount { get; set; } = 10;
    private int PageSize { get; set; } = 10;
    private bool IsLoading { get; set; }
    private static List<GridColumn> Columns =>
    [
        new GridColumn { Field =  nameof(MedicalContactResult.FirstName), HeaderText = "Prénom" },
            new GridColumn { Field = nameof(MedicalContactResult.LastName), HeaderText = "Nom" },
            new GridColumn { Field = nameof(MedicalContactResult.Email), HeaderText = "Email" },
            new GridColumn { Field = nameof(MedicalContactResult.PhoneNumber), HeaderText = "Téléphone" },
            new GridColumn { Field = nameof(MedicalContactResult.Position), HeaderText = "Position" }
    ];
    // toolbar items
    private static List<string> ToolbarItems => ["Search", "ExcelExport", "PdfExport", "Print"];

    private bool _isDeleteModalVisible;
    private string _alertMessage = string.Empty;
    private string _alertType = "success";
    private bool _isAlertVisible;
    private MedicalContactResult SelectedMedicalContact { get; set; } = default!;
    private const string MedicalContactsCacheKey = "MedicalContacts";

    [Inject] private ILogger<MedicalContactsIndex> Logger { get; set; } = default!;


    protected override async Task OnInitializedAsync()
    {
        await LoadMedicalContactsAsync();
    }
    private async Task LoadMedicalContactsAsync()
    {
        IsLoading = true;
        var (exists, cachedValue) = CacheService.Get(MedicalContactsCacheKey);

        if (exists)
        {
            MedicalContacts = (IList<MedicalContactResult>)cachedValue;
        }
        else
        {
            await LoadMedicalContactsFromApiAsync();
            CacheService.Set(MedicalContactsCacheKey, MedicalContacts);
        }
        IsLoading = false;
    }
    private async Task LoadMedicalContactsFromApiAsync()
    {

        IsLoading = true;
        var medicalContactResult = await ProxyService.GetAsync<IList<MedicalContactResult>>(PreventionRoutes.MedicalContacts.GetAll);

        if (medicalContactResult.IsSuccess)
        {
            MedicalContacts = medicalContactResult.Value;
        }

        IsLoading = false;
    }
    private void DeleteMedicalContactTrigger(MedicalContactResult medicalContact)
    {
        SelectedMedicalContact = medicalContact;
        _isDeleteModalVisible = true;
    }

    private void HideDeleteModal() => _isDeleteModalVisible = false;

    // add method 
    private void AddMedicalContact()
    {
        Navigation.NavigateTo("/medicalcontacts/add");
    }

    private async Task DeleteMedicalContactAsync()
    {
        try
        {
            var result = await ProxyService.DeleteAsync(PreventionRoutes.Buildings.Delete.Replace("{id:guid}", SelectedMedicalContact.Id.ToString()));

            if (result.IsSuccess)
            {
                MedicalContacts = MedicalContacts.Where(n => n.Id != SelectedMedicalContact.Id).ToList();
                ShowAlert("Medical contact deleted successfully.");
            }
            else
            {
                ShowAlert("Failed to delete medical contact.", "danger");
            }

        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error deleting medical contact: {Message}", ex.Message);
            ShowAlert("An error occurred while deleting the User. Please try again.", "danger");
        }
        finally
        {
            HideDeleteModal();
        }
    }

    private void ShowAlert(string message, string type = "success")
    {
        _alertMessage = message;
        _alertType = type;
        _isAlertVisible = true;
        Task.Delay(3000).ContinueWith(_ =>
        {
            _isAlertVisible = false;
            StateHasChanged();
        });
    }

    private void GoToEdit(MedicalContactResult medical)
    {

        Navigation.NavigateTo($"medicalcontacts/edit/{medical.Id}");

    }

}
