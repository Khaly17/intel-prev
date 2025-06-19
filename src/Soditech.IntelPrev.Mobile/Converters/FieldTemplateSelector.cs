using System;
using System.Globalization;
using Soditech.IntelPrev.Reports.Shared.RegisterFields;
using Microsoft.Maui.Controls;
using Soditech.IntelPrev.Reports.Shared.Enums;

namespace Soditech.IntelPrev.Mobile.Converters;

public class FieldTemplateSelector : DataTemplateSelector, IValueConverter
{
    public DataTemplate TextFieldTemplate { get; set; }
    public DataTemplate BooleanFieldTemplate { get; set; }
    public DataTemplate DateFieldTemplate { get; set; }
    public DataTemplate NumberFieldTemplate { get; set; }

    protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
    {
        var field = item as CreateReportFieldCommand;
        if (field == null) return null;

        // Using the FieldType enum values from Shared library
        switch (field.FieldType)
        {
            case nameof(FieldType.Text):
                return TextFieldTemplate;
            case nameof(FieldType.Boolean):
                return BooleanFieldTemplate;
            case nameof(FieldType.Date):
                return DateFieldTemplate;
            case nameof(FieldType.Number):
                return NumberFieldTemplate;
            default:
                return TextFieldTemplate;
        }
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is CreateReportFieldCommand field)
        {
            // Since we're using native MAUI controls, just return the field
            return field;
        }
        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        //TODO: Not yet implemented
        throw new NotSupportedException("ConvertBack is not supported for this converter.");
    }
}