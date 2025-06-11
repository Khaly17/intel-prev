using System.Globalization;

namespace Soditech.IntelPrev.Mobile.Converters;

/// <summary>
/// Converter that returns true if a string is not null or whitespace.
/// </summary>
public class IsStringNotNullOrWhiteSpaceConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string stringValue)
        {
            return !string.IsNullOrWhiteSpace(stringValue);
        }
        return false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        //TODO: Not yet implemented
        throw new NotSupportedException("ConvertBack is not supported for this converter.");
    }
}