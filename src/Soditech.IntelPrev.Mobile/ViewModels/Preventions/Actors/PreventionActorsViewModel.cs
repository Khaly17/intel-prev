using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Soditech.IntelPrev.Mobile.Core.Dependency;
using Soditech.IntelPrev.Mobile.ViewModels.Base;
using Soditech.IntelPrev.Prevensions.Shared;
using Soditech.IntelPrev.Prevensions.Shared.CommitteeMembers;
using Soditech.IntelPrev.Proxy;

namespace Soditech.IntelPrev.Mobile.ViewModels.Preventions.Actors;

public class PreventionActorsViewModel : MauiViewModel
{
    private bool _isRefreshing;
    public bool IsRefreshing
    {
        get => _isRefreshing;
        set => SetProperty(ref _isRefreshing, value);
    }
    
    private bool _isBusy;
    public bool IsBusy
    {
        get => _isBusy;
        set => SetProperty(ref _isBusy, value);
    }

    private IEnumerable<CommitteeMemberResult> _committeeMembers = default!;
    public IEnumerable<CommitteeMemberResult> CommitteeMembers
    {
        get => _committeeMembers;
        set => SetProperty(ref _committeeMembers, value);
    }

    private CommitteeMemberResult _selectedCommitteeMember;
    public CommitteeMemberResult SelectedCommitteeMember
    {
        get => _selectedCommitteeMember;
        set => SetProperty(ref _selectedCommitteeMember, value);
    }

    private readonly IProxyService _proxyClientService = DependencyResolver.GetRequiredService<IProxyService>();

    public ICommand PageAppearingCommand => new AsyncRelayCommand(async () => await InitializeAsync());

    public ICommand CommitteeMemberSelectedCommand => new RelayCommand<CommitteeMemberResult>(member =>
    {
        if (member != null)
        {
            SelectedCommitteeMember = member;
            // Handle committee member selection if needed
            // Example: Navigate to committee member details or show more info
            // await _navigationService.NavigateToAsync($"{nameof(CommitteeMemberDetailsView)}?id={member.Id}");
        }
    });

    public IAsyncRelayCommand RefreshCommand => new AsyncRelayCommand(async () => await InitializeAsync());

    /// <inheritdoc />
    public override async Task InitializeAsync()
    {
        // get all committee members from the server
        try
        {
            IsBusy = true;
            var committeeMembersResult = await _proxyClientService.GetAsync<IEnumerable<CommitteeMemberResult>>(PreventionRoutes.CommitteeMembers.GetAll);
            if (committeeMembersResult.IsSuccess)
            {
                CommitteeMembers = committeeMembersResult.Value;
            }
            else
            {
                //TODO: handle error
            }
        }
        finally
        {
            IsBusy = false;
        }
    }
}