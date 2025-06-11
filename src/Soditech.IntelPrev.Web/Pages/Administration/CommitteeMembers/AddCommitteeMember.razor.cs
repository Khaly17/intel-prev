using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Soditech.IntelPrev.Users.Shared;
using Soditech.IntelPrev.Users.Shared.Roles;
using Soditech.IntelPrev.Web.Models;
using Soditech.IntelPrev.Web.Models.Utils;
using Soditech.IntelPrev.Preventions.Shared.CommitteeMembers;
using Soditech.IntelPrev.Preventions.Shared;

namespace Soditech.IntelPrev.Web.Pages.Administration.CommitteeMembers
{
    public partial class AddCommitteeMember : ComponentBase
    {
        public CommitteeMemberResult NewCommitteeMember { get; set; } = new();

        public IEnumerable<RoleModel> _roles { get; set; } = new List<RoleModel>();

        public string? errorMessage { get; set; }

        public string? successMessage { get; set; }

        private List<Guid> SelectedRoles { get; set; } = [];

        [Inject] private ILogger<AddCommitteeMember> Logger { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            await LoadRoles();
        }

        private async Task CreateCommitteeMember()
        {
            errorMessage = null;
            successMessage = null;
            try
            {
                var result = await ProxyService.PostAsync<CommitteeMemberResult>(PreventionRoutes.CommitteeMembers.Create, NewCommitteeMember);

                if (result.IsSuccess)
                {
                    successMessage = "Le membre du comité a été ajouté avec succès !";
                    Navigation.NavigateTo("/committeemembers");
                }
                else
                {
                    errorMessage = result.Error?.Message ?? "Une erreur est survenue lors de la création du membre du comité.";
                    Logger.LogError("{code} : {message}", result.Error?.Code, result.Error?.Message);
                }
            }
            catch (Exception ex)
            {
                errorMessage = "Error: Cannot create committee member";
                Logger.LogError(ex, errorMessage);
            }
        }

        private async Task LoadRoles()
        {
            try
            {
                var response = await ProxyService.GetAsync<IEnumerable<RoleResult>>(UserRoutes.Roles.GetAll);
                if (response.IsSuccess)
                {
                    _roles = response.Value.Select(r => new RoleModel
                    {
                        Id = r.Id,
                        Name = r.Name,
                        IsSelected = false
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                errorMessage = " Error: Cannot load roles";
                Logger.LogError(ex, errorMessage);
            }
        }

    }
}