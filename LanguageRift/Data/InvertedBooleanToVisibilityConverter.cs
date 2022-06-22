using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace LanguageRift.Data;

[ValueConversion(typeof(bool), typeof(Visibility))]
public class InvertedBooleanToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        value is false ? Visibility.Visible : Visibility.Collapsed;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        value is Visibility.Visible;
}