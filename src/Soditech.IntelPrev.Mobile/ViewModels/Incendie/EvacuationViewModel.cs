using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using Soditech.IntelPrev.Mobile.ViewModels.Base;

namespace Soditech.IntelPrev.Mobile.ViewModels.Incendie;

public partial class EvacuationViewModel : MauiViewModel
{
    private string _beforeEvacuationInfo;
    private string _fireStartInfo;
    private string _evacuationProcedure;
    private string _importantNotes;
    private string _evacuationTeam;

    public static string OverviewTitle => "En cas d'évacuation";

    public string BeforeEvacuationInfo
    {
        get => _beforeEvacuationInfo;
        set
        {
            if (_beforeEvacuationInfo == value) return;
            _beforeEvacuationInfo = value;
            OnPropertyChanged();
        }
    }

    public string FireStartInfo
    {
        get => _fireStartInfo;
        set
        {
            if (_fireStartInfo == value) return;
            _fireStartInfo = value;
            OnPropertyChanged();
        }
    }

    public string EvacuationProcedure
    {
        get => _evacuationProcedure;
        set
        {
            if (_evacuationProcedure == value) return;
            _evacuationProcedure = value;
            OnPropertyChanged();
        }
    }

    public string ImportantNotes
    {
        get => _importantNotes;
        set
        {
            if (_importantNotes == value) return;
            _importantNotes = value;
            OnPropertyChanged();
        }
    }

    public string EvacuationTeam
    {
        get => _evacuationTeam;
        set
        {
            if (_evacuationTeam == value) return;
            _evacuationTeam = value;
            OnPropertyChanged();
        }
    }

    public ICommand PageAppearingCommand => new AsyncRelayCommand(async () => await InitializeAsync());

    public EvacuationViewModel(string beforeEvacuationInfo, string fireStartInfo, string evacuationProcedure, string importantNotes, string evacuationTeam)
    {
        _beforeEvacuationInfo = beforeEvacuationInfo;
        _fireStartInfo = fireStartInfo;
        _evacuationProcedure = evacuationProcedure;
        _importantNotes = importantNotes;
        _evacuationTeam = evacuationTeam;
        InitializeContent();
    }

    private void InitializeContent()
    {
        BeforeEvacuationInfo = "AVANT l'EVACUATION :\n\nS'informer et se former pour :\n" +
                               "- Reconnaître le signal d'alarme.\n" +
                               "- Connaître son rôle en cas d'incendie (agent sécurité incendie, guide file, serre file)\n" +
                               "- Connaître les différents cheminements d'évacuation\n" +
                               "- Connaître le(s) point(s) de rassemblement.";

        FireStartInfo = "EN CAS DE DEPART DE FEU :\nLa lutte contre le feu est avant tout une lutte contre le temps !\n\n" +
                        "1) Donner l'alerte :\n" +
                        "- Par l'application INTEL'PREV, VOUS ETES AUTOMATIQUEMENT GEOLOCALISER\n" +
                        "- Par déclenchement manuel OU En appelant le PC Sécurité\n" +
                        "Indiquer le lieu, la nature et l'ampleur du sinistre\n" +
                        "- En présence d'un PC Sécurité sur le site, il ne vous appartient pas d'appeler en premier lieu les secours extérieurs\n\n" +
                        "2) Sur un départ de feu, si la personne s'en sent capable et a reçu une formation adéquate, utiliser les moyens de lutte disponibles (extincteurs, RIA).";

        EvacuationProcedure = "3) Evacuer :\n\n" +
                              "- Fermer les portes et fenêtres.\n" +
                              "- Accorder une attention particulière aux personnes handicapées ou blessées.\n" +
                              "- Se diriger vers les sorties de secours en suivant la signalisation.\n" +
                              "- Utiliser les escaliers et non pas les ascenseurs.\n" +
                              "- Rejoindre le point de rassemblement indiqué sur le plan du bâtiment.\n" +
                              "- Attendre l'accord du coordinateur pour retourner à son poste de travail.";

        ImportantNotes = "Il est important de :\n" +
                         "- Garder son calme (ne pas pousser, ne pas crier « Au feu ! »)\n" +
                         "- Rassurer les personnes qui semblent perdre leur calme.\n" +
                         "- Ne pas prendre le temps de rassembler ses affaires personnelles\n" +
                         "- Ne pas courir.\n" +
                         "- Aviser en route les personnes qui ne semblent pas avoir déjà pris conscience de l'alarme.\n" +
                         "- Ne jamais retourner en arrière.\n" +
                         "- En cas de fumées, se rappeler qu'il est préférable de se baisser\n" +
                         "- Ne jamais obstruer les circulations\n" +
                         "- Ne JAMAIS prendre de risques.";

        EvacuationTeam = "LE GROUPE D'EVACUATION :\n" +
                         "- RESPONSABLE EVACUATION (SSIAP)\n" +
                         "- GUIDE FILE généralement premier (celui qui guide le groupe vers la sortie)\n" +
                         "- SERRE FIL généralement dernier (S'assure que personne ne reste dans les locaux et ferme la marche)";
    }

    public override async Task InitializeAsync()
    {
        InitializeContent();
    }
}