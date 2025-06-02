using System.Globalization;
using Soditech.IntelPrev.Reports.Shared.RegisterFieldGroups;

namespace Soditech.IntelPrev.Mobile.Converters;

public class IsCreateReportFieldGroupCommandConverter : IValueConverter
{
    /// <inheritdoc />
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is CreateReportFieldGroupCommand;
    }

    /// <inheritdoc />
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool b)
        {
            return b ? new CreateReportFieldGroupCommand() : null;
        }
        return null;
    }
}