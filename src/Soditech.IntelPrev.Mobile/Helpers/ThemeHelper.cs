using System;
using Microsoft.Maui.Graphics;

namespace Soditech.IntelPrev.Mobile.Helpers
{
    /// <summary>
    /// Fournit un accès centralisé aux couleurs et thèmes de l'application
    /// </summary>
    public static class ThemeHelper
    {
        // Couleurs principales du logo IntelPrev
        public static readonly Color FireRed = Color.FromRgb(214, 40, 40);         // #D62828 - Bande gauche du logo
        public static readonly Color DeepSkyBlue = Color.FromRgb(0, 63, 136);      // #003F88 - Contour hexagonal "P"
        public static readonly Color PureWhite = Color.FromRgb(255, 255, 255);     // #FFFFFF - Arrière-plan général
        public static readonly Color CharcoalGray = Color.FromRgb(47, 47, 47);     // #2F2F2F - Texte "IntelPrev"
        public static readonly Color LightGray = Color.FromRgb(232, 232, 232);     // #E8E8E8 - Ombres/bordures

        // Couleurs sémantiques pour l'interface utilisateur
        public static readonly Color Primary = DeepSkyBlue;          // Couleur principale
        public static readonly Color PrimaryDark = Color.FromRgb(0, 42, 91);  // Version plus foncée de DeepSkyBlue
        public static readonly Color Secondary = FireRed;            // Couleur secondaire
        public static readonly Color Background = PureWhite;         // Arrière-plan
        public static readonly Color TextPrimary = CharcoalGray;     // Texte principal
        public static readonly Color TextSecondary = Color.FromRgb(116, 116, 116); // Texte secondaire
        public static readonly Color Accent = Color.FromRgb(0, 123, 255);    // Accent pour les éléments interactifs
        public static readonly Color Border = LightGray;             // Bordures
        public static readonly Color Success = Color.FromRgb(40, 167, 69);   // Messages de succès
        public static readonly Color Warning = Color.FromRgb(255, 193, 7);   // Avertissements
        public static readonly Color Danger = FireRed;               // Erreurs et dangers
        public static readonly Color Info = Color.FromRgb(23, 162, 184);     // Informations

        // Obtenir la couleur par son nom (pour la réflexion et le binding)
        public static Color GetColorByName(string colorName)
        {
            return colorName switch
            {
                "FireRed" => FireRed,
                "DeepSkyBlue" => DeepSkyBlue,
                "PureWhite" => PureWhite,
                "CharcoalGray" => CharcoalGray,
                "LightGray" => LightGray,
                "Primary" => Primary,
                "PrimaryDark" => PrimaryDark,
                "Secondary" => Secondary,
                "Background" => Background,
                "TextPrimary" => TextPrimary,
                "TextSecondary" => TextSecondary,
                "Accent" => Accent,
                "Border" => Border,
                "Success" => Success,
                "Warning" => Warning,
                "Danger" => Danger,
                "Info" => Info,
                _ => Colors.Transparent
            };
        }

        // Convertir une couleur en chaîne hexadécimale
        public static string ToHex(this Color color)
        {
            return $"#{(int)(color.Red * 255):X2}{(int)(color.Green * 255):X2}{(int)(color.Blue * 255):X2}";
        }
    }
}
