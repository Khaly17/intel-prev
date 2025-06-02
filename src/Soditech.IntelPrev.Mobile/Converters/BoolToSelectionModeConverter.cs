using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace Soditech.IntelPrev.Mobile.Converters
{
    public class BoolToSelectionModeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isSelectable)
                return isSelectable ? SelectionMode.Single : SelectionMode.None;

            return SelectionMode.None;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is SelectionMode mode && mode == SelectionMode.Single;
        }
    }
}
