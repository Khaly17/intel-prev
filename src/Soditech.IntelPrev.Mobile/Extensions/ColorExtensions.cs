using Microsoft.Maui.Graphics;
using System;

namespace Soditech.IntelPrev.Mobile.Extensions;

/// <summary>
/// Extensions pour faciliter le travail avec les couleurs
/// </summary>
public static class ColorExtensions
{
    /// <summary>
    /// Crée une couleur avec une opacité modifiée
    /// </summary>
    /// <param name="color">Couleur d'origine</param>
    /// <param name="opacity">Nouvelle opacité entre 0 et 1</param>
    /// <returns>Couleur avec l'opacité modifiée</returns>
    public static Color WithOpacity(this Color color, float opacity)
    {
        return new Color(color.Red, color.Green, color.Blue, opacity);
    }
        
    /// <summary>
    /// Convertit une couleur en chaîne hexadécimale
    /// </summary>
    /// <param name="color">Couleur à convertir</param>
    /// <returns>Chaîne hexadécimale</returns>
    public static string ToHex(this Color color)
    {
        return $"#{(int)(color.Red * 255):X2}{(int)(color.Green * 255):X2}{(int)(color.Blue * 255):X2}";
    }
        
    /// <summary>
    /// Crée une couleur légèrement plus claire
    /// </summary>
    /// <param name="color">Couleur d'origine</param>
    /// <param name="factor">Facteur d'éclaircissement (0-1)</param>
    /// <returns>Couleur plus claire</returns>
    public static Color Lighter(this Color color, float factor = 0.1f)
    {
        return new Color(
            Math.Min(1, color.Red + (1 - color.Red) * factor),
            Math.Min(1, color.Green + (1 - color.Green) * factor),
            Math.Min(1, color.Blue + (1 - color.Blue) * factor),
            color.Alpha);
    }
        
    /// <summary>
    /// Crée une couleur légèrement plus foncée
    /// </summary>
    /// <param name="color">Couleur d'origine</param>
    /// <param name="factor">Facteur d'assombrissement (0-1)</param>
    /// <returns>Couleur plus foncée</returns>
    public static Color Darker(this Color color, float factor = 0.1f)
    {
        return new Color(
            Math.Max(0, color.Red - color.Red * factor),
            Math.Max(0, color.Green - color.Green * factor),
            Math.Max(0, color.Blue - color.Blue * factor),
            color.Alpha);
    }
}