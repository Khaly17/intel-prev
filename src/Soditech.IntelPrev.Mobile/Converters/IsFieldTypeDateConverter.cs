using System.Globalization;
using Soditech.IntelPrev.Reports.Shared.Enums;

namespace Soditech.IntelPrev.Mobile.Converters;

public class IsFieldTypeDateConverter : IValueConverter
{
    /// <inheritdoc />
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string str)
        {
            return str == FieldType.Date.ToString();
        }
        return false;
    }

    /// <inheritdoc />
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool b)
        {
            return b ? FieldType.Date.ToString() : null;
        }
        return null;
    }
}