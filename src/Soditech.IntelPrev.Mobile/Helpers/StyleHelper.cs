using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Soditech.IntelPrev.Mobile.Extensions;

namespace Soditech.IntelPrev.Mobile.Helpers
{
    /// <summary>
    /// Classe d'aide pour appliquer des styles cohérents aux éléments UI
    /// </summary>
    public static class StyleHelper
    {
        // Boutons
        public static void ApplyPrimaryButtonStyle(Button button)
        {
            button.BackgroundColor = ResourceExtensions.GetColor("Primary");
            button.TextColor = Colors.White;
            button.FontAttributes = FontAttributes.Bold;
            button.CornerRadius = 8;
            button.Padding = new Thickness(16, 10);
        }

        public static void ApplySecondaryButtonStyle(Button button)
        {
            button.BackgroundColor = ResourceExtensions.GetColor("Secondary");
            button.TextColor = Colors.White;
            button.FontAttributes = FontAttributes.Bold;
            button.CornerRadius = 8;
            button.Padding = new Thickness(16, 10);
        }

        public static void ApplyOutlineButtonStyle(Button button)
        {
            button.BackgroundColor = Colors.Transparent;
            button.TextColor = ResourceExtensions.GetColor("Primary");
            button.BorderColor = ResourceExtensions.GetColor("Primary");
            button.BorderWidth = 1;
            button.CornerRadius = 8;
            button.Padding = new Thickness(16, 10);
        }

        // Labels
        public static void ApplyTitleStyle(Label label)
        {
            label.TextColor = ResourceExtensions.GetColor("TextPrimary");
            label.FontSize = 24;
            label.FontAttributes = FontAttributes.Bold;
        }

        public static void ApplySubtitleStyle(Label label)
        {
            label.TextColor = ResourceExtensions.GetColor("TextPrimary");
            label.FontSize = 18;
            label.FontAttributes = FontAttributes.Bold;
        }

        public static void ApplyBodyTextStyle(Label label)
        {
            label.TextColor = ResourceExtensions.GetColor("TextPrimary");
            label.FontSize = 16;
        }

        public static void ApplySecondaryTextStyle(Label label)
        {
            label.TextColor = ResourceExtensions.GetColor("TextSecondary");
            label.FontSize = 14;
        }

        // Frames
        public static void ApplyCardStyle(Frame frame)
        {
            frame.BackgroundColor = Colors.White;
            frame.BorderColor = ResourceExtensions.GetColor("Border");
            frame.CornerRadius = 8;
            frame.HasShadow = true;
            frame.Padding = new Thickness(16);
            frame.Margin = new Thickness(0, 0, 0, 16);
        }

        // Entry
        public static void ApplyStandardEntryStyle(Entry entry)
        {
            entry.BackgroundColor = Colors.White;
            entry.TextColor = ResourceExtensions.GetColor("TextPrimary");
            entry.PlaceholderColor = ResourceExtensions.GetColor("TextSecondary");
            entry.FontSize = 16;
        }
    }
}
