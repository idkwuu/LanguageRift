using System;
using System.Windows.Controls;
using System.Windows.Data;
using Windows.UI.Xaml;

namespace LoL_Language
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class InvertBooleanToVisibilityConverter : IValueConverter
    {
        #region IValueConverter Members
        
        public IValueConverter VisibilityConverter { get; set; }

        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (value is not bool data)
                throw new InvalidOperationException("need a bool");

            return VisibilityConverter.Convert(!data, targetType, parameter, culture);
        }
        
        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
        
        #endregion
    }
}