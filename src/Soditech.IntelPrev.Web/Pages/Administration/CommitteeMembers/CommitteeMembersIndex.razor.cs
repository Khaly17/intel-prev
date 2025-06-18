using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Soditech.IntelPrev.Prevensions.Shared;
using Soditech.IntelPrev.Prevensions.Shared.CommitteeMembers;
using Soditech.IntelPrev.Users.Shared.Tenants;
using Syncfusion.Blazor.Grids;

namespace Soditech.IntelPrev.Web.Pages.Administration.CommitteeMembers;

public partial class CommitteeMembersIndex : ComponentBase
{
    public IList<CommitteeMemberResult> CommitteeMembers { get; set; } = [];
    private int PageCount { get; set; } = 10;
    private int PageSize { get; set; } = 10;
    private bool IsLoading { get; set; }

    private static List<GridColumn> Columns =>
    [
        new() { Field = nameof(CommitteeMemberResult.FirstName), HeaderText = "Prénom" },
        new() { Field = nameof(CommitteeMemberResult.LastName), HeaderText = "Nom" },
        new() { Field = nameof(CommitteeMemberResult.Email), HeaderText = "Email" },
        new() { Field = nameof(CommitteeMemberResult.PhoneNumber), HeaderText = "Téléphone" },
    ];
    // toolbar items
    private static List<string> ToolbarItems => ["Search", "ExcelExport", "PdfExport", "Print"];

    private bool _isDeleteModalVisible;
    private string _alertMessage = string.Empty;
    private string _alertType = "success";
    private bool _isAlertVisible;
    private CommitteeMemberResult SelectedCommitteeMember { get; set; } = default!;
    public string TenantIdCacheKey { get; set; } = "selectedTenant";
    private TenantResult SelectedTenant { get; set; } = default!;

    private const string CommitteeMembersCacheKey = "CommitteeMembers";

    [Inject] private ILogger<CommitteeMembersIndex> Logger { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await LoadCommitteeMembersAsync();
    }

    private async Task LoadCommitteeMembersAsync()
    {
        IsLoading = true;
        var (exists, cachedValue) = CacheService.Get(CommitteeMembersCacheKey);

        if (exists)
        {
            CommitteeMembers = (IList<CommitteeMemberResult>)cachedValue;
        }
        else
        {
            await LoadCommitteeMembersFromApiAsync();
            CacheService.Set(CommitteeMembersCacheKey, CommitteeMembers);
        }
        IsLoading = false;
    }
    public async Task LoadCommitteeMembersFromApiAsync()
    {
        IsLoading = true;
        var committeeMembersResult = await ProxyService.GetAsync<IList<CommitteeMemberResult>>(PreventionRoutes.CommitteeMembers.GetAll);

        if (committeeMembersResult.IsSuccess)
        {
            CommitteeMembers = committeeMembersResult.Value;
        }
        IsLoading = false;
    }

    private void DeleteCommitteeMemberTrigger(CommitteeMemberResult committeeMember)
    {
        SelectedCommitteeMember = committeeMember;
        _isDeleteModalVisible = true;
    }

    private void HideDeleteModal() => _isDeleteModalVisible = false;

    private void AddCommitteeMember()
    {
        Navigation.NavigateTo("/committeemembers/add");
    }

    private async Task DeleteCommitteeMemberAsync()
    {
        try
        {
            var result = await ProxyService.DeleteAsync(PreventionRoutes.CommitteeMembers.Delete.Replace("{id:guid}", SelectedCommitteeMember.Id.ToString()));

            if (result.IsSuccess)
            {
                CommitteeMembers = CommitteeMembers.Where(n => n.Id != SelectedCommitteeMember.Id).ToList();
                ShowAlert("Membre du comité supprimé avec succès.");
            }
            else
            {
                ShowAlert("Échec de la suppression du membre du comité.", "danger");
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Erreur lors de la suppression du membre du comité : {Message}", ex.Message);
            ShowAlert("Une erreur est survenue lors de la suppression du membre du comité. Veuillez réessayer.", "danger");
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

    private void GoToEdit(CommitteeMemberResult committeeMember)
    {
        Navigation.NavigateTo($"committeemembers/edit/{committeeMember.Id}");
    }
}