using System.Globalization;

namespace Soditech.IntelPrev.Mobile.Localization;

public static class L
{
    public static string Localize(string text)
    {
        return LocalizeInternal(text);
    }

    public static string Localize(string text, params object[] args)
    {
        return string.Format(LocalizeInternal(text), args);
    }

    public static string LocalizeWithThreeDots(string text, params object[] args)
    {
        var localizedText = Localize(text, args);
        return CultureInfo.CurrentCulture.TextInfo.IsRightToLeft ? "..." + localizedText : localizedText + "...";
    }

    public static string LocalizeWithParantheses(string text, object valueWithinParentheses, params object[] args)
    {
        var localizedText = Localize(text);
        return CultureInfo.CurrentCulture.TextInfo.IsRightToLeft
            ? " (" + valueWithinParentheses + ")" + localizedText
            : localizedText + " (" + valueWithinParentheses + ")";
    }

    private static string LocalizeInternal(string text)
    {
        //TODO: Implement localization
        return text;
    }
}