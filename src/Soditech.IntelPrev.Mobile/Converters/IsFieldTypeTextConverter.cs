using System;
using System.Globalization;
using Microsoft.Maui.Controls;
using Soditech.IntelPrev.Reports.Shared.Enums;

namespace Soditech.IntelPrev.Mobile.Converters;

public class IsFieldTypeTextConverter : IValueConverter
{
    /// <inheritdoc />
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string str)
        {
            return str == nameof(FieldType.Text);
        }
        return false;
    }

    /// <inheritdoc />
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool b)
        {
            return b ? nameof(FieldType.Text) : null;
        }
        return null;
    }
}