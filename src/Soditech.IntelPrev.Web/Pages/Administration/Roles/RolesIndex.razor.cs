using Microsoft.AspNetCore.Components;
using Soditech.IntelPrev.Users.Shared;
using Soditech.IntelPrev.Users.Shared.Roles;
using Soditech.IntelPrev.Users.Shared.Users;
using Soditech.IntelPrev.Web.Components.Widgets.Tables;
using Syncfusion.Blazor.Grids;
using Action = System.Action;

namespace Soditech.IntelPrev.Web.Pages.Administration.Roles;

public partial class RolesIndex : ComponentBase
{
    private IEnumerable<RoleResult> _roles = [];
    private IEnumerable<UserResult> _users = [];
    private IEnumerable<UserResult> _allUsers = [];
    private IEnumerable<UserResult> _filteredUsers = [];
    private IEnumerable<UserResult> _selectedUsers = [];
    // private IEnumerable<SiteResult> _sites = [];

    private RoleResult Role { get; set; } = new RoleResult();
    private RoleResult SelectedRoleToDelete { get; set; } = new RoleResult();
    [Inject] private ILogger<RolesIndex> Logger { get; set; } = default!;

    private bool IsLoadingRoles { get; set; }
    private bool IsLoadingUsers { get; set; }
    private bool IsAssignUserModalVisible { get; set; }
    private bool IsCreatedRole { get; set; }
    private string CreateRoleLabel { get; set; } = "Ajouter un nouveau rôle";
    private string AffectLabel { get; set; } = "Affectation de rôle";
    private string SelectedUserId { get; set; } = string.Empty;
    private string SelectedRoleId { get; set; } = string.Empty;
    private string SelectedSiteId { get; set; } = string.Empty;
    private string SelectedRoleScope { get; set; } = string.Empty;
    private string SelectedRole { get; set; } = string.Empty;

    // public string ViewUsersLabel = "View Users";
    private const string unassignLabel = "Enlever";
    // public string UnassignIconCss = "fa fa-refresh";

    private const string RolesCacheKey = "Roles";
    private const string RoleIdCacheKey = "Role_";
    private const string AllUsersCacheKey = "Users";
    // private const string SitesCacheKey = "Sites";

        private string _headerConfirmLabel = "Confirmer la désaffectation";
        private string _confirmMessage = "Êtes-vous sûr de vouloir désaffecter l'utilisateur";
        private string _confirmLabel = "Confirmer";
        private bool _isConfirmModalVisible = false;

        private string _headerDeleteLabel = "Confirmer la suppression";
        private string _deleteMessage = "Êtes-vous sûr de vouloir supprimer ce rôle";
        private string _deleteLabel = "Confirmer";
        private bool _isDeleteModalVisible = false;

        private string _alertMessage = string.Empty;
        private string _alertType = "success";
        private bool _isAlertVisible = false;
        private bool _isAddRoleVisible = false;

        private int _totalCount;
        private int _pageSize = 30;
        private int _currentPage = 1;
        private int _pageCount = 1;

        private int _userTotalCount;
        private int _userPageSize = 10;
        private int _userCurrentPage = 1;
        private int _userPageCount = 1;

        private static List<GridColumn> Columns => new()
        {
            new GridColumn { Field = "Scope", HeaderText = "Type" },
            new GridColumn { Field = "Name", HeaderText = "Nom" },
            new GridColumn { Field = "Description", HeaderText = "Description" },
            new GridColumn { Field = "UsersCount", HeaderText = "Utilisateurs" }
        };

        private List<GridColumn> UsersColumns { get; set; } = [];
        
        private static List<string> ToolbarItems => ["Search", "ExcelExport", "PdfExport", "Print"];

        private string RoleUsersTittle { get; set; } = "Membres du Rôle";

        private bool _isLoading;

        protected override async Task OnInitializedAsync()
        {
            var tasks = new List<Task>()
            {
                LoadRolesAsync(),
                LoadAllUsersAsync(),
                //LoadSitesAsync()
            };

            await Task.WhenAll(tasks);
        }

        private async Task LoadRolesAsync()
        {
            _isLoading = true;
            var (exists, cachedValue) = CacheService.Get(RolesCacheKey);

            if (exists)
            {
                _roles = (IList<RoleResult>)cachedValue;
            }
            else
            {
                await LoadRolesFromApiAsync();
                CacheService.Set(RolesCacheKey, _roles);
            }
            _isLoading = false;
        }

        private async Task LoadRolesFromApiAsync()
        {
            IsLoadingRoles = true;
            var result = await ProxyService.GetAsync<IEnumerable<RoleResult>>(UserRoutes.Roles.GetAll);
            if (result.IsSuccess)
            {
                _roles = result.Value ?? [];
                _totalCount = _roles.Count();
                _pageCount = CalculatePageCount(_totalCount, _pageSize);
            }
            IsLoadingRoles = false;
        }

        private async Task LoadUsersByRoleIdAsync(string roleId)
        {
            var userCacheKey = $"{RoleIdCacheKey}{roleId}";

            var (exists, cachedValue) = CacheService.Get(userCacheKey);

            if (exists)
            {
                _users = (IList<UserResult>)cachedValue;
                _selectedUsers = _users.ToList();
                _filteredUsers = _allUsers.Except(_selectedUsers).ToList();
                // await Task.Delay(10);
            }
            else
            {
                await LoadUsersFromApiAsync(roleId);
            }

        }

        private async Task LoadUsersFromApiAsync(string roleId)
        {
            var userCacheKey = $"{RoleIdCacheKey}{roleId}";
            
            var result = await ProxyService.GetAsync<IEnumerable<UserResult>>(UserRoutes.Roles.GetUsers.Replace("{id:guid}", roleId));
            if (result.IsSuccess)
            {
                _users = result.Value ?? [];
                _selectedUsers = _users.ToList();
                _filteredUsers = _allUsers.Except(_selectedUsers, new UserResultComparer()).ToList();
                CacheService.Set(userCacheKey, _users);
            }
        }
     

        private async Task LoadAllUsersAsync()
        {
            var (exists, cachedValue) = CacheService.Get(AllUsersCacheKey);

            if (exists)
            {
                _allUsers = (IList<UserResult>)cachedValue;
            }
            else
            {
                await LoadAllUsersFromApiAsync();
            }
        }
        private async Task LoadAllUsersFromApiAsync()
        {
            var result = await ProxyService.GetAsync<IEnumerable<UserResult>>(UserRoutes.Users.GetUsers);
            if (result.IsSuccess)
            {
                _allUsers = result.Value ?? [];
            }

        }

        private async Task ShowUsersByRoleIdAsync(RoleResult roleResult)
        {
            IsLoadingUsers = true;
            SelectedRoleId = roleResult.Id.ToString();
            SelectedRoleScope = roleResult.Scope;
            RoleUsersTittle = $"Membres du rôles {roleResult.Name}";
            // updated columns according to the selected role
            await UpdateUserColumns(roleResult.Scope);  // Scope : maybe "Application", "Site", "National", etc.

            await LoadUsersByRoleIdAsync(SelectedRoleId); 
           
            IsLoadingUsers = false;
            
            StateHasChanged(); 
        }

        private void OpenAssignUserDialog() => IsAssignUserModalVisible = true;
        private void OpenAddRoleDialog() => IsCreatedRole = true;   
        private async Task AssignUserToRole()
        {
            var data = new
            {
                userId = SelectedUserId,
                roleId = SelectedRoleId,
                siteId = SelectedRoleScope == "Site" ? SelectedSiteId : null
            };
            var result = await ProxyService.PostAsync<AffectRoleToUserCommand>(UserRoutes.Roles.AffectToUser, data);
            if (result.IsSuccess)
            {

                var assignedUser = _allUsers.FirstOrDefault(u => u.Id == Guid.Parse(SelectedUserId));
                if (assignedUser != null)
                {
                    _selectedUsers = _selectedUsers.Append(assignedUser).ToList();
                    _filteredUsers = _allUsers.Except(_selectedUsers).ToList(); 
                }

                IsAssignUserModalVisible = false;
                ShowDefaultAlert("Assignation successfully.", "success");
            }

            StateHasChanged() ;
            await Task.CompletedTask;
        }
        private async Task UnAssignUserToRole(string userId)
        {
            var data = new
            {
                userId = userId,
                roleId = SelectedRoleId
            };
            var result = await ProxyService
                .PostAsync<UnAffectRoleToUserCommand>
                (UserRoutes.Roles.UnAffectToUser, data);
            if (result.IsSuccess)
            {

                var userToRemove = _selectedUsers.FirstOrDefault(u => u.Id == Guid.Parse(userId));
                if (userToRemove != null)
                {
                    _selectedUsers = _selectedUsers.Where(u => u.Id != userToRemove.Id).ToList();
                    _filteredUsers = _allUsers.Except(_selectedUsers).ToList(); // Mettre à jour les utilisateurs non affectés
                }

                IsAssignUserModalVisible = false;
                ShowDefaultAlert("UnAssignation successfully.", "success");

                StateHasChanged();
            }
            await Task.CompletedTask;
        }

        private void HideModals()
        {
            IsAssignUserModalVisible = false;
            IsCreatedRole = false;
        }
        private void HideConfirmModal()
        {
            _isConfirmModalVisible = false;
            _isDeleteModalVisible = false;
        }

        private void ShowDetails(UserResult userResult)
        {
            SelectedUserId = userResult.Id.ToString();
            _isConfirmModalVisible = true;

            StateHasChanged();
        }
        private async Task ConfirmUnAssignAsync()
        {
            await UnAssignUserToRole(SelectedUserId);

            _isConfirmModalVisible = false;

            StateHasChanged();
        }

        private int CalculatePageCount(int totalCount, int pageSize) =>
            pageSize > 0 ? (int)Math.Ceiling((double)totalCount / pageSize) : 1;
        

        private void ShowAlerts(Action setVisible, Action setInvisible, string message, string type = "success")
        {
            _alertMessage = message;
            _alertType = type;
            setVisible?.Invoke();

            Task.Delay(3000).ContinueWith(_ =>
            {
                setInvisible?.Invoke();
                StateHasChanged();
            });
        }

        private void ShowDefaultAlert(string message, string type = "success")
        {
            ShowAlerts(() => _isAlertVisible = true, () => _isAlertVisible = false, message, type);
        }

        private void ShowAddRoleAlert(string message, string type = "success")
        {
            ShowAlerts(() => _isAddRoleVisible = true, () => _isAddRoleVisible = false, message, type);
        }
        

        private async Task SaveRole()
        {
            var result = await ProxyService.PostAsync<RoleResult>(
                UserRoutes.Roles.Create,
                Role
            );

            if (result.IsSuccess)
            {
                _roles = _roles.Append(result.Value).ToList();

                CacheService.Set(RolesCacheKey, _roles);

                _totalCount = _roles.Count();
                _pageCount = CalculatePageCount(_totalCount, _pageSize);
                StateHasChanged();
                
                ShowAddRoleAlert("Role ajouté avec succès.", "success");
                IsCreatedRole = false;
            }
            else
            {
                ShowAddRoleAlert(result.Error.Message, "danger");
            }
        }

        private async Task DeleteRole()
        {
            _isDeleteModalVisible = false;

            try
            {
                var result = await ProxyService.DeleteAsync(UserRoutes.Roles.Delete.Replace("{id:guid}", SelectedRoleToDelete.Id.ToString()));

                if (result.IsSuccess)
                {
                    _roles = _roles.Where(r => r.Id != SelectedRoleToDelete.Id).ToList();

                    CacheService.Set(RolesCacheKey, _roles);

                    _totalCount = _roles.Count();
                    _pageCount = CalculatePageCount(_totalCount, _pageSize);

                    ShowAddRoleAlert("Role deleted successfully.", "success");
                }
                else
                {
                    ShowAddRoleAlert("Failed to delete the role.", "danger");
                }

                StateHasChanged();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error deleting role: {Message}", ex.Message);
                ShowAddRoleAlert("An error occured while deleting the role.", "danger");
            }
        }

        private void ShowDeleteRoleModal(RoleResult roleResult)
        {
            _isDeleteModalVisible = true;
            SelectedRoleToDelete = roleResult;
        }

        GenericGrid<RoleResult> Grid;
        
        public async Task GetSelectedRole(RowSelectEventArgs<RoleResult> args)
        {
            var selectedRowIndexes = await this.Grid.GenGrid.GetSelectedRowIndexesAsync();
            var index = selectedRowIndexes.First();
         
            var selectedRole = _roles.ElementAt(index);
           
            await ShowUsersByRoleIdAsync(selectedRole);
            
            StateHasChanged();
        }

        private async Task UpdateUserColumns(string roleType)
        {
            // Clear previous columns
            UsersColumns.Clear();

            // shared columns 
            UsersColumns.Add(new GridColumn { Field = nameof(UserResult.FirstName), HeaderText = "Prénom" });
            UsersColumns.Add(new GridColumn { Field = nameof(UserResult.LastName), HeaderText = "Nom" });
           
            // if role scope is "site", then add SiteName
            if (roleType == "Site")
            {
                UsersColumns.Add(new GridColumn { Field = nameof(UserResult.SiteName), HeaderText = "Site"});
                // if data come from the cache, the site name column is passed after the `action` column.
                // Adding Delay resolve this issue. 
                await Task.Delay(1);
            }
        }
        
}