using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Soditech.IntelPrev.Reports.Shared;
using Soditech.IntelPrev.Reports.Shared.RegisterTypes;
using Syncfusion.Blazor.Grids;

namespace Soditech.IntelPrev.Web.Pages.Administration.RegisterTypes;
public partial class RegisterIndex
{
    public IList<RegisterTypeResult> Registers { get; set; } = [];
    private int PageCount { get; set; } = 10;
    private int PageSize { get; set; } = 10;
    private bool IsLoading { get; set; }

    private static List<GridColumn> Columns =>
    [
        new() { Field = nameof(RegisterTypeResult.Name), HeaderText = "Nom" },
        new() { Field = nameof(RegisterTypeResult.Description), HeaderText = "Description" },
        new() { Field = nameof(RegisterTypeResult.IsActive), HeaderText = "Actif" , DisplayAsCheckBox = true}
    ];

    // toolbar items
    private static List<string> ToolbarItems => ["Search", "ExcelExport", "PdfExport", "Print"];

    private bool _isDeleteModalVisible;
    private string _alertMessage = string.Empty;
    private string _alertType = "success";
    private bool _isAlertVisible;
    private const string RegisterCacheKey = "Register";
    private RegisterTypeResult SelectedRegister { get; set; } = default!;

    [Inject] private ILogger<RegisterTypeResult> Logger { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await LoadRegisterAsync();
    }

    private void DeleteRegisterTrigger(RegisterTypeResult register)
    {
        SelectedRegister = register;
        _isDeleteModalVisible = true;
    }

    private void HideDeleteModal() => _isDeleteModalVisible = false;

    private void AddRegister()
    {
        Navigation.NavigateTo("/registers/add");
    }

    private async Task DeleteRegisterAsync()
    {
        try
        {
            var result = await ProxyService.DeleteAsync(ReportRoutes.RegisterTypes.Delete.Replace("{id:guid}", SelectedRegister.Id.ToString()));

            if (result.IsSuccess)
            {
                Registers = Registers.Where(n => n.Id != SelectedRegister.Id).ToList();
                ShowAlert("Registre supprimé avec succès.");
            }
            else
            {
                ShowAlert("Échec de la suppression du registre.", "danger");
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Erreur lors de la suppression du registre : {Message}", ex.Message);
            ShowAlert("Une erreur s'est produite lors de la suppression du registre. Veuillez réessayer.", "danger");
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

    private void GoToEdit(RegisterTypeResult register)
    {
        Navigation.NavigateTo($"registers/edit/{register.Id}");
    }

    private async Task LoadRegisterAsync()
    {
        IsLoading = true;
        var (exists, cachedValue) = CacheService.Get(RegisterCacheKey);

        if (exists)
        {
            Registers = (IList<RegisterTypeResult>)cachedValue;
        }
        else
        {
            await GetRegisterFromApiAsync();
            CacheService.Set(RegisterCacheKey, Registers);
        }
        IsLoading = false;
    }
    private async Task GetRegisterFromApiAsync()
    {
        IsLoading = true;
        var registersResult = await ProxyService.GetAsync<IList<RegisterTypeResult>>(ReportRoutes.RegisterTypes.GetAll);

        if (registersResult.IsSuccess)
        {
            Registers = registersResult.Value;
        }
        IsLoading = false;
    }
}