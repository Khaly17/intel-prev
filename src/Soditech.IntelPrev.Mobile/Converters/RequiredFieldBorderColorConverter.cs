using System;
using System.Globalization;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Soditech.IntelPrev.Mobile.Converters;

/// <summary>
/// Converts a boolean indicating if a required field is unfilled to a border color.
/// </summary>
public class RequiredFieldBorderColorConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is bool and true ? Colors.Red : Colors.Transparent;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        //TODO: Not yet implemented
        throw new NotSupportedException("ConvertBack is not supported for this converter.");

    }
}