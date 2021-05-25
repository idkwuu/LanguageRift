using System;
using System.Windows.Data;

namespace LoL_Language
{
    [ValueConversion(typeof(string), typeof(bool))]
    public class RegionPropertyConverter : IValueConverter
    {
        #region Variables
        private DataViewModel _dvm = new DataViewModel();
        #endregion
        
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (value is not string s)
                throw new InvalidOperationException("need a string");

            return (bool)(s == _dvm.Region);
        }
        
        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
        
        #endregion
    }
}