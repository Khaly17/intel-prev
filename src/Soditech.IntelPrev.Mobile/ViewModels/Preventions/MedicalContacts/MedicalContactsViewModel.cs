using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Soditech.IntelPrev.Mobile.Core.Dependency;
using Soditech.IntelPrev.Mobile.ViewModels.Base;
using Soditech.IntelPrev.Prevensions.Shared;
using Soditech.IntelPrev.Prevensions.Shared.MedicalContacts;
using Soditech.IntelPrev.Proxy;

namespace Soditech.IntelPrev.Mobile.ViewModels.Preventions.MedicalContacts;

public class MedicalContactsViewModel : MauiViewModel
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

    private IEnumerable<MedicalContactResult> _medicalContacts = default!;
    public IEnumerable<MedicalContactResult> MedicalContacts
    {
        get => _medicalContacts;
        set => SetProperty(ref _medicalContacts, value);
    }

    private MedicalContactResult _selectedMedicalContact;
    public MedicalContactResult SelectedMedicalContact
    {
        get => _selectedMedicalContact;
        set => SetProperty(ref _selectedMedicalContact, value);
    }

    private readonly IProxyService _proxyClientService = DependencyResolver.GetRequiredService<IProxyService>();

    public ICommand PageAppearingCommand => new AsyncRelayCommand(async () => await InitializeAsync());

    public ICommand MedicalContactSelectedCommand => new RelayCommand<MedicalContactResult>(contact =>
    {
        if (contact != null)
        {
            SelectedMedicalContact = contact;
            // Handle medical contact selection if needed
            // Example: Navigate to contact details or show more info
            // await _navigationService.NavigateToAsync($"{nameof(MedicalContactDetailsView)}?id={contact.Id}");
        }
    });

    public IAsyncRelayCommand RefreshCommand => new AsyncRelayCommand(async () => await InitializeAsync());

    public override async Task InitializeAsync()
    {
        try
        {
            IsBusy = true;
            var medicalContactsResult = await _proxyClientService.GetAsync<IEnumerable<MedicalContactResult>>(PreventionRoutes.MedicalContacts.GetAll);
            if (medicalContactsResult.IsSuccess)
            {
                MedicalContacts = medicalContactsResult.Value;
            }
            else
            {
                // TODO: Handle error - could add error messaging or logging here
            }
        }
        finally
        {
            IsBusy = false;
        }
    }
}
