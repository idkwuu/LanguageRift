using System;
using System.Data;
using System.Windows.Data;

namespace LoL_Language
{
    [ValueConversion(typeof(string), typeof(bool))]
    public class LanguagePropertyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (value is not string s)
                throw new InvalidOperationException("need a string");

            var result = (bool)(s == Properties.Settings.Default.Language);
            return result;
        }
        
        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}