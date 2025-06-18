using Soditech.IntelPrev.Mobile.Core.Dependency;
using Soditech.IntelPrev.Mobile.ViewModels.Base;
using Soditech.IntelPrev.Proxy;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Soditech.IntelPrev.Prevensions.Shared;
using Soditech.IntelPrev.Prevensions.Shared.PreventionSetting;

namespace Soditech.IntelPrev.Mobile.ViewModels.ProPrev;

class MyLibraryViewModel : MauiViewModel
{
    private readonly IProxyService _proxyClientService = DependencyResolver.GetRequiredService<IProxyService>();
    private string _definitionText = "La prévention des risques professionnels, c’est l’ensemble des dispositions à mettre \r\nEn œuvre pour préserver la santé et la sécurité des salariés,  \r\naméliorer les conditions de travail et tendre au bien-être au travail.";
    private bool _isExpanded = false;

    public override async Task InitializeAsync()
    {
        IsBusy = true;
        await GetContentAsync();
        IsBusy = false;
    }

    public string DefinitionText
    {
        get => _definitionText;
        set
        {
            if (_definitionText == value) return;
            _definitionText = value;
            _definitionText = $"<html><head><style>body {{ font-size: 18px; color: #424242; line-height: 1.6; }}</style></head><body>{_definitionText}</body></html>";
            OnPropertyChanged();
        }
    }

    public bool IsExpanded
    {
        get => _isExpanded;
        set
        {
            if (_isExpanded == value) return;
            _isExpanded = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsCollapsed));
            OnPropertyChanged(nameof(ToggleText));
        }
    }

    public bool IsCollapsed => !_isExpanded;
    public string ToggleText => IsExpanded ? "Voir moins" : "Voir plus...";
    public ICommand ToggleCommand => new Command(() => IsExpanded = !IsExpanded);
    public ICommand PageAppearingCommand => new AsyncRelayCommand(async () => await InitializeAsync());

    private async Task GetContentAsync()
    {
        var result = await _proxyClientService.GetAsync<PreventionContentResult>(PreventionRoutes.ProPrevSettings.GetMyLibraryContent);
        if (result.IsSuccess)
        {
            DefinitionText = result.Value.Content;
        }
        else
        {
            DefinitionText = "Aucune donnée disponible.";
        }
    }
}