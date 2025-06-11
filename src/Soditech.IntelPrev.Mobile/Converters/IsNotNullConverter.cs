using System.Globalization;

namespace Soditech.IntelPrev.Mobile.Converters;

public class IsNotNullConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value != null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        //Not yet implemented
        throw new NotSupportedException("ConvertBack is not supported for this converter.");
    }
}
