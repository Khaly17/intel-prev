using System;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using System.Windows.Input;
using Soditech.IntelPrev.Mobile.ViewModels.Base;

namespace Soditech.IntelPrev.Mobile.ViewModels.Incendie;

public partial class FireInstructionsViewModel : MauiViewModel
{
    public static string OverviewTitle => "Les consignes de sécurité incendie";

    private string _customInstructions;

    public string CustomInstructions
    {
        get => _customInstructions;
        set => SetProperty(ref _customInstructions, value);
    }

    public ICommand PageAppearingCommand => new AsyncRelayCommand(async () => await InitializeAsync());
    private async Task LoadCustomInstructions()
    {
        try
        {
            await SetBusyAsync(async () =>
            {
                // TODO: Replace with actual API call
                // Mock API response
                CustomInstructions = "Instructions personnalisées pour le site:\n\n" +
                                     "• En cas d'incendie, utiliser l'escalier B pour l'évacuation\n" +
                                     "• Point de rassemblement: Parking Nord\n" +
                                     "• Numéro d'urgence interne: 555\n" +
                                     "• Responsable sécurité: M. Durant - Poste 123";
            }, "Chargement des consignes personnalisées...");
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(ex);
        }
    }

    public override async Task InitializeAsync()
    {
        await LoadCustomInstructions();

    }
}