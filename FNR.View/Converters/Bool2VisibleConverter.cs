using System;
using System.Globalization;
using System.Windows;

namespace FNR.View
{
    public class Bool2VisibleConverter : BaseValueConverter<Bool2VisibleConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
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

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
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
