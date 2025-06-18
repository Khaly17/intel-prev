using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace Soditech.IntelPrev.Mobile.Converters;

/// <summary>
/// Converts a boolean to an integer (1 for true, 0 for false)
/// </summary>
public class BoolToIntConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is bool and true ? 1 : 0;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int intValue)
        {
            return intValue > 0;
        }
        return false;
    }
}