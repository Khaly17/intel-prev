using System.Globalization;

namespace Soditech.IntelPrev.Mobile.Converters;

/// <summary>
/// Converts a boolean indicating if a required field is unfilled to a border color.
/// </summary>
public class RequiredFieldBorderColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool isRequiredAndUnfilled && isRequiredAndUnfilled)
        {
            return Colors.Red;
        }
        return Colors.Transparent;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}