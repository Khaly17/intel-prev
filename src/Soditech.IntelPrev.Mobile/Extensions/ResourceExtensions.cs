using Microsoft.Maui.Graphics;
using Soditech.IntelPrev.Mobile.Helpers;

namespace Soditech.IntelPrev.Mobile.Extensions
{
    /// <summary>
    /// Extensions pour accéder facilement aux ressources de l'application
    /// </summary>
    public static class ResourceExtensions
    {
        /// <summary>
        /// Obtient une couleur à partir des ressources de l'application
        /// </summary>
        /// <param name="resourceKey">Clé de la ressource couleur</param>
        /// <returns>La couleur correspondante</returns>
        public static Color GetColor(string resourceKey)
        {
            if (Application.Current?.Resources?.TryGetValue(resourceKey, out var color) == true && color is Color resourceColor)
            {
                return resourceColor;
            }

            // Fallback sur les couleurs définies dans ThemeHelper
            return ThemeHelper.GetColorByName(resourceKey);
        }

        /// <summary>
        /// Obtient un brush à partir des ressources de l'application
        /// </summary>
        /// <param name="resourceKey">Clé de la ressource brush</param>
        /// <returns>Le brush correspondant</returns>
        public static Brush GetBrush(string resourceKey)
        {
            if (Application.Current?.Resources?.TryGetValue($"{resourceKey}Brush", out var brush) == true && brush is Brush resourceBrush)
            {
                return resourceBrush;
            }

            // Créer un brush à partir de la couleur
            var color = GetColor(resourceKey);
            return new SolidColorBrush(color);
        }
    }
}
