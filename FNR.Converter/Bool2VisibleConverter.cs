using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FNR.Converter
{
    public class Bool2VisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if ((bool)value)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if ((Visibility)value == Visibility.Visible)
                    return false;
                else
                    return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
