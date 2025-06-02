using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Soditech.IntelPrev.Mobile.ViewModels.Base;

namespace Soditech.IntelPrev.Mobile.ViewModels.Preventions.Materials;

public partial class MaterialsViewModel : MauiViewModel
{
    private ObservableCollection<MaterialItem> _materials;
    public ObservableCollection<MaterialItem> Materials
    {
        get => _materials;
        set => SetProperty(ref _materials, value);
    }

    public ICommand PageAppearingCommand => new AsyncRelayCommand(async () => await InitializeAsync());

    public MaterialsViewModel()
    {
        _materials = new ObservableCollection<MaterialItem>();
    }

    public override async Task InitializeAsync()
    {
        await SetBusyAsync(() =>
        {
            InitializeCollections();
            return Task.CompletedTask;
        });
    }

    private void InitializeCollections()
    {
        Materials = new ObservableCollection<MaterialItem>
        {
            new MaterialItem
            {
                Title = "Les DAE",
                Description = "Un Défibrillateur Automatisé Externe (DAE) est un dispositif médical qui aide à la réanimation de victimes d'arrêt cardiaque.",
                ImageSource = "dae.jpg"
            },
            new MaterialItem
            {
                Title = "Les trousses de secours",
                Description = "Une trousse de secours ou trousse de premiers soins est un contenant où l'on retrouve des dispositifs médicaux permettant d'effectuer les premiers soins en cas de blessures, douleurs ou autres traumatismes.",
                ImageSource = "firstaid.jpg"
            },
            new MaterialItem
            {
                Title = "Les EPI",
                Description = "Les équipements de protection individuelle (EPI) sont destinés à protéger le travailleur contre un ou plusieurs risques professionnels. Leur utilisation ne doit être envisagée qu'en complément des autres mesures d'élimination ou de réduction des risques.",
                ImageSource = "ppe.jpg"
            }
        };
    }
}

public class MaterialItem
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageSource { get; set; }
}
