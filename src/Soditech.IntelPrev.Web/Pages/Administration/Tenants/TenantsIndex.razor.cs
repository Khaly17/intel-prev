using Microsoft.AspNetCore.Components;
using Soditech.IntelPrev.Users.Shared;
using Soditech.IntelPrev.Users.Shared.Roles;
using Soditech.IntelPrev.Users.Shared.Tenants;
using Soditech.IntelPrev.Users.Shared.Users;
using Soditech.IntelPrev.Web.Components.Widgets.Tables;
using Syncfusion.Blazor.Grids;

namespace Soditech.IntelPrev.Web.Pages.Administration.Tenants;

public partial class TenantsIndex : ComponentBase
{
    private IList<TenantResult> Tenants { get; set; } = [];
    public IList<UserResult> Users { get; set; } = [];
    private int PageCount { get; set; } = 10;
    private int PageSize { get; set; } = 10;
    private bool IsLoading { get; set; }
    private bool IsLoadingUsers { get; set; }
    public string TenantIdCacheKey { get; set; } = "selectedTenant";

    private string SelectedTenantId { get; set; } = string.Empty;


    private static List<GridColumn> Columns =>
    [
        new() { Field =  nameof(TenantResult.Name), HeaderText = "Nom" },
        new() { Field = nameof(TenantResult.DisplayName), HeaderText = "Nom visible" },
        new() { Field = nameof(TenantResult.Description), HeaderText = "Description" },
        new() { Field = nameof(TenantResult.IsActive), HeaderText = "Statut", DisplayAsCheckBox = true },
        new() { Field = nameof(TenantResult.RolesCount), HeaderText = "Nbre de roles" },
        new() { Field = nameof(TenantResult.UsersCount), HeaderText = "Nbre de personnes" },
    ];
    // toolbar items
    private static List<string> ToolbarItems => ["Search", "ExcelExport", "PdfExport", "Print"];
    
    private bool _isDeleteModalVisible;
    private string _alertMessage = string.Empty;
    private string _alertType = "success";
    private bool _isAlertVisible ;
    private bool _isDisableTenantModalVisible;
    private TenantResult SelectedTenant { get; set; } = default!;

    [Inject] private ILogger<TenantsIndex> Logger { get; set; } = default!;


    protected override async Task OnInitializedAsync()
    {
        await GetTenants();

    }

    private async Task GetTenants()
    {
        IsLoading = true;
        var tenantsResult = await ProxyService.GetAsync<IEnumerable<TenantResult>>(UserRoutes.Tenants.GetAll);

        if (tenantsResult.IsSuccess)
        {
            Tenants = tenantsResult.Value.ToList();
        }

        IsLoading = false;
    }
    private void DeleteTenantTrigger(TenantResult tenant)
    {
        SelectedTenant = tenant;
        _isDeleteModalVisible = true;
    }

    private void HideDeleteModal() => _isDeleteModalVisible = false;
    
    // add method 
    private void AddTenant()
    {
        Navigation.NavigateTo("/tenants/add");
    }
    
    private async Task DeleteTenantAsync()
    {
        try
        {
            var result = await ProxyService.DeleteAsync(UserRoutes.Tenants.Delete.Replace("{id:guid}", SelectedTenant.Id.ToString()));

            if (result.IsSuccess)
            {
                Tenants = Tenants.Where(n => n.Id != SelectedTenant.Id).ToList();
                ShowAlert("Structure supprimé avec succès.");
            }
            else
            {
                ShowAlert("Échec de la suppression de la structure.", "danger");
            }

        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Erreur lors de la suppression de la tructure : {Message}", ex.Message);
            ShowAlert("Une erreur s'est produite lors de la suppression de la tructure. Veuillez réessayer.", "danger");
        }
        finally
        {
            HideDeleteModal();
        }
    }

    private void DisableTenantTrigger(TenantResult tenant)
    {
        SelectedTenant = tenant;
        _isDisableTenantModalVisible = true;
    }

    private void HideDisableTenantModal() => _isDisableTenantModalVisible = false;

    private async Task DisableTenantAsync()
    {
        try
        {
            var request = new DisableTenantCommand(SelectedTenant.Id);

            var result = await ProxyService.PostAsync(UserRoutes.Tenants.Disable.Replace("{id:guid}", SelectedTenant.Id.ToString()), request);

            if (result.IsSuccess)
            {
                ShowAlert("Structure désactivée avec succès.");
            }
            else
            {
                ShowAlert("Échec de la désactivation de la structure.", "danger");
            }

        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Erreur lors de la désactivation de la structure : {Message}", ex.Message);
            ShowAlert("Une erreur s'est produite lors de la désactivation de la structure. Veuillez réessayer.", "danger");
        }
        finally
        {
            HideDisableTenantModal();
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

   
    
     private void GoToEdit(TenantResult tenant)
    {
        Navigation.NavigateTo($"tenants/edit/{tenant.Id}");

    }

    private async Task LoadUsersFromApiAsync(string SelectedTenantId)
    {

        IsLoadingUsers = true;
        var usersResult = await ProxyService.GetAsync<IList<UserResult>>(UserRoutes.Tenants.GetById.Replace("{id:guid}", SelectedTenantId)+"/users");

        if (usersResult.IsSuccess)
        {
            Users = usersResult.Value;
        }

        IsLoadingUsers = false;
    }

    private async Task LoadUsersByRoleIdAsync(string roleId)
    {

        await LoadUsersFromApiAsync(roleId);

    }

    GenericGrid<TenantResult> Grid;

    public async Task GetSelectedTenant(RowSelectEventArgs<TenantResult> args)
    {
        var selectedRowIndexes = await this.Grid.GenGrid.GetSelectedRowIndexesAsync();
        var index = selectedRowIndexes.First();

        var selectedTenant = Tenants.ElementAt(index);

        await ShowUsersByTenantIdAsync(selectedTenant);

        StateHasChanged();
    }

    private async Task ShowUsersByTenantIdAsync(TenantResult tenantResult)
    {
        IsLoadingUsers = true;

        CacheService.Set(TenantIdCacheKey, tenantResult);
        SelectedTenantId = tenantResult.Id.ToString();
        await LoadUsersByRoleIdAsync(SelectedTenantId);
        IsLoadingUsers = false;

        StateHasChanged();
    }

}