using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Soditech.IntelPrev.Mobile.Views.Incendie;
using Soditech.IntelPrev.Mobile.ViewModels.Base;

namespace Soditech.IntelPrev.Mobile.ViewModels.Incendie
{
    public partial class FireSafetyViewModel : MauiViewModel
    {
        private string _fireSafetyDescription;
        private string safetyPrinciples;
        private string safetyCauses;
        private bool _isOverviewExpanded = false;
        private bool _isPrinciplesExpanded = false;
        private bool _isCausesExpanded = false;

        public string OverviewTitle => "Sécurité Incendie";

        #region Text Content Properties
        public string FireSafetyDescription
        {
            get => _fireSafetyDescription;
            set
            {
                if (_fireSafetyDescription == value)
                    return;
                _fireSafetyDescription = value;
                OnPropertyChanged();
            }
        }

        public string SafetyPrinciples
        {
            get => safetyPrinciples;
            set
            {
                if (safetyPrinciples == value)
                    return;
                safetyPrinciples = value;
                OnPropertyChanged();
            }
        }

        public string SafetyCauses
        {
            get => safetyCauses;
            set
            {
                if (safetyCauses == value)
                    return;
                safetyCauses = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Expandable Text Properties

        // Overview section
        public bool IsOverviewExpanded
        {
            get => _isOverviewExpanded;
            set
            {
                _isOverviewExpanded = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsOverviewCollapsed));
                OnPropertyChanged(nameof(OverviewToggleText));
            }
        }

        public bool IsOverviewCollapsed => !IsOverviewExpanded;

        public string OverviewToggleText => IsOverviewExpanded ? "Voir moins" : "Voir plus...";

        // Safety Principles section
        public bool IsPrinciplesExpanded
        {
            get => _isPrinciplesExpanded;
            set
            {
                _isPrinciplesExpanded = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsPrinciplesCollapsed));
                OnPropertyChanged(nameof(PrinciplesToggleText));
            }
        }

        public bool IsPrinciplesCollapsed => !IsPrinciplesExpanded;

        public string PrinciplesToggleText => IsPrinciplesExpanded ? "Voir moins" : "Voir plus...";

        // Safety Causes section
        public bool IsCausesExpanded
        {
            get => _isCausesExpanded;
            set
            {
                _isCausesExpanded = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsCausesCollapsed));
                OnPropertyChanged(nameof(CausesToggleText));
            }
        }

        public bool IsCausesCollapsed => !IsCausesExpanded;

        public string CausesToggleText => IsCausesExpanded ? "Voir moins" : "Voir plus...";
        #endregion

        #region Commands
        public ICommand PageAppearingCommand => new AsyncRelayCommand(async () => await InitializeAsync());

        public ICommand ToggleOverviewCommand => new Command(() => IsOverviewExpanded = !IsOverviewExpanded);

        public ICommand TogglePrinciplesCommand => new Command(() => IsPrinciplesExpanded = !IsPrinciplesExpanded);

        public ICommand ToggleCausesCommand => new Command(() => IsCausesExpanded = !IsCausesExpanded);
        #endregion

        public FireSafetyViewModel(string fireSafetyDescription, string safetyPrinciples, string safetyCauses)
        {
            _fireSafetyDescription = fireSafetyDescription;
            this.safetyPrinciples = safetyPrinciples;
            this.safetyCauses = safetyCauses;
            InitializeCollections();
        }

        private void InitializeCollections()
        {
            FireSafetyDescription = "La prévention des risques incendies s'inscrit dans une démarche globale des risques professionnels," +
                                    " elle se compose de lois, d'arrété, de réglement, de norme, de consigne, de disposition etc.\r\n\r\nElle vise à prévenir tout départ de feu dans un établissement," +
                                    " afin de protéger les personnes et les biens. Sachant que le risque zéro n'existe pas, cette prévention des risques a pour but en cas de départ de feu," +
                                    " De limiter sa propagation, faciliter l'évacuation des occupant et de facilter I'intervention des services de Secours,\r\n\r\nLa sécurité incendie en entreprise est une des obligations qui incombe aux dirigeants d'entreprises, collectivités territoriales, structures hospitaliéres etc... " +
                                    "considérés comme les premiers responsables de la sécurité des personnes et des biens.";

            SafetyPrinciples = "-\tÉviter l'éclosion d'un feu\r\nDe nombreux moyens peuvent être pris afin de réduire les causes de feu comme :Un contrôle régulier des installations techniques , Portez une attention particulière aux équipements de cuisson et aux objets chauffants.\r\n\r\n-\tFavoriser l'évacuation des personnes\r\nCela consiste à mettre en place des mesures permettant d'assurer l'évacuation rapide et sans danger de toutes les personnes présentes. \r\nLa mise en place et l'entretien d'alarmes incendie, de détecteurs de fumée et de monoxyde de carbone.\r\n\r\n-\tLimiter la propagation du feu\r\nDivers moyens doivent être mis en place pour limiter la diffusion du feu.\r\n\r\n-\tFaciliter l'intervention des secours\r\nMettre en place des mesures favorisant l'action des secours (utilisation des moyens de lutte comme les extincteurs et autres équipements de prévention des incendies.\r\n\r\n - Formation du personnel\r\n- Les rondes préventives\r\n";

            SafetyCauses = "Les causes énergétiques :\r\nSouvent les incendies sont causés par un dysfonctionnement des appareils électriques, de chauffages, des travaux par point chaud, etc….Il peut s'agir d'étincelles ou de non-conformité d'une installation électrique (elles représentent  environ 30 %  des incendies)\r\nLes causes humaines :\r\nUn incendie peut survenir à cause d'une négligence humaine et elle  peut être accidentelle ou volontaire.\r\nLes causes naturelles :\r\nLes conditions climatiques peuvent parfois être une source de départ d'incendie, notamment par le soleil, la foudre, etc…\r\n";
        }

        public override async Task InitializeAsync()
        {
            // Use await to ensure we're properly implementing the async method
            await Task.CompletedTask;
            InitializeCollections();
        }
    }

    public class FireSafetyPrinciples
    {
        public required string Name { get; set; }
        public required string Content { get; set; }
    }

    public class SafetyTip
    {
        public required string TipText { get; set; }
        public required string TutorialLink { get; set; }
    }
}
