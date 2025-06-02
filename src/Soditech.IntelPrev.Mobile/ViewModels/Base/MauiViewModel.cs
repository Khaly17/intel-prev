using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Controls.UserDialogs.Maui;
using Soditech.IntelPrev.Mobile.Core.Dependency;
using Soditech.IntelPrev.Mobile.Localization;
using Soditech.IntelPrev.Users.Shared.Users;

namespace Soditech.IntelPrev.Mobile.ViewModels.Base
{
    public abstract class MauiViewModel  : ObservableObject, ITransientDependency
    {
        protected UserResult? _currentUser;
        private bool _isBusy;
        // protected readonly INavigationService NavigationService;
        // protected readonly IModalService ModalService;
        // public IMapper ObjectMapper { get; set; }
        public bool IsNotBusy => !IsBusy;

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsNotBusy));
            }
        }

      
        public virtual async Task InitializeAsync(object navigationData)
        {
            await Task.FromResult(false);
        }
        public virtual async Task InitializeAsync()
        {
            await Task.FromResult(false);
        }


        protected async Task SetBusyAsync(Func<Task> func, string? loadingMessage = null)
        {
            loadingMessage ??= L.Localize("LoadWithThreeDot");

            UserDialogs.Instance.ShowLoading(loadingMessage, MaskType.None);
            IsBusy = true;

            try
            {
                await func();
            }
            finally
            {
                UserDialogs.Instance.HideHud();
                IsBusy = false;
            }
        }

        public async Task HandleExceptionAsync(Exception ex)
        {
            // Handle exception by showing a dialog 
            await Shell.Current.DisplayAlert("Erreur", "Impossible d'obtenir votre position", "OK");
        }
    }
}