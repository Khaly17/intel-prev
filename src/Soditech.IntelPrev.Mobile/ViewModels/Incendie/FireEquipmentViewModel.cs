using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Soditech.IntelPrev.Mobile.ViewModels.Base;

namespace Soditech.IntelPrev.Mobile.ViewModels.Incendie;

public partial class FireEquipmentViewModel : MauiViewModel
{
    private ObservableCollection<FireEquipmentItem> _equipmentItems;

    public ObservableCollection<FireEquipmentItem> EquipmentItems
    {
        get => _equipmentItems;
        set => SetProperty(ref _equipmentItems, value);
    }

    public static string OverviewTitle => "Le matériel incendie";

    public ICommand PageAppearingCommand => new AsyncRelayCommand(async () => await InitializeAsync());

    public FireEquipmentViewModel(ObservableCollection<FireEquipmentItem> equipmentItems)
    {
        _equipmentItems = equipmentItems;
        InitializeCollections();
    }

    public FireEquipmentViewModel()
    {
    }

    private void InitializeCollections()
    {
        EquipmentItems =
        [
            new()
            {
                Title = "Le matériel incendie !",
                Description =
                    "Le matériel disponible et mis en place pour la lutte contre les feux. Le matériel qui est le plus souvent utilisé pour combattre les départs de feu (extincteur, RAI , couvertures anti-feu). Un feu peut avoir des conséquences dramatiques pour une entreprise et son personnel. A NOTER QUE LE MATERIEL PRESENTE EST LE PLUS COURANT, IL EST POSSIBLE QUE CERTAINS MATERIELS SOIENT PLUS TECHNIQUES ET PLUS SOPHISTIQUES DANS CERTAINES ENTREPRISES.",
                ImageSource = "equipments.jpg"
            },

            new()
            {
                Title = "Extincteur",
                Description =
                    "L'extincteur est un appareil de lutte contre un feu naissant avec pour but d'éviter ou de stopper sa propagation. IL EST CONSEILLE DE FAIRE UNE FORMATION DE MANIPULATION DES EXTINCTEURS !",
                ImageSource = "fireequipment.jpg"
            },

            new()
            {
                Title = "Robinet d'incendie armé",
                Description =
                    "Un robinet d'incendie armé, ou RIA, est un équipement de première intervention, alimenté en eau, pour la lutte contre les départs de feu. Il est utilisable par toutes les personnes, qualifiées ou non, en attendant si nécessaire l'arrivée des sapeurs-pompiers.",
                ImageSource = "robinet.jpg"
            },

            new()
            {
                Title = "Le bac à sable contre l'incendie",
                Description =
                    "Le bac à sable est un objet de lutte contre le départ feu. Il est simple d'utilisation, peut-être de taille et composition variable, est très efficace pour arrêter un début de feu et peut même être obligatoire dans certains endroits à connaître.",
                ImageSource = "bacasable.jpg"
            },

            new()
            {
                Title = "Le bloc déclencheur manuel d'incendie",
                Description =
                    "Un déclencheur manuel d'alarme incendie (DMA) est un appareil qui permet de signaler la présence d'un feu. En cas de départ de feu, vous avez la possibilité de le déclencher en appuyant / percutant sur le point noir.",
                ImageSource = "declencheur.jpg"
            },

            new()
            {
                Title = "La couverture anti feu",
                Description =
                    "Les couvertures anti-feu sont des produits qui sont conçus pour fournir une protection contre le feu à tous ceux qui sont présents dans une zone où un foyer d'incendie.",
                ImageSource = "couverture.jpg"
            },

            new()
            {
                Title = "Le désenfumage",
                Description =
                    "Le désenfumage consiste à évacuer une partie des fumées produites par l'incendie en créant une hauteur d'air libre sous la couche de fumée par l'action de cette trappe.",
                ImageSource = "desenfumage.jpg"
            }
        ];
    }

    public override async Task InitializeAsync()
    {
        await SetBusyAsync(() =>
        {
            InitializeCollections();
            return Task.CompletedTask;
        });
    }
}

public class FireEquipmentItem
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageSource { get; set; }
}