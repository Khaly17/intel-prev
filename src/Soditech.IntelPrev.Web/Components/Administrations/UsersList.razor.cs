using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Soditech.IntelPrev.Users.Shared.Users;
using Soditech.IntelPrev.Users.Shared;
using Syncfusion.Blazor.Grids;

namespace Soditech.IntelPrev.Web.Components.Administrations;

public partial class UsersList: ComponentBase
{
    [Parameter]
    public IList<UserResult> Users { get; set; } = [];
    private int PageCount { get; set; } = 10;
    private int PageSize { get; set; } = 10;
    private bool IsLoading { get; set; }


    private static List<GridColumn> Columns =>
    [
        new() { Field =  nameof(UserResult.TenantName), HeaderText = "Structure" },
        new() { Field = nameof(UserResult.UserName), HeaderText = "Identifiant" },
        new() { Field = nameof(UserResult.FirstName), HeaderText = "Prénom" },
        new() { Field = nameof(UserResult.LastName), HeaderText = "Nom" },
        new() { Field = nameof(UserResult.AppVersion), HeaderText = "Version"},
    ];
    // toolbar items
    private static List<string> ToolbarItems => ["Search", "ExcelExport", "PdfExport", "Print"];

    private bool _isDeleteModalVisible;
    private string _alertMessage = string.Empty;
    private string _alertType = "success";
    private bool _isAlertVisible;
    private bool _isResetPasswordModalVisible;
    private UserResult SelectedUser { get; set; } = default!;

    [Inject] private ILogger<UsersList> Logger { get; set; } = default!;


    private void DeleteUserTrigger(UserResult user)
    {
        SelectedUser = user;
        _isDeleteModalVisible = true;
    }

    private void HideDeleteModal() => _isDeleteModalVisible = false;

    // add method 
    private void AddUser()
    {
        Navigation.NavigateTo("/users/add");
    }

    private async Task DeleteUserAsync()
    {
        try
        {
            var result = await ProxyService.DeleteAsync(UserRoutes.Users.Delete.Replace("{id:guid}", SelectedUser.Id.ToString()));

            if (result.IsSuccess)
            {
                Users = Users.Where(n => n.Id != SelectedUser.Id).ToList();
                ShowAlert("User deleted successfully.");
            }
            else
            {
                ShowAlert("Failed to delete User.", "danger");
            }

        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error deleting User: {Message}", ex.Message);
            ShowAlert("An error occurred while deleting the User. Please try again.", "danger");
        }
        finally
        {
            HideDeleteModal();
        }
    }

    private void ResetPasswordTrigger(UserResult user)
    {
        SelectedUser = user;
        _isResetPasswordModalVisible = true;
    }

    private void HideResetPasswordModal() => _isResetPasswordModalVisible = false;

    private async Task ResetPasswordAsync()
    {
        try
        {
            var request = new ResetPasswordCommand(SelectedUser.Id);

            var result = await ProxyService.PostAsync(UserRoutes.Users.ResetPassword, request);

            if (result.IsSuccess)
            {
                ShowAlert("Password reset successfully.");
            }
            else
            {
                ShowAlert("Failed to reset password.", "danger");
            }

        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error resetting password: {Message}", ex.Message);
            ShowAlert("An error occurred while resetting the password. Please try again.", "danger");
        }
        finally
        {
            HideResetPasswordModal();
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



    private void GoToEdit(UserResult user)
    {
        Navigation.NavigateTo($"users/edit/{user.Id}");

    }

    private async Task GetUsers()
    {

        IsLoading = true;
        var usersResult = await ProxyService.GetAsync<IList<UserResult>>(UserRoutes.Users.GetUsers);

        if (usersResult.IsSuccess)
        {
            Users = usersResult.Value;
        }

        IsLoading = false;
    }
}
